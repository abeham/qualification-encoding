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

using System.Collections.Generic;
using System.Drawing;
using HEAL.Attic;

namespace HeuristicLab.Operators.Views.GraphVisualization {
  [StorableType("d6ae073c-ddf7-4923-abed-1b3c864ac492")]
  public interface IOperatorShapeInfo : IShapeInfo {
    bool Collapsed { get; set; }
    string Title { get; set; }
    string TypeName { get; set; }
    Color Color { get; set; }
    Color LineColor { get; set; }
    float LineWidth { get; set; }
    Bitmap Icon { get; set; }

    void AddConnector(string connectorName);
    void RemoveConnector(string connectorName);

    IEnumerable<string> Labels { get; }
    void UpdateLabels(IEnumerable<string> labels);
  }
}
