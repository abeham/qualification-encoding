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

using System.Collections.Generic;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("07929C14-40FC-44AD-A947-24DA59D6DCE2")]
  [Item("MergeQualificationGroupManipulator", "Merges two qualification pools into one.")]
  public sealed class MergeQualificationGroupManipulator : QualificationManipulator {
    [StorableConstructor]
    private MergeQualificationGroupManipulator(StorableConstructorFlag _) : base(_) { }
    private MergeQualificationGroupManipulator(MergeQualificationGroupManipulator original, Cloner cloner) : base(original, cloner) { }
    public MergeQualificationGroupManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MergeQualificationGroupManipulator(this, cloner);
    }

    protected override void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Apply(encoding, qualification, random);
    }

    public static void Apply(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      Merge(encoding, qualification, random);
      QualificationHelpers.Repair(encoding, qualification, random);
    }

    public static void Merge(QualificationEncoding encoding, Qualification qualification, IRandom random) {
      var groupCount = qualification.Groups.Count;
      if (groupCount == 1) return;

      // select two group randomly
      var idx1 = random.Next(groupCount);
      var idx2 = (idx1 + random.Next(1, groupCount)) % groupCount;

      var g1 = default(KeyValuePair<Qualification.Pool, int>);
      var g2 = g1;
      foreach (var g in qualification.Groups) {
        if (idx1 == 0) g1 = g;
        if (idx2 == 0) g2 = g;
        idx1--;
        idx2--;
        if (idx1 < 0 && idx2 < 0) break;
      }

      // merge the two groups
      qualification.Groups.Remove(g2.Key);
      qualification.Groups.Remove(g1.Key);

      var newPool = g1.Key.GetBuilder().UnionWith(g2.Key).Build();

      qualification.AddOrIncrease(newPool, g1.Value + g2.Value);
    }
  }
}
