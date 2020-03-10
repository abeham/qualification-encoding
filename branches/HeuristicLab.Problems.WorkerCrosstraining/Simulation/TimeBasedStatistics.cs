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
using SimSharp;

namespace HeuristicLab.Problems.WorkerCrosstraining {
  public sealed class TimeBasedStatistics {
    private readonly Simulation env;

    public int Count { get; private set; }
    public double TotalTimeD { get; private set; }
    public TimeSpan TotalTime { get { return env.ToTimeSpan(TotalTimeD); } }

    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Area { get; private set; }
    public double Mean { get; private set; }
    public double StdDev { get { return Math.Sqrt(Variance); } }
    public double Variance { get { return (TotalTimeD > 0) ? variance / TotalTimeD : 0.0; } }

    private double lastUpdateTime;
    private double lastValue;
    public double Current { get { return lastValue; } }

    private double variance;

    private bool firstSample;

    public void Reset(double initial = 0) {
      Count = 0;
      TotalTimeD = 0;
      Min = Max = Area = Mean = 0;
      variance = 0;
      firstSample = false;
      lastUpdateTime = env.NowD;
      lastValue = initial;
    }

    public TimeBasedStatistics(Simulation env, double initial = 0) {
      this.env = env;
      lastUpdateTime = env.NowD;
      lastValue = initial;
    }

    public void Increase(double value = 1) {
      UpdateTo(lastValue + value);
    }

    public void Decrease(double value = 1) {
      UpdateTo(lastValue - value);
    }

    public void UpdateTo(double value) {
      Count++;

      if (!firstSample) {
        Min = Max = Mean = value;
        firstSample = true;
      } else {
        if (value < Min) Min = value;
        if (value > Max) Max = value;

        var duration = env.NowD - lastUpdateTime;
        if (duration > 0) {
          Area += (lastValue * duration);
          var oldMean = Mean;
          Mean = oldMean + (lastValue - oldMean) * duration / (duration + TotalTimeD);
          variance = variance + (lastValue - oldMean) * (lastValue - Mean) * duration;
          TotalTimeD += duration;
        }
      }

      lastUpdateTime = env.NowD;
      lastValue = value;
    }
  }
}
