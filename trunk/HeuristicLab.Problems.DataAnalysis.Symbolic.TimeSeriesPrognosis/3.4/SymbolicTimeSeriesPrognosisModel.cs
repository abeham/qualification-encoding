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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HEAL.Attic;
using HeuristicLab.Problems.DataAnalysis.Symbolic.Regression;

namespace HeuristicLab.Problems.DataAnalysis.Symbolic.TimeSeriesPrognosis {
  /// <summary>
  /// Represents a symbolic time-series prognosis model
  /// </summary>
  [StorableType("88B9EB98-F156-4A36-A290-48BDB25C6E3C")]
  [Item(Name = "Symbolic Time-Series Prognosis Model", Description = "Represents a symbolic time series prognosis model.")]
  public class SymbolicTimeSeriesPrognosisModel : SymbolicRegressionModel, ISymbolicTimeSeriesPrognosisModel {

    public new ISymbolicTimeSeriesPrognosisExpressionTreeInterpreter Interpreter {
      get { return (ISymbolicTimeSeriesPrognosisExpressionTreeInterpreter)base.Interpreter; }
    }

    [StorableConstructor]
    protected SymbolicTimeSeriesPrognosisModel(StorableConstructorFlag _) : base(_) { }
    protected SymbolicTimeSeriesPrognosisModel(SymbolicTimeSeriesPrognosisModel original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new SymbolicTimeSeriesPrognosisModel(this, cloner);
    }

    public SymbolicTimeSeriesPrognosisModel(string targetVariable, ISymbolicExpressionTree tree, ISymbolicTimeSeriesPrognosisExpressionTreeInterpreter interpreter, double lowerLimit = double.MinValue, double upperLimit = double.MaxValue) : base(targetVariable, tree, interpreter, lowerLimit, upperLimit) { }

    public IEnumerable<IEnumerable<double>> GetPrognosedValues(IDataset dataset, IEnumerable<int> rows, IEnumerable<int> horizons) {
      var estimatedValues = Interpreter.GetSymbolicExpressionTreeValues(SymbolicExpressionTree, dataset, rows, horizons);
      return estimatedValues.Select(predictionPerRow => predictionPerRow.LimitToRange(LowerEstimationLimit, UpperEstimationLimit));
    }

    public ISymbolicTimeSeriesPrognosisSolution CreateTimeSeriesPrognosisSolution(ITimeSeriesPrognosisProblemData problemData) {
      return new SymbolicTimeSeriesPrognosisSolution(this, new TimeSeriesPrognosisProblemData(problemData));
    }
    ITimeSeriesPrognosisSolution ITimeSeriesPrognosisModel.CreateTimeSeriesPrognosisSolution(ITimeSeriesPrognosisProblemData problemData) {
      return CreateTimeSeriesPrognosisSolution(problemData);
    }
  }
}
