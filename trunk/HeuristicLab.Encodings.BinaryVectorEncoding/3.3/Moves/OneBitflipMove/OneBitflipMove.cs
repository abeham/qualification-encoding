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
  [Item("OneBitflipMove", "Item that describes a one bitflip move.")]
  [StorableType("BFF3ABCF-9C7B-41F8-8861-B2164D92FD89")]
  public class OneBitflipMove : OneIndexMove {
    [StorableConstructor]
    protected OneBitflipMove(StorableConstructorFlag _) : base(_) { }
    protected OneBitflipMove(OneBitflipMove original, Cloner cloner) : base(original, cloner) { }
    public OneBitflipMove() : base() { }
    public OneBitflipMove(int index) : this(index, null) { }
    public OneBitflipMove(int index, BinaryVector binaryVector) : base(index, binaryVector) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new OneBitflipMove(this, cloner);
    }
  }
}
