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
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Optimization.Operators;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [Item("Hamming Similarity Calculator for Qualifications", "Calculates the solution similarity based on the Hamming distance between two qualification pools.")]
  [StorableType("A0B3B39E-8718-4456-A262-BD0E606B291D")]
  public sealed class HammingSimilarityCalculator : SingleObjectiveSolutionSimilarityCalculator {
    protected override bool IsCommutative {
      get { return true; }
    }

    [StorableConstructor]
    private HammingSimilarityCalculator(StorableConstructorFlag _) : base(_) { }
    private HammingSimilarityCalculator(HammingSimilarityCalculator original, Cloner cloner) : base(original, cloner) { }
    public HammingSimilarityCalculator() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new HammingSimilarityCalculator(this, cloner);
    }

    /// <summary>
    /// Calculates the similarity as the ratio of equal pools weighted by the differences in number of workers
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static double CalculateSimilarity(Qualification left, Qualification right) {
      if (left == null || right == null)
        throw new ArgumentException("Cannot calculate similarity because one or both of the provided solutions is null.");
      if (ReferenceEquals(left, right)) return 1.0;

      var first = left;
      var second = right;
      if (left.Pools > right.Pools) {
        first = right;
        second = left;
      }

      var similarity = 0.0;
      var count = 0;
      foreach (var l in first.Groups) {
        if (second.Groups.TryGetValue(l.Key, out var v)) {
          similarity += 1.0 - (Math.Abs(l.Value - v) / Math.Max(l.Value, v));
          count++;
        }
      }
      if (count > 1) similarity /= count;
      return similarity;
    }

    public static bool Equals(Qualification left, Qualification right) {
      if (left == null && right == null) return true;
      if (left == null || right == null) return false;
      if (ReferenceEquals(left, right)) return true;
      if (left.Pools != right.Pools) return false;

      foreach (var l in left.Groups) {
        if (right.Groups.TryGetValue(l.Key, out var v)) {
          if (l.Value != v) return false;
        } else return false;
      }
      return true;
    }

    public override double CalculateSolutionSimilarity(IScope leftSolution, IScope rightSolution) {
      var left = leftSolution.Variables[SolutionVariableName].Value as Qualification;
      var right = rightSolution.Variables[SolutionVariableName].Value as Qualification;

      return CalculateSimilarity(left, right);
    }

    /// <summary>
    /// Calculates the hamming distance between two qualification pools.
    /// </summary>
    /// <param name="a">The first pool</param>
    /// <param name="b">The second pool</param>
    /// <returns>The number qualification present in <paramref name="a"/>, but not in <paramref name="b"/> and vice versa.</returns>
    public static int Differences(Qualification.Pool a, Qualification.Pool b) {
      var dist = 0;
      for (var i = 0; i < a.Count; i++)
        if (a[i] != b[i]) dist++;
      return dist;
    }

    /// <summary>
    /// Calculates the number of additional qualifications not present in <paramref name="base"/>.
    /// </summary>
    /// <param name="base">The base qualification pool.</param>
    /// <param name="p">The pool to compare against <paramref name="base"/>.</param>
    /// <returns>The number of qualifications in <paramref name="p"/> that are not present in <paramref name="base"/>.</returns>
    public static int NumberOfAdditionalQualifications(Qualification.Pool @base, Qualification.Pool p) {
      var additional = 0;
      for (var i = 0; i < @base.Count; i++)
        if (!@base[i] && p[i]) additional++;
      return additional;
    }

    /// <summary>
    /// Calculates the number of overlapping qualifications present in both <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first qualification pool.</param>
    /// <param name="b">The second qualification pool.</param>
    /// <returns>The number of qualifications in both <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static int NumberOfOverlappingQualifications(Qualification.Pool a, Qualification.Pool b) {
      var overlap = 0;
      for (var i = 0; i < a.Count; i++)
        if (a[i] && b[i]) overlap++;
      return overlap;
    }

    public static bool AreDisjoint(Qualification.Pool left, Qualification.Pool right) {
      if (left == null || right == null) return true;
      if (ReferenceEquals(left, right)) return false;

      for (var i = 0; i < left.Count; i++)
        if (left[i] && right[i]) return false;
      return true;
    }
  }
}
