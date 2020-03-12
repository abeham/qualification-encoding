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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Parameters;
using HEAL.Attic;

namespace HeuristicLab.Selection {
  /// <summary>
  /// A base class for reduction operators.
  /// </summary>
  [Item("Reducer", "A base class for reduction operators.")]
  [StorableType("ADEA7125-A5E1-4A26-AE05-9DE900B9BE5C")]
  public abstract class Reducer : SingleSuccessorOperator {
    protected ScopeParameter CurrentScopeParameter {
      get { return (ScopeParameter)Parameters["CurrentScope"]; }
    }

    public IScope CurrentScope {
      get { return CurrentScopeParameter.ActualValue; }
    }

    [StorableConstructor]
    protected Reducer(StorableConstructorFlag _) : base(_) { }
    protected Reducer(Reducer original, Cloner cloner)
      : base(original, cloner) {
    }
    protected Reducer()
      : base() {
      Parameters.Add(new ScopeParameter("CurrentScope", "The current scope from which sub-scopes should be selected."));
    }

    public sealed override IOperation Apply() {
      List<IScope> scopes = new List<IScope>(CurrentScope.SubScopes);
      List<IScope> reduced = Reduce(scopes);

      CurrentScope.SubScopes.Clear();
      CurrentScope.SubScopes.AddRange(reduced);

      return base.Apply();
    }

    protected abstract List<IScope> Reduce(List<IScope> scopes);
  }
}
