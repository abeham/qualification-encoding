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

using Google.OrTools.LinearSolver;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.ExactOptimization.LinearProgramming {

  [Item("Clp/Cbc", "Clp (https://projects.coin-or.org/Clp) and Cbc (https://projects.coin-or.org/Cbc) can be used out of the box.")]
  [StorableType("53148A15-C754-4CCA-AFAA-200916CDB7F5")]
  public class CoinOrSolver : IncrementalLinearSolver {

    public CoinOrSolver() {
      Parameters.Remove(solverSpecificParametersParam);
    }

    protected CoinOrSolver(CoinOrSolver original, Cloner cloner)
      : base(original, cloner) {
    }

    [StorableConstructor]
    protected CoinOrSolver(StorableConstructorFlag _) : base(_) { }

    public override bool SupportsPause => false;

    public override bool SupportsQualityUpdate => ProblemType == ProblemType.LinearProgramming;

    public override bool SupportsStop => false;

    protected override Solver.OptimizationProblemType OptimizationProblemType =>
      ProblemType == ProblemType.LinearProgramming
        ? Solver.OptimizationProblemType.ClpLinearProgramming
        : Solver.OptimizationProblemType.CbcMixedIntegerProgramming;

    public override IDeepCloneable Clone(Cloner cloner) => new CoinOrSolver(this, cloner);
  }
}
