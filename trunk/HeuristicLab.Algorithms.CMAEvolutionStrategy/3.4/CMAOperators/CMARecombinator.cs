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
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Operators;
using HeuristicLab.Parameters;
using HEAL.Attic;
using System;

namespace HeuristicLab.Algorithms.CMAEvolutionStrategy {
  [Item("CMARecombinator", "Base class that calculates the weighted mean of a number of offspring.")]
  [StorableType("C0798B4D-1685-4720-828A-17E40879000B")]
  public abstract class CMARecombinator : SingleSuccessorOperator, ICMARecombinator {

    public Type CMAType {
      get { return typeof(CMAParameters); }
    }

    #region Parameter Properties
    public IScopeTreeLookupParameter<RealVector> OffspringParameter {
      get { return (IScopeTreeLookupParameter<RealVector>)Parameters["Offspring"]; }
    }

    public ILookupParameter<RealVector> MeanParameter {
      get { return (ILookupParameter<RealVector>)Parameters["Mean"]; }
    }

    public ILookupParameter<RealVector> OldMeanParameter {
      get { return (ILookupParameter<RealVector>)Parameters["OldMean"]; }
    }

    public ILookupParameter<CMAParameters> StrategyParametersParameter {
      get { return (ILookupParameter<CMAParameters>)Parameters["StrategyParameters"]; }
    }
    #endregion

    [StorableConstructor]
    protected CMARecombinator(StorableConstructorFlag _) : base(_) { }
    protected CMARecombinator(CMARecombinator original, Cloner cloner) : base(original, cloner) { }
    protected CMARecombinator()
      : base() {
      Parameters.Add(new ScopeTreeLookupParameter<RealVector>("Offspring", "The offspring that should be recombined."));
      Parameters.Add(new LookupParameter<RealVector>("Mean", "The new mean solution."));
      Parameters.Add(new LookupParameter<RealVector>("OldMean", "The old mean solution."));
      Parameters.Add(new LookupParameter<CMAParameters>("StrategyParameters", "The CMA-ES strategy parameters used for mutation."));
      OffspringParameter.ActualName = "RealVector";
      MeanParameter.ActualName = "XMean";
      OldMeanParameter.ActualName = "XOld";
    }

    public override IOperation Apply() {
      var sp = StrategyParametersParameter.ActualValue;
      if (sp.Weights == null) sp.Weights = GetWeights(sp.Mu);

      var offspring = OffspringParameter.ActualValue;
      var mean = new RealVector(offspring[0].Length);
      for (int i = 0; i < mean.Length; i++) {
        for (int j = 0; j < sp.Mu; j++)
          mean[i] += sp.Weights[j] * offspring[j][i];
      }

      var oldMean = MeanParameter.ActualValue;
      MeanParameter.ActualValue = mean;
      OldMeanParameter.ActualValue = oldMean;
      return base.Apply();
    }

    protected abstract double[] GetWeights(int mu);
  }
}