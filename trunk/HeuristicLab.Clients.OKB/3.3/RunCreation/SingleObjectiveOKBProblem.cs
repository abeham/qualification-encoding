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
using HeuristicLab.Optimization;
using HEAL.Attic;

namespace HeuristicLab.Clients.OKB.RunCreation {
  [Item("OKB Problem (single-objective)", "Represents a single-objective problem which is stored in the OKB.")]
  [Creatable(CreatableAttribute.Categories.TestingAndAnalysisOKB, Priority = 110)]
  [StorableType("1A1DF6E8-4A3F-4D91-9B1D-6FF6EC8D1055")]
  public sealed class SingleObjectiveOKBProblem : OKBProblem, ISingleObjectiveHeuristicOptimizationProblem, IStorableContent {
    public string Filename { get; set; }

    public override Type ProblemType {
      get { return typeof(ISingleObjectiveHeuristicOptimizationProblem); }
    }
    public new ISingleObjectiveHeuristicOptimizationProblem Problem {
      get { return base.Problem as ISingleObjectiveHeuristicOptimizationProblem; }
    }
    public IParameter BestKnownQualityParameter {
      get { return Problem.BestKnownQualityParameter; }
    }
    public IParameter MaximizationParameter {
      get { return Problem.MaximizationParameter; }
    }
    public new ISingleObjectiveEvaluator Evaluator {
      get { return Problem.Evaluator; }
    }

    [StorableConstructor]
    private SingleObjectiveOKBProblem(StorableConstructorFlag _) : base(_) { }
    private SingleObjectiveOKBProblem(SingleObjectiveOKBProblem original, Cloner cloner) : base(original, cloner) { }
    public SingleObjectiveOKBProblem() : base(new EmptySingleObjectiveProblem("No problem selected. Please choose a single-objective problem instance from the OKB.")) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SingleObjectiveOKBProblem(this, cloner);
    }
  }
}
