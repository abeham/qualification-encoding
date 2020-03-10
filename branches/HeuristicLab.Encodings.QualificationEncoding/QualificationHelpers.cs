using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Core;
using HeuristicLab.Random;

namespace HeuristicLab.Encodings.QualificationEncoding {
  public static class QualificationHelpers {

    public static void Repair(QualificationEncoding encoding, Qualification solution, IRandom random) {
      // CONSTRAINT 1: Every qualification must be assigned to at least one worker
      var unset = new HashSet<int>(solution.Groups.First().Key.GetFalseIndices());
      foreach (var pool in solution.Groups.Skip(1)) {
        if (unset.Count == 0) break;
        unset.ExceptWith(pool.Key.GetTrueIndices());
      }

      foreach (var idx in unset) {
        // select one group randomly
        var group = solution.Groups.SampleRandom(random);
        solution.Groups.Remove(group.Key);
        var newPool = group.Key.GetBuilder().Set(idx).Build();
        solution.AddOrIncrease(newPool, group.Value);
      }

      // CONSTRAINT 2: Every worker must be assigned
      var unassignedWorkers = encoding.Workers - solution.NumberOfWorkers;
      while (unassignedWorkers > 0) {
        // too little workers are assigned
        var group = solution.Groups.SampleRandom(random);
        var assign = random.Next(unassignedWorkers) + 1;
        solution.Groups[group.Key] = group.Value + assign;
        unassignedWorkers -= assign;
      }
      if (unassignedWorkers < 0) {
        // too much workers are assigned
        while (solution.Groups.Count > encoding.Workers) {
          // and too much qualification pools exist
          MergeQualificationGroupManipulator.Merge(encoding, solution, random);
        }
        while (unassignedWorkers < 0) {
          var group = solution.Groups.Where(x => x.Value > 1).SampleRandom(random);
          var unassign = random.Next(Math.Min(-unassignedWorkers, group.Value - 1)) + 1;
          solution.Groups[group.Key] = group.Value - unassign;
          unassignedWorkers += unassign;
        }
      }

      if (!solution.Validate(encoding)) throw new InvalidOperationException("Solution is not valid after repair.");
      // TODO: CONSTRAINT 3: Maximum number of workers for a certain maximum number of qualifications
      // TODO: CONSTRAINT 4: There is a maximum number of workers that can be assigned to certain qualifications
      // TODO: CONSTRAINT 5: Specific combinations of qualifications are not allowed
    }
  }
}
