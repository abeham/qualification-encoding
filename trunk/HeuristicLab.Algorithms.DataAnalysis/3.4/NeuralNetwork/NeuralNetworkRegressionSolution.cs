#region License Information
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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;
using HeuristicLab.Problems.DataAnalysis;

namespace HeuristicLab.Algorithms.DataAnalysis {
  /// <summary>
  /// Represents a neural network solution for a regression problem which can be visualized in the GUI.
  /// </summary>
  [Item("NeuralNetworkRegressionSolution", "Represents a neural network solution for a regression problem which can be visualized in the GUI.")]
  [StorableType("E1E1B29A-B87A-4BCA-9A2C-83DA8AD7B9D9")]
  public sealed class NeuralNetworkRegressionSolution : RegressionSolution, INeuralNetworkRegressionSolution {

    public new INeuralNetworkModel Model {
      get { return (INeuralNetworkModel)base.Model; }
      set { base.Model = value; }
    }

    [StorableConstructor]
    private NeuralNetworkRegressionSolution(StorableConstructorFlag _) : base(_) { }
    private NeuralNetworkRegressionSolution(NeuralNetworkRegressionSolution original, Cloner cloner)
      : base(original, cloner) {
    }
    public NeuralNetworkRegressionSolution(INeuralNetworkModel nnModel, IRegressionProblemData problemData)
      : base(nnModel, problemData) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new NeuralNetworkRegressionSolution(this, cloner);
    }
  }
}
