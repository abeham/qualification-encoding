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

using HEAL.Attic;

namespace HeuristicLab.ExactOptimization.LinearProgramming {

  [StorableType("73192CE9-6D6B-4631-95BE-C15B8B5F9FE6")]
  public enum ResultStatus {
    Optimal,        // optimal.
    Feasible,       // feasible, or stopped by limit.
    Infeasible,     // proven infeasible.
    Unbounded,      // proven unbounded.
    Abnormal,       // abnormal, i.e., error of some kind.
    ModelInvalid,   // the model is trivially invalid (NaN coefficients, etc).
    NotSolved = 6,  // not been solved yet.
    OptimalWithinTolerance = int.MaxValue  // optimal within gap tolerance but objective != bound
  }
}
