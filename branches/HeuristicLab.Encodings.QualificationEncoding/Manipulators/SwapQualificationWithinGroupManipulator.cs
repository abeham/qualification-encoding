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

using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Random;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("B37975D8-FD27-4C27-BC38-D84599251130")]
  [Item("SwapQualificationWithinGroupManipulator", "Trades one qualification present with another that is not present.")]
  public sealed class SwapQualificationWithinGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private SwapQualificationWithinGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private SwapQualificationWithinGroupManipulator(SwapQualificationWithinGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public SwapQualificationWithinGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SwapQualificationWithinGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      // select one group randomly
      var group = qualification.Groups.SampleRandom(random);
      var pool = group.Key;
      int poolSize = group.Value;

      // randomly select one set and one unset qualification
      int countFalse = 0, countTrue = 0, idxFalse = -1, idxTrue = -1;
      for (int i = 0; i < pool.Count; i++) {
        if (pool[i]) {
          countTrue++;
          if (countTrue == 1 || random.NextDouble() < 1.0 / countTrue)
            idxTrue = i;
        } else {
          countFalse++;
          if (countFalse == 1 || random.NextDouble() < 1.0 / countFalse)
            idxFalse = i;
        }
      }

      // remove the old pool
      qualification.Groups.Remove(pool);

      var newPoolBuilder = pool.GetBuilder();
      // swap the set and the unset qualification
      if (idxFalse >= 0) newPoolBuilder.Flip(idxFalse);
      if (idxTrue >= 0) newPoolBuilder.Flip(idxTrue);

      qualification.AddOrIncrease(newPoolBuilder.Build(), poolSize);

      QualificationHelpers.Repair(encoding, qualification, random);
    }
  }
}
