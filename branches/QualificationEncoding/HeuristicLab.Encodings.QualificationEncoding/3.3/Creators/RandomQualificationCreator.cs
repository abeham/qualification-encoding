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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("2501D2B9-B2F4-419C-B3F2-11D6F0783263")]
  [Item("RandomQualificationCreator", "Creates random qualifications.")]
  public sealed class RandomQualificationCreator : QualificationCreator {
    [StorableConstructor]
    private RandomQualificationCreator(StorableConstructorFlag _) : base(_) { }
    private RandomQualificationCreator(RandomQualificationCreator original, Cloner cloner) : base(original, cloner) { }
    public RandomQualificationCreator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RandomQualificationCreator(this, cloner);
    }

    protected override Qualification Create(QualificationEncoding encoding, IRandom random) {
      var workers = encoding.Workers;
      var qualifications = encoding.Qualifications;

      int nrOfGroups = 1 + random.Next(workers);
      var vectorSet = new HashSet<Qualification.Pool>();

      for (int i = 0; i < nrOfGroups; i++) {
        var builder = Qualification.Pool.Builder.CreateEmpty(qualifications);
        var oneSet = false;
        for (var j = 0; j < builder.Count; j++) {
          if (random.NextDouble() < 1.0 / qualifications) {
            builder.Set(j);
            oneSet = true;
          }
        }
        if (!oneSet) builder.Set(random.Next(0, builder.Count)); // ensure that at least one qualification is set
        vectorSet.Add(builder.Build());
      }

      var vectors = vectorSet.ToArray();

      var sizes = Enumerable.Repeat(1, vectors.Length).ToArray();
      for (int i = 0; i < workers - vectors.Length; i++)
        sizes[random.Next(sizes.Length)]++;

      var solution = new Qualification(vectors, sizes);
      QualificationHelpers.Repair(encoding, solution, random);
      return solution;
    }
  }
}
