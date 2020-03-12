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

using System;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Parameters {
  /// <summary>
  /// A parameter whose value is defined in the parameter itself.
  /// </summary>
  [Item("FixedValueParameter", "A parameter whose value is defined in the parameter itself and cannot be set.")]
  [StorableType("7787B99D-5F32-4639-B47A-76CB4D204392")]
  public class FixedValueParameter<T> : ValueParameter<T>, IFixedValueParameter<T> where T : class, IItem, new() {

    public override T Value {
      get { return base.Value; }
      set { throw new NotSupportedException("FixedValueParameters do not support setting their value."); }
    }

    IItem IFixedValueParameter.Value { get { return Value; } }

    [StorableConstructor]
    protected FixedValueParameter(StorableConstructorFlag _) : base(_) { }
    protected FixedValueParameter(FixedValueParameter<T> original, Cloner cloner) : base(original, cloner) { }

    public FixedValueParameter() : base() { }
    public FixedValueParameter(string name) : base(name) { }
    public FixedValueParameter(string name, T value) : base(name, value) { }
    public FixedValueParameter(string name, string description) : base(name, description) { }
    public FixedValueParameter(string name, string description, T value) : base(name, description, value) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new FixedValueParameter<T>(this, cloner);
    }
  }
}
