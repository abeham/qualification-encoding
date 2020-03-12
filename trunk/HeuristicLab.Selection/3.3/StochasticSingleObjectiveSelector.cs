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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HEAL.Attic;

namespace HeuristicLab.Selection {
  /// <summary>
  /// A base class for stochastic selection operators which consider a single double quality value for selection.
  /// </summary>
  [Item("StochasticSingleObjectiveSelector", "A base class for stochastic selection operators which consider a single double quality value for selection.")]
  [StorableType("566030A6-F1F8-4F45-A768-4D2BD236D39E")]
  public abstract class StochasticSingleObjectiveSelector : SingleObjectiveSelector, IStochasticOperator {
    public ILookupParameter<IRandom> RandomParameter {
      get { return (LookupParameter<IRandom>)Parameters["Random"]; }
    }
    [StorableConstructor]
    protected StochasticSingleObjectiveSelector(StorableConstructorFlag _) : base(_) { }
    protected StochasticSingleObjectiveSelector(StochasticSingleObjectiveSelector original, Cloner cloner) : base(original, cloner) { }
    protected StochasticSingleObjectiveSelector()
      : base() {
      Parameters.Add(new LookupParameter<IRandom>("Random", "The pseudo random number generator used for stochastic selection."));
    }
  }
}
