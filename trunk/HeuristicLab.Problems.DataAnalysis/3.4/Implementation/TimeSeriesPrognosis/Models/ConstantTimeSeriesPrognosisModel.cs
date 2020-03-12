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
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Problems.DataAnalysis {
  [StorableType("A69BF8CC-2A57-4FBE-A468-8D38F3B6BBA6")]
  [Item("Constant TimeSeries Model", "A time series model that returns for all prediciton the same constant value.")]
  [Obsolete]
  public class ConstantTimeSeriesPrognosisModel : ConstantRegressionModel, ITimeSeriesPrognosisModel {
    [StorableConstructor]
    protected ConstantTimeSeriesPrognosisModel(StorableConstructorFlag _) : base(_) { }
    protected ConstantTimeSeriesPrognosisModel(ConstantTimeSeriesPrognosisModel original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new ConstantTimeSeriesPrognosisModel(this, cloner);
    }

    public ConstantTimeSeriesPrognosisModel(double constant, string targetVariable) : base(constant, targetVariable) { }

    public IEnumerable<IEnumerable<double>> GetPrognosedValues(IDataset dataset, IEnumerable<int> rows, IEnumerable<int> horizons) {
      return horizons.Select(horizon => Enumerable.Repeat(Constant, horizon));
    }

    public ITimeSeriesPrognosisSolution CreateTimeSeriesPrognosisSolution(ITimeSeriesPrognosisProblemData problemData) {
      return new TimeSeriesPrognosisSolution(this, new TimeSeriesPrognosisProblemData(problemData));
    }
  }
}
