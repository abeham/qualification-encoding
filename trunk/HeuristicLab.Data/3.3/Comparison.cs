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
using System.Drawing;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Data {
  [Item("Comparison", "Represents a comparison.")]
  [StorableType("2753AB02-748C-47C8-8D55-A1C43A57DF7D")]
  public class Comparison : ValueTypeValue<ComparisonType>, IComparable {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Enum; }
    }

    [StorableConstructor]
    protected Comparison(StorableConstructorFlag _) : base(_) { }
    protected Comparison(Comparison original, Cloner cloner)
      : base(original, cloner) {
    }
    public Comparison() : base() { }
    public Comparison(ComparisonType value) : base(value) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Comparison(this, cloner);
    }

    public virtual int CompareTo(object obj) {
      Comparison other = obj as Comparison;
      if (other != null)
        return Value.CompareTo(other.Value);
      else
        return Value.CompareTo(obj);
    }
  }
}
