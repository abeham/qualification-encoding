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

using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;
using HeuristicLab.Problems.DataAnalysis;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [Item("PCA", "Initializes the matrix by performing a principal components analysis.")]
  [StorableType("C08B748E-61BC-4297-9ED0-383A41A699B4")]
  public sealed class PcaInitializer : NcaInitializer {

    [StorableConstructor]
    private PcaInitializer(StorableConstructorFlag _) : base(_) { }
    private PcaInitializer(PcaInitializer original, Cloner cloner) : base(original, cloner) { }
    public PcaInitializer() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new PcaInitializer(this, cloner);
    }

    public override double[,] Initialize(IClassificationProblemData data, int dimensions) {
      var instances = data.TrainingIndices.Count();
      var attributes = data.AllowedInputVariables.Count();

      var pcaDs = data.Dataset.ToArray(data.AllowedInputVariables, data.TrainingIndices);

      int info;
      double[] varianceValues;
      double[,] matrix;
      alglib.pcabuildbasis(pcaDs, instances, attributes, out info, out varianceValues, out matrix);

      return matrix;
    }

  }
}
