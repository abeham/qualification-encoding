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
using System.Drawing;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Analysis;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.ExpressionGenerator;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.TestFunctions.MultiObjective;
using HeuristicLab.Random;
using CancellationToken = System.Threading.CancellationToken;

namespace HeuristicLab.Algorithms.MOEAD {
  [Item("MOEADAlgorithmBase", "Base class for all MOEA/D algorithm variants.")]
  [StorableType("E00BAC79-C6F9-42B6-8468-DEEC7FFCE804")]
  public abstract class MOEADAlgorithmBase : BasicAlgorithm {
    #region data members
    [StorableType("C04DB21A-316F-4210-9CA7-A915BE8EBC96")]
    protected enum NeighborType { NEIGHBOR, POPULATION }

    [StorableType("FE35F480-E522-45E0-A229-99E61DA7B8BC")]
    // TCHE = Chebyshev (Tchebyshev)
    // PBI  = Penalty-based boundary intersection
    // AGG  = Weighted sum
    public enum FunctionType { TCHE, PBI, AGG }

    [Storable]
    protected IRandom Random { get; set; }

    [Storable]
    protected double[] IdealPoint { get; set; }
    [Storable]
    protected double[] NadirPoint { get; set; } // potentially useful for objective normalization

    [Storable]
    protected double[][] lambda;

    [Storable]
    protected int[][] neighbourhood;

    [Storable]
    protected IMOEADSolution[] solutions;

    [Storable]
    protected FunctionType functionType;

    [Storable]
    protected IMOEADSolution[] population;

    [Storable]
    protected IMOEADSolution[] offspringPopulation;

    [Storable]
    protected IMOEADSolution[] jointPopulation;

    [Storable]
    protected int evaluatedSolutions;

    [Storable]
    protected ExecutionContext executionContext;

    [Storable]
    protected IScope globalScope;

    [Storable]
    protected ExecutionState previousExecutionState;

    [Storable]
    protected ExecutionState executionState;
    #endregion

    #region parameters
    private const string SeedParameterName = "Seed";
    private const string SetSeedRandomlyParameterName = "SetSeedRandomly";
    private const string PopulationSizeParameterName = "PopulationSize";
    private const string ResultPopulationSizeParameterName = "ResultPopulationSize";
    private const string CrossoverProbabilityParameterName = "CrossoverProbability";
    private const string CrossoverParameterName = "Crossover";
    private const string MutationProbabilityParameterName = "MutationProbability";
    private const string MutatorParameterName = "Mutator";
    private const string MaximumEvaluatedSolutionsParameterName = "MaximumEvaluatedSolutions";
    private const string AnalyzerParameterName = "Analyzer";
    // MOEA-D parameters
    private const string NeighbourSizeParameterName = "NeighbourSize";
    private const string NeighbourhoodSelectionProbabilityParameterName = "NeighbourhoodSelectionProbability";
    private const string MaximumNumberOfReplacedSolutionsParameterName = "MaximumNumberOfReplacedSolutions";
    private const string FunctionTypeParameterName = "FunctionType";
    private const string NormalizeObjectivesParameterName = "NormalizeObjectives";

    [Storable] public IValueParameter<MultiAnalyzer> AnalyzerParameter { get; private set; }
    [Storable] public IConstrainedValueParameter<StringValue> FunctionTypeParameter { get; private set; }
    [Storable] public IFixedValueParameter<IntValue> NeighbourSizeParameter { get; private set; }
    [Storable] public IFixedValueParameter<BoolValue> NormalizeObjectivesParameter { get; private set; }
    [Storable] public IFixedValueParameter<IntValue> MaximumNumberOfReplacedSolutionsParameter { get; private set; }
    [Storable] public IFixedValueParameter<DoubleValue> NeighbourhoodSelectionProbabilityParameter { get; private set; }
    [Storable] public IFixedValueParameter<IntValue> SeedParameter { get; private set; }
    [Storable] public IFixedValueParameter<BoolValue> SetSeedRandomlyParameter { get; private set; }
    [Storable] public IValueParameter<IntValue> PopulationSizeParameter { get; private set; }
    [Storable] public IValueParameter<IntValue> ResultPopulationSizeParameter { get; private set; }
    [Storable] public IValueParameter<PercentValue> CrossoverProbabilityParameter { get; private set; }
    [Storable] public IConstrainedValueParameter<ICrossover> CrossoverParameter { get; private set; }
    [Storable] public IValueParameter<PercentValue> MutationProbabilityParameter { get; private set; }
    [Storable] public IConstrainedValueParameter<IManipulator> MutatorParameter { get; private set; }
    [Storable] public IValueParameter<IntValue> MaximumEvaluatedSolutionsParameter { get; private set; }
    #endregion

    #region parameter properties
    public new IMultiObjectiveHeuristicOptimizationProblem Problem {
      get { return (IMultiObjectiveHeuristicOptimizationProblem)base.Problem; }
      set { base.Problem = value; }
    }
    public int Seed {
      get { return SeedParameter.Value.Value; }
      set { SeedParameter.Value.Value = value; }
    }
    public bool SetSeedRandomly {
      get { return SetSeedRandomlyParameter.Value.Value; }
      set { SetSeedRandomlyParameter.Value.Value = value; }
    }
    public bool NormalizeObjectives {
      get { return NormalizeObjectivesParameter.Value.Value; }
      set { NormalizeObjectivesParameter.Value.Value = value; }
    }
    public int PopulationSize {
      get { return PopulationSizeParameter.Value.Value; }
      set { PopulationSizeParameter.Value.Value = value; }
    }
    public int ResultPopulationSize {
      get { return ResultPopulationSizeParameter.Value.Value; }
      set { ResultPopulationSizeParameter.Value.Value = value; }
    }
    public double CrossoverProbability {
      get { return CrossoverProbabilityParameter.Value.Value; }
      set { CrossoverProbabilityParameter.Value.Value = value; }
    }
    public ICrossover Crossover {
      get { return CrossoverParameter.Value; }
      set { CrossoverParameter.Value = value; }
    }
    public double MutationProbability {
      get { return MutationProbabilityParameter.Value.Value; }
      set { MutationProbabilityParameter.Value.Value = value; }
    }
    public IManipulator Mutator {
      get { return MutatorParameter.Value; }
      set { MutatorParameter.Value = value; }
    }
    public MultiAnalyzer Analyzer {
      get { return AnalyzerParameter.Value; }
      set { AnalyzerParameter.Value = value; }
    }
    public int MaximumEvaluatedSolutions {
      get { return MaximumEvaluatedSolutionsParameter.Value.Value; }
      set { MaximumEvaluatedSolutionsParameter.Value.Value = value; }
    }
    public int NeighbourSize {
      get { return NeighbourSizeParameter.Value.Value; }
      set { NeighbourSizeParameter.Value.Value = value; }
    }
    public int MaximumNumberOfReplacedSolutions {
      get { return MaximumNumberOfReplacedSolutionsParameter.Value.Value; }
      set { MaximumNumberOfReplacedSolutionsParameter.Value.Value = value; }
    }
    public double NeighbourhoodSelectionProbability {
      get { return NeighbourhoodSelectionProbabilityParameter.Value.Value; }
      set { NeighbourhoodSelectionProbabilityParameter.Value.Value = value; }
    }
    #endregion

    #region constructors
    public MOEADAlgorithmBase() {
      Parameters.Add(SeedParameter = new FixedValueParameter<IntValue>(SeedParameterName, "The random seed used to initialize the new pseudo random number generator.", new IntValue(0)));
      Parameters.Add(SetSeedRandomlyParameter = new FixedValueParameter<BoolValue>(SetSeedRandomlyParameterName, "True if the random seed should be set to a random value, otherwise false.", new BoolValue(true)));
      Parameters.Add(PopulationSizeParameter = new ValueParameter<IntValue>(PopulationSizeParameterName, "The size of the population of solutions.", new IntValue(100)));
      Parameters.Add(ResultPopulationSizeParameter = new ValueParameter<IntValue>(ResultPopulationSizeParameterName, "The size of the population of solutions.", new IntValue(100)));
      Parameters.Add(CrossoverProbabilityParameter = new ValueParameter<PercentValue>(CrossoverProbabilityParameterName, "The probability that the crossover operator is applied.", new PercentValue(0.9)));
      Parameters.Add(CrossoverParameter = new ConstrainedValueParameter<ICrossover>(CrossoverParameterName, "The operator used to cross solutions."));
      Parameters.Add(MutationProbabilityParameter = new ValueParameter<PercentValue>(MutationProbabilityParameterName, "The probability that the mutation operator is applied on a solution.", new PercentValue(0.25)));
      Parameters.Add(MutatorParameter = new ConstrainedValueParameter<IManipulator>(MutatorParameterName, "The operator used to mutate solutions."));
      Parameters.Add(AnalyzerParameter = new ValueParameter<MultiAnalyzer>(AnalyzerParameterName, "The operator used to analyze each generation.", new MultiAnalyzer()));
      Parameters.Add(MaximumEvaluatedSolutionsParameter = new ValueParameter<IntValue>(MaximumEvaluatedSolutionsParameterName, "The maximum number of evaluated solutions (approximately).", new IntValue(100_000)));
      Parameters.Add(NeighbourSizeParameter = new FixedValueParameter<IntValue>(NeighbourSizeParameterName, new IntValue(20)));
      Parameters.Add(MaximumNumberOfReplacedSolutionsParameter = new FixedValueParameter<IntValue>(MaximumNumberOfReplacedSolutionsParameterName, new IntValue(2)));
      Parameters.Add(NeighbourhoodSelectionProbabilityParameter = new FixedValueParameter<DoubleValue>(NeighbourhoodSelectionProbabilityParameterName, new DoubleValue(0.1)));
      Parameters.Add(NormalizeObjectivesParameter = new FixedValueParameter<BoolValue>(NormalizeObjectivesParameterName, new BoolValue(true)));
      Parameters.Add(FunctionTypeParameter = new ConstrainedValueParameter<StringValue>(FunctionTypeParameterName));

      foreach (var s in new[] { "Chebyshev", "PBI", "Weighted Sum" }) {
        FunctionTypeParameter.ValidValues.Add(new StringValue(s));
      }
    }

    protected MOEADAlgorithmBase(MOEADAlgorithmBase original, Cloner cloner) : base(original, cloner) {
      functionType = original.functionType;
      evaluatedSolutions = original.evaluatedSolutions;
      previousExecutionState = original.previousExecutionState;

      if (original.IdealPoint != null) {
        IdealPoint = (double[])original.IdealPoint.Clone();
      }

      if (original.NadirPoint != null) {
        NadirPoint = (double[])original.NadirPoint.Clone();
      }

      if (original.lambda != null) {
        lambda = (double[][])original.lambda.Clone();
      }

      if (original.neighbourhood != null) {
        neighbourhood = (int[][])original.neighbourhood.Clone();
      }

      if (original.solutions != null) {
        solutions = original.solutions.Select(cloner.Clone).ToArray();
      }

      if (original.population != null) {
        population = original.population.Select(cloner.Clone).ToArray();
      }

      if (original.offspringPopulation != null) {
        offspringPopulation = original.offspringPopulation.Select(cloner.Clone).ToArray();
      }

      if (original.jointPopulation != null) {
        jointPopulation = original.jointPopulation.Select(cloner.Clone).ToArray();
      }

      executionContext = cloner.Clone(original.executionContext);
      globalScope = cloner.Clone(original.globalScope);
      Random = cloner.Clone(original.Random);

      SeedParameter = cloner.Clone(original.SeedParameter);
      SetSeedRandomlyParameter = cloner.Clone(original.SetSeedRandomlyParameter);
      PopulationSizeParameter = cloner.Clone(original.PopulationSizeParameter);
      ResultPopulationSizeParameter = cloner.Clone(original.ResultPopulationSizeParameter);
      CrossoverProbabilityParameter = cloner.Clone(original.CrossoverProbabilityParameter);
      CrossoverParameter = cloner.Clone(original.CrossoverParameter);
      MutationProbabilityParameter = cloner.Clone(original.MutationProbabilityParameter);
      MutatorParameter = cloner.Clone(original.MutatorParameter);
      AnalyzerParameter = cloner.Clone(original.AnalyzerParameter);
      MaximumEvaluatedSolutionsParameter = cloner.Clone(original.MaximumEvaluatedSolutionsParameter);
      NeighbourSizeParameter = cloner.Clone(original.NeighbourSizeParameter);
      MaximumNumberOfReplacedSolutionsParameter = cloner.Clone(original.MaximumNumberOfReplacedSolutionsParameter);
      NeighbourhoodSelectionProbabilityParameter = cloner.Clone(original.NeighbourhoodSelectionProbabilityParameter);
      NormalizeObjectivesParameter = cloner.Clone(original.NormalizeObjectivesParameter);
      FunctionTypeParameter = cloner.Clone(original.FunctionTypeParameter);
    }


    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      if (!Parameters.ContainsKey(NormalizeObjectivesParameterName)) {
        Parameters.Add(NormalizeObjectivesParameter = new FixedValueParameter<BoolValue>(NormalizeObjectivesParameterName, new BoolValue(true)));
      }
      if (SeedParameter == null) SeedParameter = (IFixedValueParameter<IntValue>)Parameters[SeedParameterName];
      if (SetSeedRandomlyParameter == null) SetSeedRandomlyParameter = (IFixedValueParameter<BoolValue>)Parameters[SetSeedRandomlyParameterName];
      if (PopulationSizeParameter == null) PopulationSizeParameter = (IValueParameter<IntValue>)Parameters[PopulationSizeParameterName];
      if (ResultPopulationSizeParameter == null) ResultPopulationSizeParameter = (IValueParameter<IntValue>)Parameters[ResultPopulationSizeParameterName];
      if (CrossoverProbabilityParameter == null) CrossoverProbabilityParameter = (IValueParameter<PercentValue>)Parameters[CrossoverProbabilityParameterName];
      if (CrossoverParameter == null) CrossoverParameter = (IConstrainedValueParameter<ICrossover>)Parameters[CrossoverParameterName];
      if (MutationProbabilityParameter == null) MutationProbabilityParameter = (IValueParameter<PercentValue>)Parameters[MutationProbabilityParameterName];
      if (MutatorParameter == null) MutatorParameter = (IConstrainedValueParameter<IManipulator>)Parameters[MutatorParameterName];
      if (AnalyzerParameter == null) AnalyzerParameter = (IValueParameter<MultiAnalyzer>)Parameters[AnalyzerParameterName];
      if (MaximumEvaluatedSolutionsParameter == null) MaximumEvaluatedSolutionsParameter = (IValueParameter<IntValue>)Parameters[MaximumEvaluatedSolutionsParameterName];
      if (NeighbourSizeParameter == null) NeighbourSizeParameter = (IFixedValueParameter<IntValue>)Parameters[NeighbourSizeParameterName];
      if (MaximumNumberOfReplacedSolutionsParameter == null) MaximumNumberOfReplacedSolutionsParameter = (IFixedValueParameter<IntValue>)Parameters[MaximumNumberOfReplacedSolutionsParameterName];
      if (NeighbourhoodSelectionProbabilityParameter == null) NeighbourhoodSelectionProbabilityParameter = (IFixedValueParameter<DoubleValue>)Parameters[NeighbourhoodSelectionProbabilityParameterName];
      if (NormalizeObjectivesParameter == null) NormalizeObjectivesParameter = (IFixedValueParameter<BoolValue>)Parameters[NormalizeObjectivesParameterName];
      if (FunctionTypeParameter == null) FunctionTypeParameter = (IConstrainedValueParameter<StringValue>)Parameters[FunctionTypeParameterName];
      if (Parameters.ContainsKey("Random")) {
        Random = ((IValueParameter<IRandom>)Parameters["Random"]).Value;
        Parameters.Remove("Random");
      }
    }

    [StorableConstructor]
    protected MOEADAlgorithmBase(StorableConstructorFlag deserializing) : base(deserializing) { }
    #endregion

    private void InitializePopulation(CancellationToken cancellationToken, bool[] maximization) {
      var creator = Problem.SolutionCreator;
      var evaluator = Problem.Evaluator;

      var dimensions = maximization.Length;
      population = new IMOEADSolution[PopulationSize];

      var parentScope = executionContext.Scope;
      // first, create all individuals
      for (int i = 0; i < PopulationSize; ++i) {
        var childScope = new Scope(i.ToString()) { Parent = parentScope };
        ExecuteOperation(cancellationToken, executionContext.CreateChildOperation(creator, childScope));
        parentScope.SubScopes.Add(childScope);
      }

      // then, evaluate them and update qualities
      for (int i = 0; i < PopulationSize; ++i) {
        var childScope = parentScope.SubScopes[i];
        ExecuteOperation(cancellationToken, executionContext.CreateChildOperation(evaluator, childScope));

        var qualities = (DoubleArray)childScope.Variables[evaluator.QualitiesParameter.TranslatedName].Value;
        var solution = new MOEADSolution(childScope, dimensions, 0);
        for (int j = 0; j < dimensions; ++j) {
          solution.Qualities[j] = maximization[j] ? 1 - qualities[j] : qualities[j];
        }
        population[i] = solution;
      }
    }

    protected void InitializeAlgorithm(CancellationToken cancellationToken) {
      if (SetSeedRandomly) Seed = RandomSeedGenerator.GetSeed();
      Random = new FastRandom(Seed);

      globalScope.Variables.Add(new Variable("Random", Random));
      bool[] maximization = ((BoolArray)Problem.MaximizationParameter.ActualValue).CloneAsArray();
      var dimensions = maximization.Length;

      InitializePopulation(cancellationToken, maximization);
      InitializeUniformWeights(dimensions);
      InitializeNeighbourHood(lambda);

      //IdealPoint = Enumerable.Repeat(double.MaxValue, dimensions).ToArray();
      IdealPoint = new double[dimensions];
      MOEADUtil.UpdateIdeal(IdealPoint, population);

      NadirPoint = Enumerable.Repeat(double.MinValue, dimensions).ToArray();
      //NadirPoint = new double[dimensions];
      MOEADUtil.UpdateNadir(NadirPoint, population);

      var functionTypeString = FunctionTypeParameter.Value.Value;
      switch (functionTypeString) {
        case "Chebyshev":
          functionType = FunctionType.TCHE;
          break;
        case "PBI":
          functionType = FunctionType.PBI;
          break;
        case "Weighted Sum":
          functionType = FunctionType.AGG;
          break;
      }

      evaluatedSolutions = PopulationSize;
    }

    protected override void Initialize(CancellationToken cancellationToken) {
      globalScope = new Scope("Global Scope");
      executionContext = new ExecutionContext(null, this, globalScope);

      // set the execution context for parameters to allow lookup
      foreach (var parameter in Problem.Parameters.OfType<IValueParameter>()) {
        // we need all of these in order for the wiring of the operators to work
        globalScope.Variables.Add(new Variable(parameter.Name, parameter.Value));
      }
      globalScope.Variables.Add(new Variable("Results", Results)); // make results available as a parameter for analyzers etc.

      base.Initialize(cancellationToken);
    }

    public override bool SupportsPause => true;

    protected void InitializeUniformWeights(int dimensions) {
      lambda = Enumerable.Range(0, PopulationSize).Select(_ => GenerateSample(dimensions)).ToArray();
    }

    // implements random number generation from https://en.wikipedia.org/wiki/Dirichlet_distribution#Random_number_generation
    private double[] GenerateSample(int dim) {
      var sum = 0d;
      var sample = new double[dim];
      for (int i = 0; i < dim; ++i) {
        sample[i] = GammaDistributedRandom.NextDouble(Random, 1, 1);
        sum += sample[i];
      }
      for (int i = 0; i < dim; ++i) {
        sample[i] /= sum;
      }
      return sample;
    }

    protected void InitializeNeighbourHood(double[][] lambda) {
      neighbourhood = new int[PopulationSize][];

      var x = new double[PopulationSize];
      var idx = new int[PopulationSize];

      for (int i = 0; i < PopulationSize; ++i) {
        for (int j = 0; j < PopulationSize; ++j) {
          x[j] = MOEADUtil.EuclideanDistance(lambda[i], lambda[j]);
          idx[j] = j;
        }

        MOEADUtil.MinFastSort(x, idx, PopulationSize, NeighbourSize);
        neighbourhood[i] = (int[])idx.Clone();
      }
    }

    protected NeighborType ChooseNeighborType() {
      return Random.NextDouble() < NeighbourhoodSelectionProbability
        ? NeighborType.NEIGHBOR
        : NeighborType.POPULATION;
    }

    protected IList<IMOEADSolution> ParentSelection(int subProblemId, NeighborType neighbourType) {
      List<int> matingPool = MatingSelection(subProblemId, 2, neighbourType);

      var parents = new IMOEADSolution[3];

      parents[0] = population[matingPool[0]];
      parents[1] = population[matingPool[1]];
      parents[2] = population[subProblemId];

      return parents;
    }

    protected List<int> MatingSelection(int subproblemId, int numberOfSolutionsToSelect, NeighborType neighbourType) {
      var listOfSolutions = new List<int>(numberOfSolutionsToSelect);

      int neighbourSize = neighbourhood[subproblemId].Length;
      while (listOfSolutions.Count < numberOfSolutionsToSelect) {
        var selectedSolution = neighbourType == NeighborType.NEIGHBOR
          ? neighbourhood[subproblemId][Random.Next(neighbourSize)]
          : Random.Next(PopulationSize);

        bool flag = true;
        foreach (int individualId in listOfSolutions) {
          if (individualId == selectedSolution) {
            flag = false;
            break;
          }
        }

        if (flag) {
          listOfSolutions.Add(selectedSolution);
        }
      }

      return listOfSolutions;
    }

    protected void UpdateNeighbourHood(IMOEADSolution individual, int subProblemId, NeighborType neighbourType) {
      int replacedSolutions = 0;
      int size = neighbourType == NeighborType.NEIGHBOR ? NeighbourSize : population.Length;

      foreach (var i in Enumerable.Range(0, size).Shuffle(Random)) {
        int k = neighbourType == NeighborType.NEIGHBOR ? neighbourhood[subProblemId][i] : i;

        double f1 = CalculateFitness(population[k].Qualities, lambda[k]);
        double f2 = CalculateFitness(individual.Qualities, lambda[k]);

        if (f2 < f1) {
          population[k] = individual;
          replacedSolutions++;
        }

        if (replacedSolutions >= MaximumNumberOfReplacedSolutions) {
          return;
        }
      }
    }

    private double CalculateFitness(double[] qualities, double[] lambda) {
      int dim = qualities.Length;
      switch (functionType) {
        case FunctionType.TCHE: {
            double maxFun = double.MinValue;

            for (int n = 0; n < dim; n++) {
              // deal with NaN and Infinity 
              var q = qualities[n];
              if (double.IsNaN(q) || double.IsInfinity(q)) {
                q = NadirPoint[n];
              }
              q -= IdealPoint[n];

              if (NormalizeObjectives) {
                q /= NadirPoint[n] - IdealPoint[n];
              }

              var l = lambda[n].IsAlmost(0) ? 1e-4 : lambda[n];
              var feval = l * Math.Abs(q);

              if (feval > maxFun) {
                maxFun = feval;
              }
            }

            return maxFun;
          }
        case FunctionType.AGG: {
            double sum = 0.0;
            for (int n = 0; n < dim; n++) {
              sum += lambda[n] * qualities[n];
            }
            return sum;
          }
        case FunctionType.PBI: {
            double d1, d2, nl;
            double theta = 5.0;
            int dimensions = dim;

            d1 = d2 = nl = 0.0;

            for (int i = 0; i < dimensions; i++) {
              d1 += (qualities[i] - IdealPoint[i]) * lambda[i];
              nl += Math.Pow(lambda[i], 2.0);
            }
            nl = Math.Sqrt(nl);
            d1 = Math.Abs(d1) / nl;

            for (int i = 0; i < dimensions; i++) {
              d2 += Math.Pow((qualities[i] - IdealPoint[i]) - d1 * (lambda[i] / nl), 2.0);
            }
            d2 = Math.Sqrt(d2);
            return d1 + theta * d2;
          }
        default: {
            throw new ArgumentException($"Unknown function type: {functionType}");
          }
      }
    }

    public IList<IMOEADSolution> GetResult() {
      if (PopulationSize > ResultPopulationSize) {
        return MOEADUtil.GetSubsetOfEvenlyDistributedSolutions(Random, population, ResultPopulationSize);
      } else {
        return population;
      }
    }

    protected void UpdateParetoFronts(bool[] maximization) {
      var qualities = population.Select(x => x.Qualities).ToArray();
      var pf = DominationCalculator<IMOEADSolution>.CalculateBestParetoFront(population, qualities, maximization);

      var n = (int)EnumerableExtensions.BinomialCoefficient(IdealPoint.Length, 2);
      var hypervolumes = new DoubleMatrix(n == 1 ? 1 : n + 1, 2) { ColumnNames = new[] { "PF hypervolume", "PF size" } };
      hypervolumes[0, 0] = Hypervolume.Calculate(pf.Select(x => x.Item2), NadirPoint, maximization);
      hypervolumes[0, 1] = pf.Count;
      var elementNames = new List<string>() { "Pareto Front" };

      ResultCollection results;
      if (Results.ContainsKey("Hypervolume Analysis")) {
        results = (ResultCollection)Results["Hypervolume Analysis"].Value;
      } else {
        results = new ResultCollection();
        Results.AddOrUpdateResult("Hypervolume Analysis", results);
      }

      ScatterPlot sp;
      if (IdealPoint.Length == 2) {
        var points = pf.Select(x => new Point2D<double>(x.Item2[0], x.Item2[1]));
        var r = OnlinePearsonsRCalculator.Calculate(points.Select(x => x.X), points.Select(x => x.Y), out OnlineCalculatorError error);
        if (error != OnlineCalculatorError.None) { r = double.NaN; }
        var resultName = "Pareto Front Analysis ";
        if (!results.ContainsKey(resultName)) {
          sp = new ScatterPlot();
          sp.Rows.Add(new ScatterPlotDataRow(resultName, "", points) { VisualProperties = { PointSize = 8 } });
          results.AddOrUpdateResult(resultName, sp);
        } else {
          sp = (ScatterPlot)results[resultName].Value;
          sp.Rows[resultName].Points.Replace(points);
        }
        sp.Name = $"Dimensions [0, 1], correlation: {r.ToString("N2")}";
      } else if (IdealPoint.Length > 2) {
        var indices = Enumerable.Range(0, IdealPoint.Length).ToArray();
        var visualProperties = new ScatterPlotDataRowVisualProperties { PointSize = 8, Color = Color.LightGray };
        var combinations = indices.Combinations(2).ToArray();
        var maximization2d = new[] { false, false };
        var solutions2d = pf.Select(x => x.Item1).ToArray();
        for (int i = 0; i < combinations.Length; ++i) {
          var c = combinations[i].ToArray();

          // calculate the hypervolume in the 2d coordinate space
          var reference2d = new[] { NadirPoint[c[0]], NadirPoint[c[1]] };
          var qualities2d = pf.Select(x => new[] { x.Item2[c[0]], x.Item2[c[1]] }).ToArray();
          var pf2d = DominationCalculator<IMOEADSolution>.CalculateBestParetoFront(solutions2d, qualities2d, maximization2d);

          hypervolumes[i + 1, 0] = pf2d.Count > 0 ? Hypervolume.Calculate(pf2d.Select(x => x.Item2), reference2d, maximization2d) : 0d;
          hypervolumes[i + 1, 1] = pf2d.Count;

          var resultName = $"Pareto Front Analysis [{c[0]}, {c[1]}]";
          elementNames.Add(resultName);

          var points = pf.Select(x => new Point2D<double>(x.Item2[c[0]], x.Item2[c[1]]));
          var pf2dPoints = pf2d.Select(x => new Point2D<double>(x.Item2[0], x.Item2[1]));

          if (!results.ContainsKey(resultName)) {
            sp = new ScatterPlot();
            sp.Rows.Add(new ScatterPlotDataRow("Pareto Front", "", points) { VisualProperties = visualProperties });
            sp.Rows.Add(new ScatterPlotDataRow($"Pareto Front [{c[0]}, {c[1]}]", "", pf2dPoints) { VisualProperties = { PointSize = 10, Color = Color.OrangeRed } });
            results.AddOrUpdateResult(resultName, sp);
          } else {
            sp = (ScatterPlot)results[resultName].Value;
            sp.Rows["Pareto Front"].Points.Replace(points);
            sp.Rows[$"Pareto Front [{c[0]}, {c[1]}]"].Points.Replace(pf2dPoints);
          }
          var r = OnlinePearsonsRCalculator.Calculate(points.Select(x => x.X), points.Select(x => x.Y), out OnlineCalculatorError error);
          var r2 = r * r;
          sp.Name = $"Pareto Front [{c[0]}, {c[1]}], correlation: {r2.ToString("N2")}";
        }
      }
      hypervolumes.RowNames = elementNames;
      results.AddOrUpdateResult("Hypervolumes", hypervolumes);
    }

    #region operator wiring and events
    protected void ExecuteOperation(CancellationToken cancellationToken, IOperation operation) {
      Stack<IOperation> executionStack = new Stack<IOperation>();
      executionStack.Push(operation);
      while (executionStack.Count > 0) {
        cancellationToken.ThrowIfCancellationRequested();
        IOperation next = executionStack.Pop();
        if (next is OperationCollection) {
          OperationCollection coll = (OperationCollection)next;
          for (int i = coll.Count - 1; i >= 0; i--)
            if (coll[i] != null) executionStack.Push(coll[i]);
        } else if (next is IAtomicOperation) {
          IAtomicOperation op = (IAtomicOperation)next;
          next = op.Operator.Execute((IExecutionContext)op, cancellationToken);
          if (next != null) executionStack.Push(next);
        }
      }
    }

    private void UpdateAnalyzers() {
      Analyzer.Operators.Clear();
      if (Problem != null) {
        foreach (IAnalyzer analyzer in Problem.Operators.OfType<IAnalyzer>()) {
          foreach (IScopeTreeLookupParameter param in analyzer.Parameters.OfType<IScopeTreeLookupParameter>())
            param.Depth = 1;
          Analyzer.Operators.Add(analyzer, analyzer.EnabledByDefault);
        }
      }
    }

    private void UpdateCrossovers() {
      ICrossover oldCrossover = CrossoverParameter.Value;
      CrossoverParameter.ValidValues.Clear();
      ICrossover defaultCrossover = Problem.Operators.OfType<ICrossover>().FirstOrDefault();

      foreach (ICrossover crossover in Problem.Operators.OfType<ICrossover>().OrderBy(x => x.Name))
        CrossoverParameter.ValidValues.Add(crossover);

      if (oldCrossover != null) {
        ICrossover crossover = CrossoverParameter.ValidValues.FirstOrDefault(x => x.GetType() == oldCrossover.GetType());
        if (crossover != null) CrossoverParameter.Value = crossover;
        else oldCrossover = null;
      }
      if (oldCrossover == null && defaultCrossover != null)
        CrossoverParameter.Value = defaultCrossover;
    }

    private void UpdateMutators() {
      IManipulator oldMutator = MutatorParameter.Value;
      MutatorParameter.ValidValues.Clear();
      IManipulator defaultMutator = Problem.Operators.OfType<IManipulator>().FirstOrDefault();

      foreach (IManipulator mutator in Problem.Operators.OfType<IManipulator>().OrderBy(x => x.Name))
        MutatorParameter.ValidValues.Add(mutator);

      if (oldMutator != null) {
        IManipulator mutator = MutatorParameter.ValidValues.FirstOrDefault(x => x.GetType() == oldMutator.GetType());
        if (mutator != null) MutatorParameter.Value = mutator;
        else oldMutator = null;
      }

      if (oldMutator == null && defaultMutator != null)
        MutatorParameter.Value = defaultMutator;
    }

    protected override void OnProblemChanged() {
      UpdateCrossovers();
      UpdateMutators();
      UpdateAnalyzers();
      base.OnProblemChanged();
    }

    protected override void OnExecutionStateChanged() {
      previousExecutionState = executionState;
      executionState = ExecutionState;
      base.OnExecutionStateChanged();
    }

    public void ClearState() {
      solutions = null;
      population = null;
      offspringPopulation = null;
      jointPopulation = null;
      lambda = null;
      neighbourhood = null;
      if (executionContext != null && executionContext.Scope != null) {
        executionContext.Scope.SubScopes.Clear();
      }
    }

    protected override void OnStopped() {
      ClearState();
      base.OnStopped();
    }
    #endregion
  }
}
