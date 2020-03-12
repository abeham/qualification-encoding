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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HEAL.Attic;

namespace HeuristicLab.Encodings.IntegerVectorEncoding {
  [Item("BoundedIntegerVectorCrossover", "Base class for bounded crossovers.")]
  [StorableType("90834C8F-461F-4A94-A7CB-25C8D1063604")]
  public abstract class BoundedIntegerVectorCrossover : IntegerVectorCrossover, IBoundedIntegerVectorOperator {

    public IValueLookupParameter<IntMatrix> BoundsParameter {
      get { return (IValueLookupParameter<IntMatrix>)Parameters["Bounds"]; }
    }

    [StorableConstructor]
    protected BoundedIntegerVectorCrossover(StorableConstructorFlag _) : base(_) { }
    protected BoundedIntegerVectorCrossover(BoundedIntegerVectorCrossover original, Cloner cloner) : base(original, cloner) { }
    public BoundedIntegerVectorCrossover()
      : base() {
      Parameters.Add(new ValueLookupParameter<IntMatrix>("Bounds", "The bounds matrix can contain one row for each dimension with three columns specifying minimum (inclusive), maximum (exclusive), and step size. If less rows are given the matrix is cycled."));
    }

    protected sealed override IntegerVector Cross(IRandom random, ItemArray<IntegerVector> parents) {
      if (BoundsParameter.ActualValue == null) throw new InvalidOperationException(Name + ": Bounds are not defined!");
      return CrossBounded(random, parents, BoundsParameter.ActualValue);
    }

    protected abstract IntegerVector CrossBounded(IRandom random, ItemArray<IntegerVector> parents, IntMatrix bounds);
  }
}
