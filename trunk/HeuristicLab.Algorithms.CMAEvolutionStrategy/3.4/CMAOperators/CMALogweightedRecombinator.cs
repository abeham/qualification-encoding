﻿#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Algorithms.CMAEvolutionStrategy {
  [Item("CMA Log-weighted Recombinator", "Calculates weighted mean based on a logarithmic decreasing weights.")]
  [StorableType("83A47E81-FF87-4A28-AEB4-DE453B55B680")]
  public class CMALogweightedRecombinator : CMARecombinator {

    [StorableConstructor]
    protected CMALogweightedRecombinator(StorableConstructorFlag _) : base(_) { }
    protected CMALogweightedRecombinator(CMALogweightedRecombinator original, Cloner cloner) : base(original, cloner) { }
    public CMALogweightedRecombinator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CMALogweightedRecombinator(this, cloner);
    }

    protected override double[] GetWeights(int mu) {
      var weights = new double[mu];
      for (int i = 0; i < mu; i++) weights[i] = Math.Log((mu + 1.0) / (i + 1.0));
      var sum = weights.Sum();
      for (int i = 0; i < mu; i++) weights[i] /= sum;
      return weights;
    }
  }
}