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
  /// Represents a support vector solution for a regression problem which can be visualized in the GUI.
  /// </summary>
  [Item("SupportVectorRegressionSolution", "Represents a support vector solution for a regression problem which can be visualized in the GUI.")]
  [StorableType("B23D2178-B9B5-4553-A633-DE359AB69BE9")]
  public sealed class SupportVectorRegressionSolution : RegressionSolution, ISupportVectorMachineSolution {

    public new ISupportVectorMachineModel Model {
      get { return (ISupportVectorMachineModel)base.Model; }
      set { base.Model = value; }
    }

    [StorableConstructor]
    private SupportVectorRegressionSolution(StorableConstructorFlag _) : base(_) { }
    private SupportVectorRegressionSolution(SupportVectorRegressionSolution original, Cloner cloner)
      : base(original, cloner) {
    }
    public SupportVectorRegressionSolution(SupportVectorMachineModel model, IRegressionProblemData problemData)
      : base(model, problemData) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SupportVectorRegressionSolution(this, cloner);
    }
  }
}
