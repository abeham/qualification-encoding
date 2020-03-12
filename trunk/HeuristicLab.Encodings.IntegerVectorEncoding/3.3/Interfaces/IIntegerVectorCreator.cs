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
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HEAL.Attic;

namespace HeuristicLab.Encodings.IntegerVectorEncoding {
  [StorableType("9742b0e1-ab88-474e-b3f7-a2a0c2f77029")]
  /// <summary>
  /// An interface which represents an operator for creating vectors of int-valued data.
  /// </summary>
  public interface IIntegerVectorCreator : ISolutionCreator, IBoundedIntegerVectorOperator {
    IValueLookupParameter<IntValue> LengthParameter { get; }
    ILookupParameter<IntegerVector> IntegerVectorParameter { get; }
  }
}