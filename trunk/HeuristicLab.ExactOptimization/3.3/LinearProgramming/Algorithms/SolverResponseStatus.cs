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

  [StorableType("D6947706-E560-4D5C-8022-C6F1F20DCD8B")]
  public enum SolverResponseStatus {
    Optimal = 0,
    Feasible = 1,
    Infeasible = 2,
    Unbounded = 3,
    Abnormal = 4,
    ModelInvalid = 5,
    NotSolved = 6,
    SolverTypeUnavailable = 7,
    ModelInvalidSolutionHint = 84,
    ModelInvalidSolverParameters = 85,
    ModelIsValid = 97,
    UnknownStatus = 99
  }
}
