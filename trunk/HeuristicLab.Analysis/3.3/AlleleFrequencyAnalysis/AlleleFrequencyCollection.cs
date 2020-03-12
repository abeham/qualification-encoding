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
using System.Drawing;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Analysis {
  /// <summary>
  /// Represents a collection of allele frequencies.
  /// </summary>
  [Item("AlleleFrequencyCollection", "Represents a collection of allele frequencies.")]
  [StorableType("7BDABB5B-1FA0-4592-8C06-399DBEABB1CB")]
  public class AlleleFrequencyCollection : ReadOnlyItemCollection<AlleleFrequency> {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Statistics; }
    }

    [StorableConstructor]
    protected AlleleFrequencyCollection(StorableConstructorFlag _) : base(_) { }
    protected AlleleFrequencyCollection(AlleleFrequencyCollection original, Cloner cloner) : base(original, cloner) { }
    public AlleleFrequencyCollection() : base() { }
    public AlleleFrequencyCollection(IEnumerable<AlleleFrequency> frequencies) : base(new ItemCollection<AlleleFrequency>(frequencies)) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new AlleleFrequencyCollection(this, cloner);
    }
  }
}
