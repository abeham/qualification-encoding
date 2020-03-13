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

namespace HeuristicLab.Algorithms.MOEAD {
  [StorableType("DFE1AC99-9AEF-4A35-884B-C82E2B3D84F7")]
  public interface IMOEADSolution : IItem {
    IItem Individual { get; set; }
    double[] Qualities { get; set; }
    double[] Constraints { get; set; }
    int Dimensions { get; }
  }

  [Item("MOEADSolution", "Represents a solution inside the MOEA/D population")]
  [StorableType("8D7DE459-E6D5-4B74-9135-00E27CF451D3")]
  public class MOEADSolution : Item, IMOEADSolution {
    [Storable]
    public IItem Individual { get; set; }

    [Storable]
    public double[] Qualities { get; set; }

    [Storable]
    public double[] Constraints { get; set; }

    public MOEADSolution(int nObjectives, int nConstraints) {
      Qualities = new double[nObjectives];
      Constraints = new double[nConstraints];
    }

    public MOEADSolution(IItem individual, int nObjectives, int nConstraints) : this(nObjectives, nConstraints) {
      Individual = individual;
    }

    public MOEADSolution(IItem individual, double[] qualities, double[] constraints) {
      Individual = individual;
      Qualities = qualities;
      Constraints = constraints;
    }

    public MOEADSolution(double[] qualities) {
      Qualities = (double[])qualities.Clone();
    }

    public int Dimensions => Qualities == null ? 0 : Qualities.Length;

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MOEADSolution(this, cloner);
    }

    protected MOEADSolution(MOEADSolution original, Cloner cloner) : base(original, cloner) {
      Qualities = (double[])original.Qualities.Clone();
      Constraints = (double[])original.Qualities.Clone();
      Individual = (IItem)original.Individual.Clone(cloner);
    }

    [StorableConstructor]
    protected MOEADSolution(StorableConstructorFlag deserializing) : base(deserializing) { }
  }

  [Item("MOEADSolution", "Represents a solution inside the MOEA/D population")]
  [StorableType("24B4B79B-5828-4E6B-BE35-452DEA1FF538")]
  public class MOEADSolution<T> : MOEADSolution where T : class, IItem {
    public new T Individual {
      get { return (T)base.Individual; }
      set { base.Individual = value; }
    }

    public MOEADSolution(T individual, int nObjectives, int nConstraints) : base(individual, nObjectives, nConstraints) { }

    protected MOEADSolution(MOEADSolution<T> original, Cloner cloner) : base(original, cloner) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MOEADSolution<T>(this, cloner);
    }

    [StorableConstructor]
    protected MOEADSolution(StorableConstructorFlag deserializing) : base(deserializing) { }
  }
}
