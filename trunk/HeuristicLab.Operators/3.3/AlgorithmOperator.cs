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

using System.Drawing;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HEAL.Attic;

namespace HeuristicLab.Operators {
  /// <summary>
  /// An operator which represents an algorithm represented as an operator graph.
  /// </summary>
  [Item("AlgorithmOperator", "An operator which represents an algorithm represented as an operator graph.")]
  [StorableType("A0F318EF-7614-4208-99C0-56DF89D4C23D")]
  public abstract class AlgorithmOperator : SingleSuccessorOperator, IOperatorGraphOperator {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Module; }
    }
    public override Image ItemImage {
      get {
        if (Breakpoint) return HeuristicLab.Common.Resources.VSImageLibrary.BreakpointActive;
        else return base.ItemImage;
      }
    }
    [Storable]
    private OperatorGraph operatorGraph;
    public OperatorGraph OperatorGraph {
      get { return operatorGraph; }
    }

    [StorableConstructor]
    protected AlgorithmOperator(StorableConstructorFlag _) : base(_) { }
    protected AlgorithmOperator(AlgorithmOperator original, Cloner cloner)
      : base(original, cloner) {
      this.operatorGraph = cloner.Clone<OperatorGraph>(original.operatorGraph);
    }
    protected AlgorithmOperator()
      : base() {
      operatorGraph = new OperatorGraph();
    }

    public override IOperation Apply() {
      OperationCollection next = new OperationCollection(base.Apply());
      if (operatorGraph.InitialOperator != null)
        next.Insert(0, ExecutionContext.CreateChildOperation(operatorGraph.InitialOperator));
      return next;
    }
  }
}
