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

using System;

namespace HeuristicLab.Problems.WorkerCrosstraining {
  public sealed class BasicStatistics {
    public int Count { get; private set; }

    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Total { get; private set; }
    public double Mean { get; private set; }
    public double StdDev { get { return Math.Sqrt(Variance); } }
    public double Variance { get { return (Count > 0) ? variance / Count : 0.0; } }
    private double variance;
    
    public BasicStatistics() {
    }

    public void Add(double value) {
      Count++;

      if (Count == 1) {
        Min = Max = Mean = value;
      } else {
        if (value < Min) Min = value;
        if (value > Max) Max = value;

        Total += value;
        var oldMean = Mean;
        Mean = oldMean + (value - oldMean) / Count;
        variance = variance + (value - oldMean) * (value - Mean);
      }
    }

    public void Reset() {
      Count = 0;
      Min = Max = Total = Mean = 0;
      variance = 0;
    }
  }
}
