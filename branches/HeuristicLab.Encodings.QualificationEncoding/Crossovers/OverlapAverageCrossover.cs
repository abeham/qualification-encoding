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

using System;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("19E47CF1-336F-4837-AAF1-5A9F824A8FB6")]
  [Item("OverlapAverageCrossover", "Averages between qualification pools using only those common to all parents. Performs a discrete crossover in case no common qualification pools are found.")]
  public sealed class OverlapAverageCrossover : QualificationCrossover {
    [StorableConstructor]
    private OverlapAverageCrossover(StorableConstructorFlag _) : base(_) { }
    private OverlapAverageCrossover(OverlapAverageCrossover original, Cloner cloner) : base(original, cloner) { }
    public OverlapAverageCrossover() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new OverlapAverageCrossover(this, cloner);
    }

    protected override Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      return Apply(encoding, parents, random);
    }

    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      if (parents.Length < 2)
        throw new ArgumentException($"{nameof(OverlapAverageCrossover)}: The number of parents must be >= 2.", nameof(parents));

      var commonGroups = new HashSet<Qualification.Pool>(parents[0].Groups.Keys);
      foreach (var p in parents.Skip(1)) {
        commonGroups.IntersectWith(p.Groups.Keys);
        if (commonGroups.Count == 0) break;
      }
      if (commonGroups.Count == 0) {
        // there are no common qualification groups among the parents
        // apply a discrete crossover as fallback
        return DiscreteCrossover.Apply(encoding, parents, random);
      }

      var offspring = new Qualification();
      foreach (var g in commonGroups) {
        var average = parents.Average(x => x.Groups[g]);
        var rounded = (int)Math.Floor(average);
        if (rounded == 0) continue;

        offspring.AddOrIncrease(g, rounded);
      }
      QualificationHelpers.Repair(encoding, offspring, random);
      return offspring;
    }
  }
}
