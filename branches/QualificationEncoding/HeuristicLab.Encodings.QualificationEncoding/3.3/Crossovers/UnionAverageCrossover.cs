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
  [StorableType("2F134E49-BB17-45F9-BC3B-B4EF23E88406")]
  [Item("UnionAverageCrossover", "Averages between qualification pools.")]
  public sealed class UnionAverageCrossover : QualificationCrossover {
    [StorableConstructor]
    private UnionAverageCrossover(StorableConstructorFlag _) : base(_) { }
    private UnionAverageCrossover(UnionAverageCrossover original, Cloner cloner) : base(original, cloner) { }
    public UnionAverageCrossover() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new UnionAverageCrossover(this, cloner);
    }

    protected override Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      return Apply(encoding, parents, random);
    }

    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      if (parents.Length < 2)
        throw new ArgumentException($"{nameof(UnionAverageCrossover)}: The number of parents must be >= 2.", nameof(parents));

      var vectors = new HashSet<Qualification.Pool>(parents.SelectMany(x => x.Groups.Keys));
      bool ceil = random.NextDouble() < 0.5;
      double delta = 0.0;
      var offspring = new Qualification();

      foreach (var vector in vectors) {
        var average = parents.Average(x => x.Groups.TryGetValue(vector, out int count) ? count : 0);
        var rounded = (int)(ceil ? Math.Ceiling(average) : Math.Floor(average));

        delta += average - rounded;

        ceil = delta.IsAlmost(0.0) ? random.NextDouble() < 0.5 : delta > 0.0;

        if (rounded == 0) continue;

        offspring.AddOrIncrease(vector, rounded);
      }

      if (!delta.IsAlmost(0.0))
        throw new InvalidOperationException($"{nameof(UnionAverageCrossover)}: Delta could not be compensated.");

      QualificationHelpers.Repair(encoding, offspring, random);
      return offspring;
    }
  }
}
