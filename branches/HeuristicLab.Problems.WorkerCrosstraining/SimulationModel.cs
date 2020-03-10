using System;
using System.Collections.Generic;
using System.Linq;
using SimSharp;
using Environment = SimSharp.Environment;

namespace SimpleModel {
  public enum DispatchStrategy { FirstComeFirstServe, LeastFlexibleFirst }

  public class SimulationModel {
    private double[] _ptimes;
    private int[] _machines;
    private double _demandTime;
    private double _customerLeadTime;
    private IQualification _qual;
    private DispatchStrategy _strategy;

    private Random _rand;
    private Environment _env;
    private ResourcePool _pool;
    private Resource[] _seq;
    private List<List<int>> _workersByTask, _tasksByWorker;

    public TimeBasedStatistics[] Backlog { get; }
    public TimeBasedStatistics WorkInProcess { get; }
    public TimeBasedStatistics FinishedGoodsInventory { get; }
    public BasicStatistics LeadTime { get; }
    public BasicStatistics Tardiness { get; }

    public SimulationModel(double[] ptimes, int[] machines, double demandTime, double customerLeadTime, IQualification qual, DispatchStrategy strategy, int rseed = 0) {
      _rand = new Random(rseed);
      _ptimes = ptimes;
      _machines = machines;
      _demandTime = demandTime;
      _customerLeadTime = customerLeadTime;
      _qual = qual;
      _strategy = strategy;

      _env = new Environment(rseed);
      _pool = new ResourcePool(_env, Enumerable.Range(0, qual.Workers).Cast<object>());
      _workersByTask = qual.GetWorkersByTask().ToList();
      _tasksByWorker = qual.GetTasksByWorker().ToList();
      _seq = machines.Select(m => new Resource(_env, Math.Max(m, 1))).ToArray();

      Backlog = Enumerable.Range(0, qual.Tasks).Select(t => new TimeBasedStatistics(_env)).ToArray();
      WorkInProcess = new TimeBasedStatistics(_env);
      FinishedGoodsInventory = new TimeBasedStatistics(_env);
      LeadTime = new BasicStatistics();
      Tardiness = new BasicStatistics();
    }

    public void Run() {
      _env.Process(Demand());
      _env.Run();
    }

    private IEnumerable<Event> Demand() {
      for (var i = 0; i < 1000; i++) {
        _env.Process(Job());
        yield return _env.TimeoutExponentialD(_demandTime);
      }
    }

    private IEnumerable<Event> Job() {
      var start = _env.NowD;
      var due = start + _env.RandExponential(_customerLeadTime);
      yield return _env.Process(JobFlow());
      LeadTime.Add(_env.NowD - start);
      Tardiness.Add(Math.Max(_env.NowD - due, 0));
      if (_env.NowD < due) {
        FinishedGoodsInventory.Increase();
        yield return _env.TimeoutD(due - _env.NowD);
        FinishedGoodsInventory.Decrease();
      }
    }

    private IEnumerable<Event> JobFlow() {
      for (var t = 0; t < _qual.Tasks; t++) {
        Backlog[t].Increase();
        using (var s = _seq[t].Request()) {
          yield return s;
          Backlog[t].Decrease();
          if (t == 0) {
            WorkInProcess.Increase();
          }
          if (_workersByTask[t].Count == 0) yield return _env.TimeoutExponentialD(_ptimes[t] * 1000);
          else {
            var req = GetWorker(t);            
            yield return req;
            var worker = (int)req.Value;
            yield return _env.TimeoutExponentialD(_ptimes[t] / _qual.GetQualificationLevel(worker, t));
            yield return _pool.Release(req);
          }
        }
      }
      WorkInProcess.Decrease();
    }

    private ResourcePoolRequest GetWorker(int task) {
      switch (_strategy) {
        case DispatchStrategy.FirstComeFirstServe:
          return _pool.Request(w => _qual.IsQualified((int)w, task));
        case DispatchStrategy.LeastFlexibleFirst:
          var avail = _workersByTask[task].Where(x => _pool.IsAvailable(w => (int)w == x)).ToList();
          
          if (avail.Count == 0) {
            return _pool.Request(w => _qual.IsQualified((int)w, task));
          } else {
            var chosen = avail.MinItems(w => _tasksByWorker[w].Count).SampleRandom(_rand);
            return _pool.Request(w => (int)w == chosen);
          }
        default: throw new NotImplementedException($"Strategy {_strategy} not implemented.");
      }
    }
  }
}
