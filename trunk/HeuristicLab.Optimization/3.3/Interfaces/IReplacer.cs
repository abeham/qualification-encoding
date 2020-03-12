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

using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Optimization {
  [StorableType("2aaf9cd7-c5dc-48d0-b59d-5801e152b19e")]
  /// <summary>
  /// Interface to mark operators that can be used as replacers.
  /// Replacers are merging a remaining and a selected branch in several special ways.
  /// </summary>
  public interface IReplacer : IOperator {
    IValueLookupParameter<ISelector> ReplacedSelectorParameter { get; }
    IValueLookupParameter<ISelector> SelectedSelectorParameter { get; }
  }
}
