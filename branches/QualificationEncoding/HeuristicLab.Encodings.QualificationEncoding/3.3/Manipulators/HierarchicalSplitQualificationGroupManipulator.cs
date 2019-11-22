#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Random;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("7B00B842-EE4E-4099-A5C4-7A08859A8C68")]
  [Item("HierarchicalSplitQualificationGroupManipulator", "Adds or removes a qualification in one randomly selected group and splits the group size.")]
  public sealed class HierarchicalSplitQualificationGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private HierarchicalSplitQualificationGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private HierarchicalSplitQualificationGroupManipulator(HierarchicalSplitQualificationGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public HierarchicalSplitQualificationGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new HierarchicalSplitQualificationGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      if (random.NextDouble() < 0.5) ApplyAddQualification(encoding, qualification, random);
      else ApplyRemoveQualification(encoding, qualification, random);
    }

    public static void ApplyAddQualification(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      var groups = qualification.Groups.Where(x => x.Key.Count(q => !q) > 0).ToList();
      if (groups.Count == 0) return; // no group where at least one qualification is unset

      // select one group randomly
      var group = groups.SampleRandom(random);
      var pool = group.Key;
      var poolSize = group.Value;

      // collect all unset qualifications
      var unsetQualis = pool.GetFalseIndices().ToList();

      var count = 1 + random.Next(unsetQualis.Count);
      var newPool = pool.GetBuilder().Flip(unsetQualis.Shuffle(random).Take(count)).Build();

      // choose how many workers to assign to the new qualification group
      int num = 1 + random.Next(poolSize);
      qualification.DecreaseOrRemove(pool, num);
      qualification.AddOrIncrease(newPool, num);

      QualificationHelpers.Repair(encoding, qualification, random);
    }

    public static void ApplyRemoveQualification(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      var groups = qualification.Groups.Where(x => x.Key.Count(q => q) > 1).ToList();
      if (groups.Count == 0) return; // no group where at least two qualifications are set

      // select one group randomly
      var group = groups.SampleRandom(random);
      var pool = group.Key;
      var poolSize = group.Value;

      // collect all set qualifications
      var setQualis = pool.GetTrueIndices().ToList();

      var count = random.Next(1, setQualis.Count);
      var newPool = pool.GetBuilder().Flip(setQualis.Shuffle(random).Take(count)).Build();

      // choose how many workers to assign to the new qualification group
      int num = 1 + random.Next(group.Value);
      qualification.DecreaseOrRemove(pool, num);
      qualification.AddOrIncrease(newPool, num);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
