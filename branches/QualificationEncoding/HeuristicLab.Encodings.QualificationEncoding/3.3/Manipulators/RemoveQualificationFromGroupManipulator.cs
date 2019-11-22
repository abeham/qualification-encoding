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
  [StorableType("413F87EE-651B-4E16-9579-D4191AADB8C5")]
  [Item("RemoveQualificationFromGroupManipulator", "Removes one qualification from N workers of a group (and therefore might create a second group).")]
  public sealed class RemoveQualificationFromGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private RemoveQualificationFromGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private RemoveQualificationFromGroupManipulator(RemoveQualificationFromGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public RemoveQualificationFromGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RemoveQualificationFromGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      // select one group randomly
      var group = qualification.Groups.SampleRandom(random);
      var pool = group.Key;
      var poolSize = group.Value;

      // collect all set qualifications
      var setQualifications = pool.GetTrueIndices().ToList();

      // return since less than two qualifications are set
      if (setQualifications.Count < 2) return;

      // select one qualification randomly
      int qIndex = setQualifications[random.Next(setQualifications.Count)];

      // create new qualification group
      var newPool = pool.GetBuilder().Flip(qIndex).Build();

      // choose how many workers to assign to the new qualification group
      int num = 1 + random.Next(poolSize);
      qualification.DecreaseOrRemove(pool, num);
      qualification.AddOrIncrease(newPool, num);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
