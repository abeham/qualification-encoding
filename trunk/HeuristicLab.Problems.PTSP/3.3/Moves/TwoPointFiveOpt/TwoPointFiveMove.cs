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
using HeuristicLab.Encodings.PermutationEncoding;
using HEAL.Attic;

namespace HeuristicLab.Problems.PTSP {
  [Item("2.5-Move", "Represents a 2.5-move.")]
  [StorableType("CAC6762D-DFF1-4B66-BAC7-FBCED4A52305")]
  public sealed class TwoPointFiveMove : Item {
    [Storable]
    public int Index1 { get; private set; }
    [Storable]
    public int Index2 { get; private set; }
    [Storable]
    public Permutation Permutation { get; private set; }
    [Storable]
    public bool IsInvert { get; private set; }

    [StorableConstructor]
    private TwoPointFiveMove(StorableConstructorFlag _) : base(_) { }
    private TwoPointFiveMove(TwoPointFiveMove original, Cloner cloner)
      : base(original, cloner) {
      this.Index1 = original.Index1;
      this.Index2 = original.Index2;
      this.IsInvert = original.IsInvert;
      this.Permutation = cloner.Clone(original.Permutation);
    }
    public TwoPointFiveMove(int index1, int index2, Permutation permutation, bool isinvert)
      : base() {
      Index1 = index1;
      Index2 = index2;
      IsInvert = isinvert;
      Permutation = permutation;
    }

    public TwoPointFiveMove(int index1, int index2, bool isinvert)
      : base() {
      Index1 = index1;
      Index2 = index2;
      IsInvert = isinvert;
      Permutation = null;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new TwoPointFiveMove(this, cloner);
    }
  }
}
