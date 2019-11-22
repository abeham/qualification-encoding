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
  [StorableType("E92EE999-BC63-4B9F-B185-DE89A03322B5")]
  [Item("DiscreteCrossover", "Chooses among multiple qualification pools.")]
  public sealed class DiscreteCrossover : QualificationCrossover {
    [StorableConstructor]
    private DiscreteCrossover(StorableConstructorFlag _) : base(_) { }
    private DiscreteCrossover(DiscreteCrossover original, Cloner cloner) : base(original, cloner) { }
    public DiscreteCrossover() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new DiscreteCrossover(this, cloner);
    }

    protected override Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      return Apply(encoding, parents, random);
    }

    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      if (parents.Length < 2)
        throw new ArgumentException($"{nameof(UnionAverageCrossover)}: The number of parents must be >= 2.", nameof(parents));

      var vectors = new HashSet<Qualification.Pool>(parents.SelectMany(x => x.Groups.Keys));
      var assignedWorkers = encoding.Workers;

      var offspring = new Qualification();
      foreach (var vector in vectors.Shuffle(random)) {
        var sizes = parents.Select(x => x.Groups.TryGetValue(vector, out int count) ? count : 0).Where(x => x > 0).ToArray();
        var size = sizes.SampleRandom(random);

        size = Math.Min(size, assignedWorkers);
        offspring.AddOrIncrease(vector, size);
        assignedWorkers -= size;

        if (assignedWorkers == 0) break;
      }

      QualificationHelpers.Repair(encoding, offspring, random);
      return offspring;
    }
  }
}
