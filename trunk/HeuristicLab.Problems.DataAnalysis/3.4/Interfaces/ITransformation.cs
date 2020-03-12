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
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Problems.DataAnalysis {
  [StorableType("6d791015-1439-40e6-a0ba-a8c49c0ddba9")]
  public interface ITransformation : IParameterizedItem {
    string ShortName { get; }
    string Column { get; }
  }

  [StorableType("a200b865-5010-46ae-9070-094ac272f3a8")]
  public interface ITransformation<T> : ITransformation {
    void ConfigureParameters(IEnumerable<T> data);
    IEnumerable<T> ConfigureAndApply(IEnumerable<T> data);
    IEnumerable<T> Apply(IEnumerable<T> data);
  }
}
