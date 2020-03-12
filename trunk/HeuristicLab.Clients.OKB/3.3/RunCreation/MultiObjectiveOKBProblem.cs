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
  [Item("OKB Problem (multi-objective)", "Represents a multi-objective problem which is stored in the OKB.")]
  [Creatable(CreatableAttribute.Categories.TestingAndAnalysisOKB, Priority = 120)]
  [StorableType("BB74E220-F721-4129-9A50-374647B16B97")]
  public sealed class MultiObjectiveOKBProblem : OKBProblem, IMultiObjectiveHeuristicOptimizationProblem, IStorableContent {
    public string Filename { get; set; }

    public override Type ProblemType {
      get { return typeof(IMultiObjectiveHeuristicOptimizationProblem); }
    }
    public new IMultiObjectiveHeuristicOptimizationProblem Problem {
      get { return base.Problem as IMultiObjectiveHeuristicOptimizationProblem; }
    }
    public IParameter MaximizationParameter {
      get { return Problem.MaximizationParameter; }
    }
    public new IMultiObjectiveEvaluator Evaluator {
      get { return Problem.Evaluator; }
    }

    [StorableConstructor]
    private MultiObjectiveOKBProblem(StorableConstructorFlag _) : base(_) { }
    private MultiObjectiveOKBProblem(MultiObjectiveOKBProblem original, Cloner cloner) : base(original, cloner) { }
    public MultiObjectiveOKBProblem() : base(new EmptyMultiObjectiveProblem("No problem selected. Please choose a multi-objective problem instance from the OKB.")) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MultiObjectiveOKBProblem(this, cloner);
    }
  }
}
