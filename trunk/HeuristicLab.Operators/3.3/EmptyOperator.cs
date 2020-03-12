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
using HEAL.Attic;

namespace HeuristicLab.Operators {
  /// <summary>
  /// An operator which represents an empty statement.
  /// </summary>
  [Item("EmptyOperator", "An operator which represents an empty statement.")]
  [StorableType("E385372F-A82C-4C27-8D93-A5366F454A17")]
  public sealed class EmptyOperator : SingleSuccessorOperator {
    [StorableConstructor]
    private EmptyOperator(StorableConstructorFlag _) : base(_) { }
    private EmptyOperator(EmptyOperator original, Cloner cloner)
      : base(original, cloner) {
    }
    public EmptyOperator()
      : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new EmptyOperator(this, cloner);
    }
  }
}
