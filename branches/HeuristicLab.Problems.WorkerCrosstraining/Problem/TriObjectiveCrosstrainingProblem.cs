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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Analysis;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.BinaryVectorEncoding;
using HeuristicLab.Encodings.QualificationEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;

namespace HeuristicLab.Problems.WorkerCrosstraining {
  [StorableType("09A63672-360B-4F5C-B43B-D5B296CCB9E1")]
  [Item("Tri-objective Worker Cross Training Problem", "")]
  [Creatable(CreatableAttribute.Categories.Problems)]
  public sealed class TriObjectiveCrosstrainingProblem : MultiObjectiveBasicProblem<QualificationEncoding> {
    public override bool[] Maximization => new[] { false, false, false };

    #region Parameter Names
    private const string RepetitionsParameterName = "Repetitions";
    private const string QualificationsParameterName = "Qualifications";
    private const string WorkersParameterName = "Workers";
    private const string UtilizationParameterName = "Utilization";
    private const string OrderAmountParameterName = "Order Amount Factor (OAM)";
    private const string PersonellRatioParameterName = "Personell Ratio (PR)";
    private const string ChangeTimeRatioParameterName = "Change Time Ratio (CR)";
    private const string LineChangeFactorParameterName = "Line Change Factor (LCR)";
    private const string DispatchStrategyParameterName = "Dispatch Strategy";
    private const string ObjectiveParameterName = "Objective";
    private const string CostWipInventoryParameterName = "Cost WIP Inventory";
    private const string CostFgiInventoryParameterName = "Cost FGI Inventory";
    private const string CostTardinessParameterName = "Cost Tardiness";
    private const string ObservationTimeParameterName = "Observation Time";
    private const string WarmupTimeParameterName = "Warmup Time";
    #endregion

    #region Parameters
    [Storable]
    private IFixedValueParameter<IntValue> repetitionsParameter;
    public IFixedValueParameter<IntValue> RepetitionsParameter => repetitionsParameter;

    [Storable]
    private IFixedValueParameter<IntValue> qualificationsParameter;
    public IFixedValueParameter<IntValue> QualificationsParameter => qualificationsParameter;

    [Storable]
    private IFixedValueParameter<IntValue> workersParameter;
    public IFixedValueParameter<IntValue> WorkersParameter => workersParameter;

    [Storable]
    private IFixedValueParameter<PercentValue> utilizationParameter;
    public IFixedValueParameter<PercentValue> UtilizationParameter => utilizationParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> orderAmountParameter;
    public IFixedValueParameter<DoubleValue> OrderAmountParameter => orderAmountParameter;

    [Storable]
    private IFixedValueParameter<PercentValue> personellRatioParameter;
    public IFixedValueParameter<PercentValue> PersonellRatioParameter => personellRatioParameter;

    [Storable]
    private IFixedValueParameter<PercentValue> changeTimeRatioParameter;
    public IFixedValueParameter<PercentValue> ChangeTimeRatioParameter => changeTimeRatioParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> lineChangeFactorParameter;
    public IFixedValueParameter<DoubleValue> LineChangeFactorParameter => lineChangeFactorParameter;

    [Storable]
    private IFixedValueParameter<EnumValue<DispatchStrategy>> dispatchStrategyParameter;
    public IFixedValueParameter<EnumValue<DispatchStrategy>> DispatchStrategyParameter => dispatchStrategyParameter;

    [Storable]
    private IFixedValueParameter<EnumValue<Objective>> objectiveParameter;
    public IFixedValueParameter<EnumValue<Objective>> ObjectiveParameter => objectiveParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> costWipInventoryParameter;
    public IFixedValueParameter<DoubleValue> CostWipInventoryParameter => costWipInventoryParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> costFgiInventoryParameter;
    public IFixedValueParameter<DoubleValue> CostFgiInventoryParameter => costFgiInventoryParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> costTardinessParameter;
    public IFixedValueParameter<DoubleValue> CostTardinessParameter => costTardinessParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> observationTimeParameter;
    public IFixedValueParameter<DoubleValue> ObservationTimeParameter => observationTimeParameter;

    [Storable]
    private IFixedValueParameter<DoubleValue> warmupTimeParameter;
    public IFixedValueParameter<DoubleValue> WarmupTimeParameter => warmupTimeParameter;
    #endregion

    #region Properties
    public int Repetitions {
      get => repetitionsParameter.Value.Value;
      set => repetitionsParameter.Value.Value = value;
    }
    public int Qualifications {
      get => qualificationsParameter.Value.Value;
      set => qualificationsParameter.Value.Value = value;
    }
    public int Workers {
      get => workersParameter.Value.Value;
      set => workersParameter.Value.Value = value;
    }
    public double Utilization {
      get => utilizationParameter.Value.Value;
      set => utilizationParameter.Value.Value = value;
    }
    public double OrderAmount {
      get => orderAmountParameter.Value.Value;
      set => orderAmountParameter.Value.Value = value;
    }
    public double PersonellRatio {
      get => personellRatioParameter.Value.Value;
      set => personellRatioParameter.Value.Value = value;
    }
    public double ChangeTimeRatio {
      get => changeTimeRatioParameter.Value.Value;
      set => changeTimeRatioParameter.Value.Value = value;
    }
    public double LineChangeFactor {
      get => lineChangeFactorParameter.Value.Value;
      set => lineChangeFactorParameter.Value.Value = value;
    }
    public DispatchStrategy DispatchStrategy {
      get => dispatchStrategyParameter.Value.Value;
      set => dispatchStrategyParameter.Value.Value = value;
    }
    public Objective Objective {
      get => objectiveParameter.Value.Value;
      set => objectiveParameter.Value.Value = value;
    }
    public double CostWipInventory {
      get => costWipInventoryParameter.Value.Value;
      set => costWipInventoryParameter.Value.Value = value;
    }
    public double CostFgiInventory {
      get => costFgiInventoryParameter.Value.Value;
      set => costFgiInventoryParameter.Value.Value = value;
    }
    public double CostTardiness {
      get => costTardinessParameter.Value.Value;
      set => costTardinessParameter.Value.Value = value;
    }
    public double ObservationTime {
      get => observationTimeParameter.Value.Value;
      set => observationTimeParameter.Value.Value = value;
    }
    public double WarmupTime {
      get => warmupTimeParameter.Value.Value;
      set => warmupTimeParameter.Value.Value = value;
    }
    #endregion

    [StorableConstructor]
    private TriObjectiveCrosstrainingProblem(StorableConstructorFlag _) : base(_) { }
    private TriObjectiveCrosstrainingProblem(TriObjectiveCrosstrainingProblem original, Cloner cloner)
      : base(original, cloner) {
      repetitionsParameter = cloner.Clone(original.repetitionsParameter);
      qualificationsParameter = cloner.Clone(original.qualificationsParameter);
      workersParameter = cloner.Clone(original.workersParameter);
      utilizationParameter = cloner.Clone(original.utilizationParameter);
      orderAmountParameter = cloner.Clone(original.orderAmountParameter);
      personellRatioParameter = cloner.Clone(original.personellRatioParameter);
      changeTimeRatioParameter = cloner.Clone(original.changeTimeRatioParameter);
      lineChangeFactorParameter = cloner.Clone(original.lineChangeFactorParameter);
      dispatchStrategyParameter = cloner.Clone(original.dispatchStrategyParameter);
      objectiveParameter = cloner.Clone(original.objectiveParameter);

      costWipInventoryParameter = cloner.Clone(original.costWipInventoryParameter);
      costFgiInventoryParameter = cloner.Clone(original.costFgiInventoryParameter);
      costTardinessParameter = cloner.Clone(original.costTardinessParameter);
      observationTimeParameter = cloner.Clone(original.observationTimeParameter);
      warmupTimeParameter = cloner.Clone(original.warmupTimeParameter);

      RegisterEvents();
    }
    public TriObjectiveCrosstrainingProblem() : base() {
      Parameters.Add(repetitionsParameter = new FixedValueParameter<IntValue>(RepetitionsParameterName, "The number of repetitions to perform.", new IntValue(20)));
      Parameters.Add(qualificationsParameter = new FixedValueParameter<IntValue>(QualificationsParameterName, "The number of qualifications.", new IntValue(6)));
      Parameters.Add(workersParameter = new FixedValueParameter<IntValue>(WorkersParameterName, "The number of workers.", new IntValue(47)));
      Parameters.Add(utilizationParameter = new FixedValueParameter<PercentValue>(UtilizationParameterName, "Target system utilization level", new PercentValue(0.95)));
      Parameters.Add(orderAmountParameter = new FixedValueParameter<DoubleValue>(OrderAmountParameterName, "Order size, a lot of small orders or fewer bigger ones", new DoubleValue(5.0)));
      Parameters.Add(personellRatioParameter = new FixedValueParameter<PercentValue>(PersonellRatioParameterName, "Ratio of the processing time that requires human workers.", new PercentValue(0.5)));
      Parameters.Add(changeTimeRatioParameter = new FixedValueParameter<PercentValue>(ChangeTimeRatioParameterName, "Ratio of the processing time that is required for changing workplaces.", new PercentValue(0.1)));
      Parameters.Add(lineChangeFactorParameter = new FixedValueParameter<DoubleValue>(LineChangeFactorParameterName, "Factor of the CR that is applied when changing between lines.", new DoubleValue(1.0)));
      Parameters.Add(dispatchStrategyParameter = new FixedValueParameter<EnumValue<DispatchStrategy>>(DispatchStrategyParameterName, "The dispatching strategy.", new EnumValue<DispatchStrategy>(DispatchStrategy.ModifiedLeastSkillFirst)));
      Parameters.Add(objectiveParameter = new FixedValueParameter<EnumValue<Objective>>(ObjectiveParameterName, "The objective that should be minimized.", new EnumValue<Objective>(Objective.Servicelevel)));

      Parameters.Add(costWipInventoryParameter = new FixedValueParameter<DoubleValue>(CostWipInventoryParameterName, "The cost of work-in-process inventory.", new DoubleValue(0.5)));
      Parameters.Add(costFgiInventoryParameter = new FixedValueParameter<DoubleValue>(CostFgiInventoryParameterName, "The cost of finished goods inventory.", new DoubleValue(1.0)));
      Parameters.Add(costTardinessParameter = new FixedValueParameter<DoubleValue>(CostTardinessParameterName, "The cost factor for tardiness.", new DoubleValue(19.0)));
      Parameters.Add(observationTimeParameter = new FixedValueParameter<DoubleValue>(ObservationTimeParameterName, "The maximum observed simulation time.", new DoubleValue(3600)));
      Parameters.Add(warmupTimeParameter = new FixedValueParameter<DoubleValue>(WarmupTimeParameterName, "The initial simulation time that does not count towards the performance.", new DoubleValue(600)));

      Encoding.Qualifications = QualificationsParameter.Value.Value;
      Encoding.Workers = WorkersParameter.Value.Value;

      RegisterEvents();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new TriObjectiveCrosstrainingProblem(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEvents();
    }

    private void RegisterEvents() {
      qualificationsParameter.Value.ValueChanged += Qualifications_ValueChanged;
      workersParameter.Value.ValueChanged += Workers_ValueChanged;
    }

    private void Qualifications_ValueChanged(object sender, EventArgs e) {
      Encoding.Qualifications = Qualifications;
    }

    private void Workers_ValueChanged(object sender, EventArgs e) {
      Encoding.Workers = Workers;
    }

    public override double[] Evaluate(Individual individual, IRandom random) {
      var encodedSol = individual.Qualification(Encoding.Name);
      var vector = encodedSol.ToBoolArray();
      var matrix = BinaryQualifiedWorkforce.FromVector(vector, Encoding.Qualifications);
      var avg = 0.0;
      for (var r = 0; r < Repetitions; r++) {
        var sim = new WSCModel(Utilization, OrderAmount, PersonellRatio, ChangeTimeRatio, LineChangeFactor,
          1, 100, 0, 0.25, 1.0,
          matrix, DispatchStrategy, r, ObservationTime, WarmupTime);
        sim.Run();

        switch (Objective) {
          case Objective.WIPLeadtime: avg += sim.WIPLeadTime.Mean; break;
          case Objective.Tardiness: avg += sim.Tardiness.Mean; break;
          case Objective.Totalcost:
            avg += CostWipInventory * sim.WIPInventory.Mean
              + CostFgiInventory * sim.FGIInventory.Mean
              + CostTardiness * sim.Tardiness.Mean;
            break;
          case Objective.Servicelevel:
            avg -= sim.ServiceLevel.Mean; break;
        }
      }
      return new[] { avg / Repetitions, vector.Count(x => x), encodedSol.Pools };
    }

    public override void Analyze(Individual[] individuals, double[][] qualities, ResultCollection results, IRandom random) {
      base.Analyze(individuals, qualities, results, random);

      int length = Encoding.Qualifications * Encoding.Workers;

      var sp = new ScatterPlot() { Name = "Solutions" };
      sp.VisualProperties.YAxisMaximumFixedValue = length;
      sp.VisualProperties.YAxisMaximumAuto = false;
      sp.VisualProperties.YAxisMinimumFixedValue = 0;
      sp.VisualProperties.YAxisMinimumAuto = false;

      var row = new ScatterPlotDataRow() { Name = "Objective vs Qualifications" };
      row.VisualProperties.PointSize = 5;
      var pareto = new ScatterPlotDataRow() { Name = "Pareto Front" };
      pareto.VisualProperties.PointSize = 7;

      var best = new double[length + 1];
      var bestVec = new BinaryVector[length + 1];
      for (var i = 0; i < qualities.Length; i++) {
        var vec = new BinaryVector(individuals[i].Qualification(Encoding.Name).ToBoolArray());

        row.Points.Add(new Point2D<double>(qualities[i][0], qualities[i][1], vec));
        var qual = (int)qualities[i][1];
        if (bestVec[qual] == null || best[qual] > qualities[i][0]) {
          bestVec[qual] = vec;
          best[qual] = qualities[i][0];
        }
      }

      var sorted = bestVec.Select((v, i) => new { Qual = i, Vec = v, Fit = best[i] }).Where(x => x.Vec != null).OrderBy(x => x.Fit);
      var minQual = -1;
      var minX = best.Min();
      var maxX = minX;
      foreach (var s in sorted) {
        if (minQual >= 0 && s.Qual >= minQual)
          continue;
        pareto.Points.Add(new Point2D<double>(s.Fit, s.Qual, s.Vec));
        minQual = s.Qual;
        if (s.Qual >= Workers && maxX < s.Fit) maxX = s.Fit;
      }

      sp.Rows.Add(row);
      sp.Rows.Add(pareto);

      sp.VisualProperties.XAxisMaximumFixedValue = maxX;
      sp.VisualProperties.XAxisMaximumAuto = false;
      sp.VisualProperties.XAxisMinimumFixedValue = minX;
      sp.VisualProperties.XAxisMinimumAuto = false;

      if (!results.ContainsKey("Scatterplot")) results.Add(new Result("Scatterplot", sp));
      else results["Scatterplot"].Value = sp;
    }
  }
}
