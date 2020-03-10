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
using HeuristicLab.Random;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("319AD274-9A39-4EFB-B9BD-380B51BDB6CC")]
  [Item("SetPackCrossover", "Aims to find a maximum set of pools that are disjoint in the qualifications. Performs a discrete crossover in case the solution is equal to one parent.")]
  public sealed class SetPackCrossover : QualificationCrossover {
    [StorableConstructor]
    private SetPackCrossover(StorableConstructorFlag _) : base(_) { }
    private SetPackCrossover(SetPackCrossover original, Cloner cloner) : base(original, cloner) { }
    public SetPackCrossover() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SetPackCrossover(this, cloner);
    }

    protected override Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      return Apply(encoding, parents, random);
    }

    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      var inputSets = parents.SelectMany(x => x.Groups).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => (int)Math.Round(x.Average(y => y.Value)));
      var result = new Dictionary<Qualification.Pool, int>();

      var numQualis = parents[0].NumberOfQualifications;
      var match = Qualification.Pool.Builder.CreateEmpty(numQualis).Build();
      var matchCount = 0;
      // greedy-heuristic for set-packing problems
      while (matchCount < numQualis && inputSets.Count > 0) {
        var bestSet = default(KeyValuePair<Qualification.Pool, int>);
        var bestDisjoint = -1;
        var bestQualis = -1;
        var bestCount = 1;
        foreach (var set in inputSets) {
          var disjoint = 0;
          foreach (var other in inputSets) {
            if (ReferenceEquals(set, other)) continue;
            if (HammingSimilarityCalculator.AreDisjoint(set.Key, other.Key))
              disjoint++;
          }
          var qualis = set.Key.CountTrue;
          if (disjoint > bestDisjoint || disjoint == bestDisjoint && qualis < bestQualis) {
            bestSet = set;
            bestDisjoint = disjoint;
            bestQualis = qualis;
            bestCount = 1;
          } else if (disjoint == bestDisjoint && qualis == bestQualis && random.NextDouble() * (++bestCount) < 1) { // random tie-breaking
            bestSet = set;
          }
        }
        inputSets.Remove(bestSet.Key);
        result[bestSet.Key] = bestSet.Value;
        match = Qualification.Pool.Builder.CreateFrom(match).UnionWith(bestSet.Key).Build();
        matchCount = match.CountTrue;
        foreach (var set in inputSets.Where(x =>
            !HammingSimilarityCalculator.AreDisjoint(bestSet.Key, x.Key)
            || HammingSimilarityCalculator.NumberOfAdditionalQualifications(match, x.Key) == 0).ToList())
          inputSets.Remove(set.Key);
      }

      var offspring = new Qualification(result);
      QualificationHelpers.Repair(encoding, offspring, random);

      if (parents.Any(p => HammingSimilarityCalculator.Equals(offspring, p))) {
        // apply a discrete crossover as fallback
        return DiscreteCrossover.Apply(encoding, parents.Length == 2 ? parents : new ItemArray<Qualification>(parents.SampleRandom(random, 2)), random);
      }

      return offspring;
    }
  }
}
