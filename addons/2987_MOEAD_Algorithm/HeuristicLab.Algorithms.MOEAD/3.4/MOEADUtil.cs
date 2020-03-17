#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2019 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Algorithms.MOEAD {
  public static class MOEADUtil {
    public static void QuickSort(double[] array, int[] idx, int from, int to) {
      if (from < to) {
        double temp = array[to];
        int tempIdx = idx[to];
        int i = from - 1;
        for (int j = from; j < to; j++) {
          if (array[j] <= temp) {
            i++;
            double tempValue = array[j];
            array[j] = array[i];
            array[i] = tempValue;
            int tempIndex = idx[j];
            idx[j] = idx[i];
            idx[i] = tempIndex;
          }
        }
        array[to] = array[i + 1];
        array[i + 1] = temp;
        idx[to] = idx[i + 1];
        idx[i + 1] = tempIdx;
        QuickSort(array, idx, from, i);
        QuickSort(array, idx, i + 1, to);
      }
    }

    public static void MinFastSort(double[] x, int[] idx, int n, int m) {
      for (int i = 0; i < m; i++) {
        for (int j = i + 1; j < n; j++) {
          if (x[i] > x[j]) {
            double temp = x[i];
            x[i] = x[j];
            x[j] = temp;
            int id = idx[i];
            idx[i] = idx[j];
            idx[j] = id;
          }
        }
      }
    }

    public static IList<IMOEADSolution> GetSubsetOfEvenlyDistributedSolutions(IRandom random, IList<IMOEADSolution> solutionList, int newSolutionListSize) {
      if (solutionList == null || solutionList.Count == 0) {
        throw new ArgumentException("Solution list is null or empty.");
      }

      return solutionList[0].Dimensions == 2
        ? TwoObjectivesCase(solutionList, newSolutionListSize)
        : MoreThanTwoObjectivesCase(random, solutionList, newSolutionListSize);
    }

    private static IList<IMOEADSolution> TwoObjectivesCase(IList<IMOEADSolution> solutionList, int newSolutionListSize) {
      var resultSolutionList = new IMOEADSolution[newSolutionListSize];

      // compute weight vectors
      double[][] lambda = new double[newSolutionListSize][];
      var values = SequenceGenerator.GenerateSteps(0m, 1m, 1m / newSolutionListSize).ToArray();
      for (int i = 0; i < newSolutionListSize; ++i) {
        var weights = new double[newSolutionListSize];
        weights[0] = (double)values[i];
        weights[1] = 1 - weights[0];

        lambda[i] = weights;
      }

      var idealPoint = new double[2];
      foreach (var solution in solutionList) {
        // update ideal point
        UpdateIdeal(idealPoint, solution.Qualities);
      }

      // Select the best solution for each weight vector
      for (int i = 0; i < newSolutionListSize; i++) {
        var currentBest = solutionList[0];
        double value = ScalarizingFitnessFunction(currentBest, lambda[i], idealPoint);
        for (int j = 1; j < solutionList.Count; j++) {
          double aux = ScalarizingFitnessFunction(solutionList[j], lambda[i], idealPoint); // we are looking for the best for the weight i
          if (aux < value) { // solution in position j is better!
            value = aux;
            currentBest = solutionList[j];
          }
        }
        resultSolutionList[i] = (MOEADSolution)currentBest.Clone();
      }

      return resultSolutionList;
    }

    private static IList<IMOEADSolution> MoreThanTwoObjectivesCase(IRandom random, IList<IMOEADSolution> solutionList, int newSolutionListSize) {
      var resultSolutionList = new List<IMOEADSolution>();

      int randomIndex = random.Next(0, solutionList.Count);

      var candidate = new List<IMOEADSolution>();
      resultSolutionList.Add(solutionList[randomIndex]);

      for (int i = 0; i < solutionList.Count; ++i) {
        if (i != randomIndex) {
          candidate.Add(solutionList[i]);
        }
      }

      while (resultSolutionList.Count < newSolutionListSize) {
        int index = 0;
        var selected = candidate[0]; // it should be a next! (n <= population size!)
        double aux = CalculateBestDistance(selected, solutionList);
        int i = 1;
        while (i < candidate.Count) {
          var nextCandidate = candidate[i];
          double distanceValue = CalculateBestDistance(nextCandidate, solutionList);
          if (aux < distanceValue) {
            index = i;
            aux = distanceValue;
          }
          i++;
        }

        // add the selected to res and remove from candidate list
        var removedSolution = candidate[index];
        candidate.RemoveAt(index);
        resultSolutionList.Add((MOEADSolution)removedSolution.Clone());
      }

      return resultSolutionList;
    }

    private static double ScalarizingFitnessFunction(IMOEADSolution currentBest, double[] lambda, double[] idealPoint) {
      double maxFun = -1.0e+30;

      for (int n = 0; n < idealPoint.Length; n++) {
        double diff = Math.Abs(currentBest.Qualities[n] - idealPoint[n]);

        double functionValue;
        if (lambda[n] == 0) {
          functionValue = 0.0001 * diff;
        } else {
          functionValue = diff * lambda[n];
        }
        if (functionValue > maxFun) {
          maxFun = functionValue;
        }
      }

      return maxFun;
    }

    public static void UpdateIdeal(double[] idealPoint, double[] point) {
      for (int i = 0; i < point.Length; ++i) {
        if (double.IsInfinity(point[i]) || double.IsNaN(point[i])) {
          continue;
        }

        if (idealPoint[i] > point[i]) {
          idealPoint[i] = point[i];
        }
      }
    }

    public static void UpdateNadir(double[] nadirPoint, double[] point) {
      for (int i = 0; i < point.Length; ++i) {
        if (double.IsInfinity(point[i]) || double.IsNaN(point[i])) {
          continue;
        }

        if (nadirPoint[i] < point[i]) {
          nadirPoint[i] = point[i];
        }
      }
    }

    public static void UpdateIdeal(double[] idealPoint, IList<IMOEADSolution> solutions) {
      foreach (var s in solutions) {
        UpdateIdeal(idealPoint, s.Qualities);
      }
    }

    public static void UpdateNadir(double[] nadirPoint, IList<IMOEADSolution> solutions) {
      foreach (var s in solutions) {
        UpdateNadir(nadirPoint, s.Qualities);
      }
    }

    private static double CalculateBestDistance(IMOEADSolution solution, IList<IMOEADSolution> solutionList) {
      var best = solutionList.Min(x => EuclideanDistance(solution.Qualities, x.Qualities));
      if (double.IsNaN(best) || double.IsInfinity(best)) {
        best = double.MaxValue;
      }
      return best;
    }

    public static double EuclideanDistance(double[] a, double[] b) {
      if (a.Length != b.Length) {
        throw new ArgumentException("Euclidean distance: the arrays have different lengths.");
      }

      var distance = 0d;
      for (int i = 0; i < a.Length; ++i) {
        var d = a[i] - b[i];
        distance += d * d;
      }
      return Math.Sqrt(distance);
    }
  }
}
