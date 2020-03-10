﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using HeuristicLab.Core;
using HeuristicLab.Optimization;

namespace HeuristicLab.Encodings.QualificationEncoding {
  /// <summary>
  /// An interface which represents an operator for creating qualifications.
  /// </summary>
  [StorableType("A4157C7D-86EC-4945-9186-69C1FB147972")]
  public interface IQualificationCreator : IQualificationOperator, ISolutionCreator {
    ILookupParameter<Qualification> QualificationParameter { get; }
  }
}