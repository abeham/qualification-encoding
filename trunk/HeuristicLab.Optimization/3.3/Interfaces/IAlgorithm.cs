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

using System;
using System.Collections.Generic;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Optimization {
  [StorableType("c293d208-59ac-4d3c-b815-47d68355628d")]
  /// <summary>
  /// Interface to represent an algorithm.
  /// </summary>
  public interface IAlgorithm : IParameterizedNamedItem, IOptimizer {
    Type ProblemType { get; }
    IProblem Problem { get; set; }
    ResultCollection Results { get; }
    bool StoreAlgorithmInEachRun { get; set; }

    void CollectResultValues(IDictionary<string, IItem> values);

    event EventHandler ProblemChanged;
    event EventHandler StoreAlgorithmInEachRunChanged;
  }
}
