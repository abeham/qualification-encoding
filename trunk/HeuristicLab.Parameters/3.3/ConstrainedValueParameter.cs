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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Parameters {
  /// <summary>
  /// A parameter whose value has to be chosen from a set of valid values.
  /// </summary>
  [Item("ConstrainedValueParameter", "A parameter whose value has to be chosen from a set of valid values.")]
  [StorableType("A9B43259-2687-4AE8-9803-B58CE01B81EE")]
  public class ConstrainedValueParameter<T> : OptionalConstrainedValueParameter<T> where T : class, IItem {
    public override T Value {
      get { return base.Value; }
      set {
        if ((value == null) && (ValidValues.Count > 0)) throw new ArgumentNullException();
        base.Value = value;
      }
    }

    [StorableConstructor]
    protected ConstrainedValueParameter(StorableConstructorFlag _) : base(_) { }
    protected ConstrainedValueParameter(ConstrainedValueParameter<T> original, Cloner cloner) : base(original, cloner) { }
    public ConstrainedValueParameter() : base() { }
    public ConstrainedValueParameter(string name) : base(name) { }
    public ConstrainedValueParameter(string name, ItemSet<T> validValues) : base(name, validValues) { }
    public ConstrainedValueParameter(string name, ItemSet<T> validValues, T value) : base(name, validValues, value) { }
    public ConstrainedValueParameter(string name, string description) : base(name, description) { }
    public ConstrainedValueParameter(string name, string description, ItemSet<T> validValues) : base(name, description, validValues) { }
    public ConstrainedValueParameter(string name, string description, ItemSet<T> validValues, T value) : base(name, description, validValues, value) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new ConstrainedValueParameter<T>(this, cloner);
    }

    protected override void ValidValues_ItemsAdded(object sender, CollectionItemsChangedEventArgs<T> e) {
      if (Value == null) Value = ValidValues.First();
    }
    protected override void ValidValues_ItemsRemoved(object sender, CollectionItemsChangedEventArgs<T> e) {
      if ((Value != null) && !ValidValues.Contains(Value)) Value = ValidValues.FirstOrDefault();
    }
    protected override void ValidValues_CollectionReset(object sender, CollectionItemsChangedEventArgs<T> e) {
      if ((Value != null) && !ValidValues.Contains(Value)) Value = ValidValues.FirstOrDefault();
    }
  }
}
