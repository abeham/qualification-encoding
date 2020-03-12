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
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Optimization {
  [StorableType("af5d3f60-6f3a-4a44-a906-688ac8296fe3")]
  public interface IResultParameter : ILookupParameter {
    string ResultCollectionName { get; set; }
    event EventHandler ResultCollectionNameChanged;
  }

  [StorableType("803e6ad6-dd9d-497a-ad1c-7cd3dc5b0d3c")]
  public interface IResultParameter<T> : ILookupParameter<T>, IResultParameter where T : class, IItem {
    T DefaultValue { get; set; }
    event EventHandler DefaultValueChanged;
  }
}
