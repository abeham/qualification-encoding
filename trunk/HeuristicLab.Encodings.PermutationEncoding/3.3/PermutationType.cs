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

using System.Drawing;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HEAL.Attic;

namespace HeuristicLab.Encodings.PermutationEncoding {
  [Item("PermutationType", "Represents a certain type of permutation.")]
  [StorableType("B37C1815-5F82-4E46-BAE3-D71F801EFF46")]
  public class PermutationType : ValueTypeValue<PermutationTypes> {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Enum; }
    }

    [StorableConstructor]
    protected PermutationType(StorableConstructorFlag _) : base(_) { }
    protected PermutationType(PermutationType original, Cloner cloner) : base(original, cloner) { }
    public PermutationType() : base() { }
    public PermutationType(PermutationTypes type) : base(type) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new PermutationType(this, cloner);
    }
  }
}
