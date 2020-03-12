﻿#region License Information
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

using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Optimization;
using HEAL.Attic;
using HeuristicLab.Problems.VehicleRouting.Interfaces;
using HeuristicLab.Problems.VehicleRouting.Variants;

namespace HeuristicLab.Problems.VehicleRouting.Encodings.Potvin {
  [Item("PotvinPDRearrangeExhaustiveMoveGenerator", "Generates rearrange moves from a given PDP encoding.")]
  [StorableType("D3975AC5-11B8-4AF4-8CB2-BE97A69C935A")]
  public sealed class PotvinPDRearrangeExhaustiveMoveGenerator : PotvinPDRearrangeMoveGenerator, IExhaustiveMoveGenerator {
    public override IDeepCloneable Clone(Cloner cloner) {
      return new PotvinPDRearrangeExhaustiveMoveGenerator(this, cloner);
    }

    [StorableConstructor]
    private PotvinPDRearrangeExhaustiveMoveGenerator(StorableConstructorFlag _) : base(_) { }

    public PotvinPDRearrangeExhaustiveMoveGenerator()
      : base() {
    }

    private PotvinPDRearrangeExhaustiveMoveGenerator(PotvinPDRearrangeExhaustiveMoveGenerator original, Cloner cloner)
      : base(original, cloner) {
    }

    protected override PotvinPDRearrangeMove[] GenerateMoves(PotvinEncoding individual, IVRPProblemInstance problemInstance) {
      List<PotvinPDRearrangeMove> result = new List<PotvinPDRearrangeMove>();
      IPickupAndDeliveryProblemInstance pdp = problemInstance as IPickupAndDeliveryProblemInstance;

      for (int i = 1; i <= problemInstance.Cities.Value; i++) {
        if (pdp == null || pdp.GetDemand(i) >= 0) {
          int tour = individual.Tours.FindIndex(t => t.Stops.Contains(i));

          result.Add(new PotvinPDRearrangeMove(i, tour, individual));
        }
      }

      return result.ToArray();
    }
  }
}
