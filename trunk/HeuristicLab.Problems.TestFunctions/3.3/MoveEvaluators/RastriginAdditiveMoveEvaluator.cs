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
using HeuristicLab.Data;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Parameters;
using HEAL.Attic;

namespace HeuristicLab.Problems.TestFunctions {
  [Item("RastriginAdditiveMoveEvaluator", "Class for evaluating an additive move on the Rastrigin function.")]
  [StorableType("F4D22E9B-6D46-4F7A-AABA-A77048F879DB")]
  public class RastriginAdditiveMoveEvaluator : AdditiveMoveEvaluator, IRastriginMoveEvaluator {
    public override System.Type EvaluatorType {
      get { return typeof(RastriginEvaluator); }
    }
    /// <summary>
    /// The parameter A is a parameter of the objective function y = Sum((x_i)^2 + A * (1 - Cos(2pi*x_i))). Default is A = 10.
    /// </summary>
    public ValueParameter<DoubleValue> AParameter {
      get { return (ValueParameter<DoubleValue>)Parameters["A"]; }
    }
    /// <summary>
    /// The parameter A is a parameter of the objective function y = Sum((x_i)^2 + A * (1 - Cos(2pi*x_i))). Default is A = 10.
    /// </summary>
    public DoubleValue A {
      get { return AParameter.Value; }
      set { if (value != null) AParameter.Value = value; }
    }

    [StorableConstructor]
    protected RastriginAdditiveMoveEvaluator(StorableConstructorFlag _) : base(_) { }
    protected RastriginAdditiveMoveEvaluator(RastriginAdditiveMoveEvaluator original, Cloner cloner) : base(original, cloner) { }
    public RastriginAdditiveMoveEvaluator() {
      Parameters.Add(new ValueParameter<DoubleValue>("A", "The parameter A is a parameter of the objective function y = Sum((x_i)^2 + A * (1 - Cos(2pi*x_i))). Default is A = 10.", new DoubleValue(10)));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RastriginAdditiveMoveEvaluator(this, cloner);
    }

    protected override double Evaluate(double quality, RealVector point, AdditiveMove move) {
      RealVectorAdditiveMoveWrapper wrapper = new RealVectorAdditiveMoveWrapper(move, point);
      return RastriginEvaluator.Apply(wrapper, A.Value);
    }
  }
}
