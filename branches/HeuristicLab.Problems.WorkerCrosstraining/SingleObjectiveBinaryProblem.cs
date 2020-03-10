using System;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.BinaryVectorEncoding;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.Binary;

namespace SimpleModel {
  public enum Objective { Leadtime = 0, Tardiness = 3, Totalcost = 4 }

  [Item("OptimalWorkforce Single-Objective Simple Model (Binary)", "")]
  [StorableClass]
  [Creatable(CreatableAttribute.Categories.Problems)]
  public class SingleObjectiveBinaryProblem : BinaryProblem {
    public override bool Maximization => false;

    #region Paramter Properties
    [Storable]
    private FixedValueParameter<IntValue> _repetitionsParameter;
    public IFixedValueParameter<IntValue> repetitionsParameter {
      get { return _repetitionsParameter; }
    }

    [Storable]
    private FixedValueParameter<IntValue> _workersParameter;
    public IFixedValueParameter<IntValue> WorkersParameter {
      get { return _workersParameter; }
    }

    [Storable]
    private ValueParameter<DoubleArray> _processingTimesParameter;
    public IValueParameter<DoubleArray> ProcessingTimesParameter {
      get { return _processingTimesParameter; }
    }

    [Storable]
    private ValueParameter<IntArray> _machinesParameter;
    public IValueParameter<IntArray> MachinesParameter {
      get { return _machinesParameter; }
    }

    [Storable]
    private FixedValueParameter<DoubleValue> _demandIntervalParameter;
    public IFixedValueParameter<DoubleValue> demandIntervalParameter {
      get { return _demandIntervalParameter; }
    }

    [Storable]
    private FixedValueParameter<DoubleValue> _customerRequiredLeadtimeParameter;
    public IFixedValueParameter<DoubleValue> customerRequiredLeadtimeParameter {
      get { return _customerRequiredLeadtimeParameter; }
    }

    [Storable]
    private FixedValueParameter<EnumValue<DispatchStrategy>> _dispatchStrategyParameter;
    public IFixedValueParameter<EnumValue<DispatchStrategy>> dispatchStrategyParameter {
      get { return _dispatchStrategyParameter; }
    }

    [Storable]
    private FixedValueParameter<EnumValue<Objective>> _objectiveParameter;
    public IFixedValueParameter<EnumValue<Objective>> objectiveParameter {
      get { return _objectiveParameter; }
    }

    [Storable]
    private FixedValueParameter<DoubleValue> _costWipInventoryParameter;
    public IFixedValueParameter<DoubleValue> costWipInventoryParameter {
      get { return _costWipInventoryParameter; }
    }

    [Storable]
    private FixedValueParameter<DoubleValue> _costFgiInventoryParameter;
    public IFixedValueParameter<DoubleValue> costFgiInventoryParameter {
      get { return _costFgiInventoryParameter; }
    }

    [Storable]
    private FixedValueParameter<DoubleValue> _costTardinessParameter;
    public IFixedValueParameter<DoubleValue> costTardinessParameter {
      get { return _costTardinessParameter; }
    }
    #endregion

    #region Properties
    public int Repetitions {
      get { return _repetitionsParameter.Value.Value; }
      set { _repetitionsParameter.Value.Value = value; }
    }
    public int Workers {
      get { return _workersParameter.Value.Value; }
      set { _workersParameter.Value.Value = value; }
    }
    public DoubleArray ProcessingTimes {
      get { return _processingTimesParameter.Value; }
      set { _processingTimesParameter.Value = value; }
    }
    public IntArray Machines {
      get { return _machinesParameter.Value; }
      set { _machinesParameter.Value = value; }
    }
    public double DemandInterval {
      get { return _demandIntervalParameter.Value.Value; }
      set { _demandIntervalParameter.Value.Value = value; }
    }
    public double CustomerRequiredLeadtime {
      get { return _customerRequiredLeadtimeParameter.Value.Value; }
      set { _customerRequiredLeadtimeParameter.Value.Value = value; }
    }
    public DispatchStrategy DispatchStrategy {
      get { return _dispatchStrategyParameter.Value.Value; }
      set { _dispatchStrategyParameter.Value.Value = value; }
    }
    public Objective Objective {
      get { return _objectiveParameter.Value.Value; }
      set { _objectiveParameter.Value.Value = value; }
    }

    public double CostWipInventory {
      get { return _costWipInventoryParameter.Value.Value; }
      set { _costWipInventoryParameter.Value.Value = value; }
    }
    public double CostFgiInventory {
      get { return _costFgiInventoryParameter.Value.Value; }
      set { _costFgiInventoryParameter.Value.Value = value; }
    }
    public double CostTardiness {
      get { return _costTardinessParameter.Value.Value; }
      set { _costTardinessParameter.Value.Value = value; }
    }
    #endregion

    [StorableConstructor]
    protected SingleObjectiveBinaryProblem(bool deserializing) : base(deserializing) { }
    protected SingleObjectiveBinaryProblem(SingleObjectiveBinaryProblem original, Cloner cloner)
    : base(original, cloner) {
      _repetitionsParameter = cloner.Clone(original._repetitionsParameter);
      _workersParameter = cloner.Clone(original._workersParameter);
      _processingTimesParameter = cloner.Clone(original._processingTimesParameter);
      _machinesParameter = cloner.Clone(original._machinesParameter);
      _demandIntervalParameter = cloner.Clone(original._demandIntervalParameter);
      _customerRequiredLeadtimeParameter = cloner.Clone(original._customerRequiredLeadtimeParameter);
      _dispatchStrategyParameter = cloner.Clone(original._dispatchStrategyParameter);
      _objectiveParameter = cloner.Clone(original._objectiveParameter);

      _costWipInventoryParameter = cloner.Clone(original._costWipInventoryParameter);
      _costFgiInventoryParameter = cloner.Clone(original._costFgiInventoryParameter);
      _costTardinessParameter = cloner.Clone(original._costTardinessParameter);

      RegisterEvents();
    }
    public SingleObjectiveBinaryProblem() {
      Parameters.Add(_repetitionsParameter = new FixedValueParameter<IntValue>("Repetitions", "The number of repetitions to perform.", new IntValue(10)));
      Parameters.Add(_workersParameter = new FixedValueParameter<IntValue>("Workers", "The number of workers.", new IntValue(5)));
      Parameters.Add(_processingTimesParameter = new ValueParameter<DoubleArray>("Processing Times", "The processing time per task.", new DoubleArray(new[] { 10.0, 5.0, 20.0 })));
      Parameters.Add(_machinesParameter = new ValueParameter<IntArray>("Machines", "The number of machines per level.", new IntArray(new[] { 2, 1, 3 })));
      Parameters.Add(_demandIntervalParameter = new FixedValueParameter<DoubleValue>("Demand Interval", "The mean of the exponentially distributed interarrival times.", new DoubleValue(7.0)));
      Parameters.Add(_customerRequiredLeadtimeParameter = new FixedValueParameter<DoubleValue>("Customer Required Leadtime", "The customer required lead time.", new DoubleValue(37.0)));
      Parameters.Add(_dispatchStrategyParameter = new FixedValueParameter<EnumValue<DispatchStrategy>>("Dispatch Strategy", "The dispatching strategy.", new EnumValue<DispatchStrategy>(DispatchStrategy.LeastFlexibleFirst)));
      Parameters.Add(_objectiveParameter = new FixedValueParameter<EnumValue<Objective>>("Objective", "The objective that should be minimized.", new EnumValue<Objective>(Objective.Leadtime)));

      Parameters.Add(_costWipInventoryParameter = new FixedValueParameter<DoubleValue>("Cost Wip Inventory", "The cost of work-in-process inventory.", new DoubleValue(0.5)));
      Parameters.Add(_costFgiInventoryParameter = new FixedValueParameter<DoubleValue>("Cost Fgi Inventory", "The cost of finished goods inventory.", new DoubleValue(1.0)));
      Parameters.Add(_costTardinessParameter = new FixedValueParameter<DoubleValue>("Cost Tardiness", "The cost factor for tardiness.", new DoubleValue(19.0)));

      Encoding.Length = Workers * ProcessingTimes.Length;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SingleObjectiveBinaryProblem(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEvents();
    }

    private void RegisterEvents() {
      _workersParameter.Value.ValueChanged += WorkersOnValueChanged;
      _processingTimesParameter.ValueChanged += ProcessingTimesOnValueChanged;
      _processingTimesParameter.Value.Reset += ProcessingTimesOnReset;
      _machinesParameter.ValueChanged += MachinesOnValueChanged;
      _machinesParameter.Value.Reset += MachinesOnReset;
    }
    private void WorkersOnValueChanged(object sender, EventArgs e) {
      Encoding.Length = Workers * ProcessingTimes.Length;
    }

    private void ProcessingTimesOnValueChanged(object sender, EventArgs e) {
      _processingTimesParameter.Value.Reset += ProcessingTimesOnReset;
      Encoding.Length = Workers * ProcessingTimes.Length;
      if (Machines.Length != ProcessingTimes.Length) Machines.Length = ProcessingTimes.Length;
    }

    private void ProcessingTimesOnReset(object sender, EventArgs e) {
      Encoding.Length = Workers * ProcessingTimes.Length;
      if (Machines.Length != ProcessingTimes.Length) Machines.Length = ProcessingTimes.Length;
    }

    private void MachinesOnValueChanged(object sender, EventArgs e) {
      _machinesParameter.Value.Reset += MachinesOnReset;
      if (Machines.Length != ProcessingTimes.Length)
        ProcessingTimes.Length = Machines.Length;
    }

    private void MachinesOnReset(object sender, EventArgs e) {
      if (Machines.Length != ProcessingTimes.Length)
        ProcessingTimes.Length = Machines.Length;
    }

    public override double Evaluate(BinaryVector vector, IRandom random) {
      var matrix = BinaryQualification.FromVector(vector, ProcessingTimes.Length);
      var avg = 0.0;
      for (var r = 0; r < Repetitions; r++) {
        var sim = new SimulationModel(ProcessingTimes.ToArray(), Machines.ToArray(), DemandInterval, CustomerRequiredLeadtime, matrix, DispatchStrategy, r);
        sim.Run();

        switch (Objective) {
          case Objective.Leadtime: avg += sim.LeadTime.Mean; break;
          case Objective.Tardiness: avg += sim.Tardiness.Mean; break;
          case Objective.Totalcost:
            avg += CostWipInventory * sim.WorkInProcess.Mean
              + CostFgiInventory * sim.FinishedGoodsInventory.Mean
              + CostTardiness * sim.Tardiness.Mean;
            break;
        }
      }
      return avg / Repetitions;
    }
  }
}
