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

using HEAL.Attic;

namespace HeuristicLab.Problems.WorkerCrosstraining {
  [StorableType("6806E4F0-E534-4EC5-A48B-3DD531749832")]
  public enum DispatchStrategy { FirstComeFirstServe, LeastSkillFirst, ModifiedLeastSkillFirst }

  [StorableType("54DAEF0B-552B-4312-AE58-7DE31C331818")]
  public enum Objective { WIPLeadtime, Tardiness, Totalcost, Servicelevel }
}