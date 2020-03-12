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
using HEAL.Attic;

namespace HeuristicLab.Problems.TestFunctions {
  [Item("RosenbrockAdditiveMoveEvaluator", "Class for evaluating an additive move on the Rosenbrock function.")]
  [StorableType("A4CE5DFB-BC5C-4024-AD0F-7E23D6A50FBD")]
  public class RosenbrockAdditiveMoveEvaluator : AdditiveMoveEvaluator {
    public override System.Type EvaluatorType {
      get { return typeof(RosenbrockEvaluator); }
    }

    [StorableConstructor]
    protected RosenbrockAdditiveMoveEvaluator(StorableConstructorFlag _) : base(_) { }
    protected RosenbrockAdditiveMoveEvaluator(RosenbrockAdditiveMoveEvaluator original, Cloner cloner) : base(original, cloner) { }
    public RosenbrockAdditiveMoveEvaluator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RosenbrockAdditiveMoveEvaluator(this, cloner);
    }

    protected override double Evaluate(double quality, RealVector point, AdditiveMove move) {
      RealVectorAdditiveMoveWrapper wrapper = new RealVectorAdditiveMoveWrapper(move, point);
      return RosenbrockEvaluator.Apply(wrapper);
    }
  }
}
