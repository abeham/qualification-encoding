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
  [StorableType("B6D74139-2848-4098-853A-34A578B16323")]
  [Item("SplitQualificationGroupManipulator", "Splits one qualification pool into two.")]
  public sealed class SplitQualificationGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private SplitQualificationGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private SplitQualificationGroupManipulator(SplitQualificationGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public SplitQualificationGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SplitQualificationGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      var groups = qualification.Groups.Where(x => x.Key.Count(q => q) > 1 && x.Value > 1).ToList();
      if (groups.Count == 0) return; // all groups consist of just 1 qualification and/or are of size 1 only

      var group = groups.SampleRandom(random);
      var pool = group.Key;
      var poolSize = group.Value;

      var setQualis = pool.GetTrueIndices().ToList();
      setQualis.ShuffleInPlace(random);
      var idx = random.Next(1, setQualis.Count);

      var qual1Pool = Qualification.Pool.Builder.CreateEmpty(pool.Count)
        .Set(setQualis.Take(idx)).Build();
      var qual2Pool = Qualification.Pool.Builder.CreateEmpty(pool.Count)
        .Set(setQualis.Skip(idx)).Build();

      var pool1Size = random.Next(1, poolSize);
      var pool2Size = poolSize - pool1Size;

      qualification.DecreaseOrRemove(pool, poolSize);
      qualification.AddOrIncrease(qual1Pool, pool1Size);
      qualification.AddOrIncrease(qual2Pool, pool2Size);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
