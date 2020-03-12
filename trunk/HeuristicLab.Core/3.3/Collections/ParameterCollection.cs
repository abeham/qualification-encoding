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

using System.Collections.Generic;
using HeuristicLab.Common;
using HEAL.Attic;

namespace HeuristicLab.Core {
  [StorableType("D1F7CEF4-42C6-46E1-B2B9-AB49C1420D55")]
  [Item("ParameterCollection", "Represents a collection of parameters.")]
  public class ParameterCollection : NamedItemCollection<IParameter> {
    [StorableConstructor]
    protected ParameterCollection(StorableConstructorFlag _) : base(_) { }
    protected ParameterCollection(ParameterCollection original, Cloner cloner) : base(original, cloner) { }
    public ParameterCollection() : base() { }
    public ParameterCollection(int capacity) : base(capacity) { }
    public ParameterCollection(IEnumerable<IParameter> collection) : base(collection) { }

    public override IDeepCloneable Clone(Cloner cloner) { return new ParameterCollection(this, cloner); }
  }
}
