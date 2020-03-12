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
using HEAL.Attic;

namespace HeuristicLab.Core {
  [StorableType("FC16764E-EA5C-4D53-953B-3BEE85C7C376")]
  [Item("OperatorSet", "Represents a set of operators.")]
  public class OperatorSet : ItemSet<IOperator> {
    [StorableConstructor]
    protected OperatorSet(StorableConstructorFlag _) : base(_) { }
    protected OperatorSet(OperatorSet original, Cloner cloner) : base(original, cloner) { }
    public OperatorSet() : base() { }
    public OperatorSet(IEnumerable<IOperator> collection) : base(collection) { }

    public override IDeepCloneable Clone(Cloner cloner) { return new OperatorSet(this, cloner); }
  }
}
