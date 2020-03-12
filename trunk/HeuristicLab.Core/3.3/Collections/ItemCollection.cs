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
using System.Drawing;
using System.Linq;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HEAL.Attic;

namespace HeuristicLab.Core {
  [StorableType("0BD4F01E-2D52-4E41-AEF8-611F10856D90")]
  [Item("ItemCollection", "Represents a collection of items.")]
  public class ItemCollection<T> : ObservableCollection<T>, IItemCollection<T> where T : class, IItem {
    public virtual string ItemName {
      get { return ItemAttribute.GetName(this.GetType()); }
    }
    public virtual string ItemDescription {
      get { return ItemAttribute.GetDescription(this.GetType()); }
    }
    public Version ItemVersion {
      get { return ItemAttribute.GetVersion(this.GetType()); }
    }
    public static Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Class; }
    }
    public virtual Image ItemImage {
      get { return ItemAttribute.GetImage(this.GetType()); }
    }

    [StorableConstructor]
    protected ItemCollection(StorableConstructorFlag _) : base(_) { }
    protected ItemCollection(ItemCollection<T> original, Cloner cloner) {
      cloner.RegisterClonedObject(original, this);
      list = new List<T>(original.Select(x => cloner.Clone(x)));
    }
    public ItemCollection() : base() { }
    public ItemCollection(int capacity) : base(capacity) { }
    public ItemCollection(IEnumerable<T> collection) : base(collection) { }

    public object Clone() {
      return Clone(new Cloner());
    }
    public virtual IDeepCloneable Clone(Cloner cloner) {
      return new ItemCollection<T>(this, cloner);
    }

    public new ReadOnlyItemCollection<T> AsReadOnly() {
      return new ReadOnlyItemCollection<T>(this);
    }

    public override string ToString() {
      return ItemName;
    }

    public event EventHandler ItemImageChanged;
    protected virtual void OnItemImageChanged() {
      EventHandler handler = ItemImageChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler ToStringChanged;
    protected virtual void OnToStringChanged() {
      EventHandler handler = ToStringChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}
