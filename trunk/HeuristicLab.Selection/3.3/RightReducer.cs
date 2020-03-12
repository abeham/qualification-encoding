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
using HeuristicLab.Optimization;
using HEAL.Attic;

namespace HeuristicLab.Selection {
  /// <summary>
  /// An operator which reduces to the sub-scopes of the rightmost sub-scope of the current scope.
  /// </summary>
  [Item("RightReducer", "An operator which reduces to the sub-scopes of the rightmost sub-scope of the current scope.")]
  [StorableType("D06F6701-B166-4AF1-9EB7-BA6C187DFBB1")]
  public sealed class RightReducer : Reducer, IReducer {
    [StorableConstructor]
    private RightReducer(StorableConstructorFlag _) : base(_) { }
    private RightReducer(RightReducer original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new RightReducer(this, cloner);
    }
    public RightReducer() : base() { }

    protected override List<IScope> Reduce(List<IScope> scopes) {
      List<IScope> reduced = new List<IScope>();
      if (scopes.Count > 0) reduced.AddRange(scopes[scopes.Count - 1].SubScopes);
      return reduced;
    }
  }
}
