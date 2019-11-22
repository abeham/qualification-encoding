#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Threading;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Random;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("A090FDEC-1F91-4A30-9CA0-3D7ACE7A91BA")]
  [Item("SetCoverCrossover", "Aims to find a minimal set of pools that cover all qualifications. Performs a discrete crossover in case the solution is equal to one parent.")]
  public sealed class SetCoverCrossover : QualificationCrossover {
    [StorableType("4AF1821A-BCC4-4028-9050-629F533346AC")]
    public enum Solver { Greedy, BranchAndBound };
    private const string SolverParameterName = "Solver";
    private const string TimeLimitParameterName = "TimeLimit";

    public IValueLookupParameter<EnumValue<Solver>> SolverParameter => (IValueLookupParameter<EnumValue<Solver>>)Parameters[SolverParameterName];
    public IValueLookupParameter<IntValue> TimeLimitParameter => (IValueLookupParameter<IntValue>)Parameters[TimeLimitParameterName];

    [StorableConstructor]
    private SetCoverCrossover(StorableConstructorFlag _) : base(_) { }
    private SetCoverCrossover(SetCoverCrossover original, Cloner cloner) : base(original, cloner) { }
    public SetCoverCrossover() : base() {
      Parameters.Add(new ValueLookupParameter<EnumValue<Solver>>(SolverParameterName, "The solver that is used to find solutions to the set cover problem.", new EnumValue<Solver>(Solver.Greedy)));
      Parameters.Add(new ValueLookupParameter<IntValue>(TimeLimitParameterName, "The time limit in milliseconds.", new IntValue(5000)));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SetCoverCrossover(this, cloner);
    }

    protected override Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random) {
      return Apply(encoding, parents, random, SolverParameter.ActualValue.Value, TimeLimitParameter.ActualValue.Value, CancellationToken);
    }

    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random, Solver solver, int timeLimit = 5000) {
      return Apply(encoding, parents, random, solver, timeLimit, CancellationToken.None);
    }
    public static Qualification Apply(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random, Solver solver, int timeLimit, CancellationToken token) {
      var inputSets = parents.SelectMany(x => x.Groups).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => (int)Math.Round(x.Average(y => y.Value)));
      Dictionary<Qualification.Pool, int> result;
      switch (solver) {
        case Solver.Greedy:
          result = SolveSCPGreedy(random, encoding, inputSets);
          break;
        case Solver.BranchAndBound:
          var upperBound = parents.MinItems(x => x.Pools).SampleRandom(random); // each solution must cover all qualification, thus the solution with fewest pools is an upper bound
          result = SolveSCPBranchAndBound(encoding, inputSets, upperBound, timeLimit, token);
          if (result.Count == 0)
            result = SolveSCPGreedy(random, encoding, inputSets);
          break;
        default:
          throw new ArgumentException("Solver " + solver + " is not known.");
      }

      var offspring = new Qualification(result);
      QualificationHelpers.Repair(encoding, offspring, random);

      if (parents.Any(p => HammingSimilarityCalculator.Equals(offspring, p))) {
        // apply a discrete crossover as fallback
        return DiscreteCrossover.Apply(encoding, parents.Length == 2 ? parents : new ItemArray<Qualification>(parents.SampleRandom(random, 2)), random);
      }

      return offspring;
    }

    private static Dictionary<Qualification.Pool, int> SolveSCPGreedy(IRandom random, QualificationEncoding encoding, Dictionary<Qualification.Pool, int> inputSets) {
      var result = new Dictionary<Qualification.Pool, int>();
      var match = Qualification.Pool.Builder.CreateEmpty(encoding.Qualifications).Build();
      var matchCount = 0;
      // greedy-heuristic for set-covering problems
      while (matchCount < encoding.Qualifications && inputSets.Count > 0) {
        var bestSet = default(KeyValuePair<Qualification.Pool, int>);
        var bestGain = -1;
        var bestCount = 1;
        foreach (var set in inputSets) {
          var gain = HammingSimilarityCalculator.NumberOfAdditionalQualifications(match, set.Key);
          if (gain > bestGain) {
            bestSet = set;
            bestGain = gain;
            bestCount = 1;
          } else if (gain == bestGain && random.NextDouble() * (++bestCount) < 1) { // random tie-breaking
            bestSet = set;
          }
        }
        inputSets.Remove(bestSet.Key);
        result[bestSet.Key] = bestSet.Value;
        match = Qualification.Pool.Builder.CreateFrom(match).UnionWith(bestSet.Key).Build();
        matchCount = match.CountTrue;
      }
      return result;
    }

    private static Dictionary<Qualification.Pool, int> SolveSCPBranchAndBound(QualificationEncoding encoding, Dictionary<Qualification.Pool, int> sets, Qualification upperBound, int timelimit, CancellationToken token) {
      var match = Qualification.Pool.Builder.CreateEmpty(encoding.Qualifications).Build();
      var best = upperBound.Groups.Select(x => x.Key).ToList();
      using (var cancel = CancellationTokenSource.CreateLinkedTokenSource(token)) {
        cancel.CancelAfter(timelimit);
        Recurse(encoding, match, new HashSet<Qualification.Pool>(sets.Select(x => x.Key)), new Stack<Qualification.Pool>(), ref best, cancel.Token);
      }
      return best.Select(x => new { Key = x, Value = sets[x] }).ToDictionary(x => x.Key, x => x.Value);
    }

    private static void Recurse(QualificationEncoding encoding, Qualification.Pool match, HashSet<Qualification.Pool> sets, Stack<Qualification.Pool> result, ref List<Qualification.Pool> best, CancellationToken token) {
      sets = new HashSet<Qualification.Pool>(sets.Where(x => HammingSimilarityCalculator.NumberOfAdditionalQualifications(match, x) > 0)); // sets that have no additional qualifications can be ignored
      // Heuristic to sort the branches, based on a weighted sum of "qualification rareity"
      var uniqueness = match.GetFalseIndices().Select(x => new { Quali = x, Uniquness = 1 - (sets.Count(y => y[x]) / (double)sets.Count) }).ToList();
      var order = sets.Select(x => new { Choice = x, Heuristic = uniqueness.Sum(y => x[y.Quali] ? y.Uniquness : 0) })
        .OrderByDescending(x => x.Heuristic).Select(x => x.Choice).ToList();

      foreach (var o in order) {
        if (token.IsCancellationRequested) return;
        var m = Qualification.Pool.Builder.CreateFrom(match).UnionWith(o).Build();
        if (m.CountTrue == encoding.Qualifications) { // achieved a valid set covering solution
          if (best.Count > result.Count + 1) { // "+1" because the current "o" has to be included
            best = result.ToList();
            best.Add(o);
          }
          return;
        } else if (result.Count + 1 < best.Count) { // Better lower bound needed
          result.Push(o);
          sets.Remove(o);
          Recurse(encoding, m, sets, result, ref best, token);
          sets.Add(o);
          result.Pop();
        }
      }
    }
  }
}
