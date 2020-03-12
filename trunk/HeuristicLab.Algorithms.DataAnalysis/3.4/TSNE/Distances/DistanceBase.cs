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

using System.Collections;
using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableType("58D49402-2AB1-49CF-9AEF-1C599C4B71F7")]
  public abstract class DistanceBase<T> : Item, IDistance<T> {
    #region HLConstructors & Cloning
    [StorableConstructor]
    protected DistanceBase(StorableConstructorFlag _) : base(_) { }
    protected DistanceBase(DistanceBase<T> original, Cloner cloner) : base(original, cloner) { }
    protected DistanceBase() { }
    #endregion

    public abstract double Get(T a, T b);

    public IComparer<T> GetDistanceComparer(T item) {
      return new DistanceComparer(item, this);
    }

    public double Get(object x, object y) {
      return Get((T)x, (T)y);
    }

    public IComparer GetDistanceComparer(object item) {
      return new DistanceComparer((T)item, this);
    }

    [StorableType("35fcecb5-9209-469b-9e4e-82efd8abfddf")]
    internal class DistanceComparer : IComparer<T>, IComparer {
      [Storable]
      private readonly T item;
      [Storable]
      private readonly IDistance<T> dist;

      [StorableConstructor]
      protected DistanceComparer(StorableConstructorFlag _) {
      }

      public DistanceComparer(T item, IDistance<T> dist) {
        this.dist = dist;
        this.item = item;
      }

      public int Compare(T x, T y) {
        return dist.Get(x, item).CompareTo(dist.Get(y, item));
      }

      public int Compare(object x, object y) {
        return Compare((T)x, (T)y);
      }
    }
  }
}