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
using HEAL.Attic;

namespace HeuristicLab.Optimization.Operators {
  [Item("UserDefinedManipulator", "A manipulator that can be customized with operators which it will execute one after another.")]
  [StorableType("B64E8AF0-990A-4E4F-BE1A-1194E0A6CA4E")]
  public class UserDefinedManipulator : UserDefinedOperator, IManipulator {
    [StorableConstructor]
    protected UserDefinedManipulator(StorableConstructorFlag _) : base(_) { }
    protected UserDefinedManipulator(UserDefinedManipulator original, Cloner cloner) : base(original, cloner) { }
    public UserDefinedManipulator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new UserDefinedManipulator(this, cloner);
    }
  }
}
