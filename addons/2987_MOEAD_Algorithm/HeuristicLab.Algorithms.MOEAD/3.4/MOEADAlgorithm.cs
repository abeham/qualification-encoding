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

using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Random;
using System.Linq;
using CancellationToken = System.Threading.CancellationToken;

namespace HeuristicLab.Algorithms.MOEAD {
  [Item("Multi-objective Evolutionary Algorithm based on Decomposition (MOEA/D)", "MOEA/D implementation adapted from jMetal.")]
  [Creatable(CreatableAttribute.Categories.PopulationBasedAlgorithms, Priority = 125)]
  [StorableType("FE39AD23-B3BF-4368-BB79-FE5BF4C36272")]
  public class MOEADAlgorithm : MOEADAlgorithmBase {
    public MOEADAlgorithm() { }

    protected MOEADAlgorithm(MOEADAlgorithm original, Cloner cloner) : base(original, cloner) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MOEADAlgorithm(this, cloner);
    }

    [StorableConstructor]
    protected MOEADAlgorithm(StorableConstructorFlag deserializing) : base(deserializing) { }

    protected override void Run(CancellationToken cancellationToken) {
      if (previousExecutionState != ExecutionState.Paused) {
        InitializeAlgorithm(cancellationToken);
      }

      var populationSize = PopulationSize.Value;
      bool[] maximization = ((BoolArray)Problem.MaximizationParameter.ActualValue).CloneAsArray();
      var maximumEvaluatedSolutions = MaximumEvaluatedSolutions.Value;
      var crossover = Crossover;
      var crossoverProbability = CrossoverProbability.Value;
      var mutator = Mutator;
      var mutationProbability = MutationProbability.Value;
      var evaluator = Problem.Evaluator;
      var analyzer = Analyzer;
      var neighbourhoodSelectionProbability = NeighbourhoodSelectionProbability;
      var rand = RandomParameter.Value;
      var normalizeObjectives = NormalizeObjectives;
      var maximumNumberOfReplacedSolutions = MaximumNumberOfReplacedSolutions;

      // cancellation token for the inner operations which should not be immediately cancelled
      var innerToken = new CancellationToken();

      while (evaluatedSolutions < maximumEvaluatedSolutions && !cancellationToken.IsCancellationRequested) {
        foreach (var subProblemId in Enumerable.Range(0, populationSize).Shuffle(rand)) {
          var neighbourType = ChooseNeighborType(rand, neighbourhoodSelectionProbability);
          var mates = MatingSelection(rand, subProblemId, 2, neighbourType); // select parents
          var s1 = (IScope)population[mates[0]].Individual.Clone();
          var s2 = (IScope)population[mates[1]].Individual.Clone();
          s1.Parent = s2.Parent = globalScope;

          IScope childScope = null;
          // crossover
          if (rand.NextDouble() < crossoverProbability) {
            childScope = new Scope($"{mates[0]}+{mates[1]}") { Parent = executionContext.Scope };
            childScope.SubScopes.Add(s1);
            childScope.SubScopes.Add(s2);
            var op = executionContext.CreateChildOperation(crossover, childScope);
            ExecuteOperation(executionContext, innerToken, op);
            childScope.SubScopes.Clear();
          }

          // mutation
          if (rand.NextDouble() < mutationProbability) {
            childScope = childScope ?? s1;
            var op = executionContext.CreateChildOperation(mutator, childScope);
            ExecuteOperation(executionContext, innerToken, op);
          }

          // evaluation
          if (childScope != null) {
            var op = executionContext.CreateChildOperation(evaluator, childScope);
            ExecuteOperation(executionContext, innerToken, op);
            var qualities = (DoubleArray)childScope.Variables["Qualities"].Value;
            var childSolution = new MOEADSolution(childScope, maximization.Length, 0);
            // set child qualities
            for (int j = 0; j < maximization.Length; ++j) {
              childSolution.Qualities[j] = maximization[j] ? 1 - qualities[j] : qualities[j];
            }
            IdealPoint.UpdateIdeal(childSolution.Qualities);
            NadirPoint.UpdateNadir(childSolution.Qualities);
            // update neighbourhood will insert the child into the population
            UpdateNeighbourHood(rand, childSolution, subProblemId, neighbourType, maximumNumberOfReplacedSolutions, normalizeObjectives);

            ++evaluatedSolutions;
          } else {
            // no crossover or mutation were applied, a child was not produced, do nothing
          }

          if (evaluatedSolutions >= maximumEvaluatedSolutions) {
            break;
          }
        }
        // run analyzer
        var analyze = executionContext.CreateChildOperation(analyzer, globalScope);
        ExecuteOperation(executionContext, innerToken, analyze);

        UpdateParetoFronts();

        Results.AddOrUpdateResult("IdealPoint", new DoubleArray(IdealPoint));
        Results.AddOrUpdateResult("NadirPoint", new DoubleArray(NadirPoint));
        Results.AddOrUpdateResult("Evaluated Solutions", new IntValue(evaluatedSolutions));

        globalScope.SubScopes.Replace(population.Select(x => (IScope)x.Individual));
      }
    }
  }
}
