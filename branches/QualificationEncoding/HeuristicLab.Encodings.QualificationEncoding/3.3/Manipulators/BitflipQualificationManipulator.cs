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
  [StorableType("9DE7D9EB-D2D0-4B6E-BEEE-EFAB931C499E")]
  [Item("BitflipQualificationManipulator", "Flips one qualification of one group.")]
  public sealed class BitflipQualificationManipulator : QualificationManipulator {
    [StorableConstructor]
    private BitflipQualificationManipulator(StorableConstructorFlag _) : base(_) { }
    private BitflipQualificationManipulator(BitflipQualificationManipulator original, Cloner cloner) : base(original, cloner) { }
    public BitflipQualificationManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new BitflipQualificationManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      // select one group randomly
      var group = qualification.Groups.SampleRandom(random);

      // select one qualification randomly
      int qIndex = random.Next(group.Key.Count);

      // create new qualification group
      var newPool = group.Key.GetBuilder().Flip(qIndex).Build();

      // return since last qualification would be flipped
      if (newPool.All(x => !x)) return;

      qualification.DecreaseOrRemove(group.Key, group.Value);
      qualification.AddOrIncrease(newPool, group.Value);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
