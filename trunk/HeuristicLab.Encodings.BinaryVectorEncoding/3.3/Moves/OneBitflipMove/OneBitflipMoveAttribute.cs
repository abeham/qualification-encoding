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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Encodings.BinaryVectorEncoding {
  [Item("OneBitflipMoveAttribute", "Describes the attributes (move quality and index) of a single bitflip move.")]
  [StorableType("D5CD3F76-E016-417F-B916-7666683A42D7")]
  public class OneBitflipMoveAttribute : Item {
    [Storable]
    public double MoveQuality { get; protected set; }
    [Storable]
    public int Index { get; protected set; }

    [StorableConstructor]
    protected OneBitflipMoveAttribute(StorableConstructorFlag _) : base(_) { }
    protected OneBitflipMoveAttribute(OneBitflipMoveAttribute original, Cloner cloner)
      : base(original, cloner) {
      this.MoveQuality = original.MoveQuality;
      this.Index = original.Index;
    }
    public OneBitflipMoveAttribute() : this(-1, 0) { }
    public OneBitflipMoveAttribute(int index, double moveQuality)
      : base() {
      Index = index;
      MoveQuality = moveQuality;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new OneBitflipMoveAttribute(this, cloner);
    }
  }
}
