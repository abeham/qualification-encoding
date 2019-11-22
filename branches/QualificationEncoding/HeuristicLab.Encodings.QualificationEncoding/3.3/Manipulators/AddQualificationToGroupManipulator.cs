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
  [StorableType("5B5D9F2A-1F44-4CAF-BCBD-8E03DA80916A")]
  [Item("AddQualificationToGroupManipulator", "Adds one qualification to N workers of a group (and therefore might create a second group).")]
  public sealed class AddQualificationToGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private AddQualificationToGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private AddQualificationToGroupManipulator(AddQualificationToGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public AddQualificationToGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new AddQualificationToGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      // select one group randomly
      var group = qualification.Groups.SampleRandom(random);
      var pool = group.Key;
      var poolSize = group.Value;

      // collect all unset qualifications
      var unsetQualifications = pool.GetFalseIndices().ToList();
      // return since all qualifications are set
      if (unsetQualifications.Count == 0) return;

      // select one qualification randomly
      int qIndex = unsetQualifications[random.Next(unsetQualifications.Count)];

      // create new qualification group
      var newPool = pool.GetBuilder().Set(qIndex).Build();

      // choose how many workers to assign to the new qualification group
      int num = 1 + random.Next(poolSize);
      qualification.DecreaseOrRemove(pool, num);
      qualification.AddOrIncrease(newPool, num);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
