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

using System.Collections.Generic;
using HEAL.Attic;

namespace HeuristicLab.Problems.DataAnalysis.Symbolic.Classification {
  [StorableType("64a5d560-0657-4022-87a0-9f3210062a84")]
  public interface ISymbolicClassificationModel : IClassificationModel, ISymbolicDataAnalysisModel {
    void RecalculateModelParameters(IClassificationProblemData problemData, IEnumerable<int> rows);
    new ISymbolicClassificationSolution CreateClassificationSolution(IClassificationProblemData problemData);

    void Scale(IClassificationProblemData problemData);
  }
}
