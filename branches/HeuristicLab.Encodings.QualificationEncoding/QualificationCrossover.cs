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

using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("0F0A1217-9972-4AD8-B19D-E8A7C1F6B608")]
  [Item("QualificationCrossover", "A base class for operators crossing qualifications.")]
  public abstract class QualificationCrossover : InstrumentedOperator, IQualificationCrossover, IStochasticOperator {
    #region Parameter Names
    private const string ParentsParameterName = "Parents";
    private const string ChildParameterName = "Child";
    private const string RandomParameterName = "Random";
    private const string EncodingParameterName = "Encoding";
    #endregion

    #region Parameters
    public ILookupParameter<ItemArray<Qualification>> ParentsParameter => (ILookupParameter<ItemArray<Qualification>>)Parameters[ParentsParameterName];
    public ILookupParameter<Qualification> ChildParameter => (ILookupParameter<Qualification>)Parameters[ChildParameterName];
    public ILookupParameter<IRandom> RandomParameter => (ILookupParameter<IRandom>)Parameters[RandomParameterName];
    public ILookupParameter<QualificationEncoding> EncodingParameter => (ILookupParameter<QualificationEncoding>)Parameters[EncodingParameterName];
    #endregion

    public override bool CanChangeName => false;

    [StorableConstructor]
    protected QualificationCrossover(StorableConstructorFlag _) : base(_) { }
    protected QualificationCrossover(QualificationCrossover original, Cloner cloner) : base(original, cloner) { }
    public QualificationCrossover() : base() {
      Parameters.Add(new ScopeTreeLookupParameter<Qualification>(ParentsParameterName, "The parent qualifications which should be crossed."));
      ParentsParameter.ActualName = "Qualification";
      Parameters.Add(new LookupParameter<Qualification>(ChildParameterName, "The child qualification resulting from the crossover."));
      ChildParameter.ActualName = "Qualification";
      Parameters.Add(new LookupParameter<IRandom>(RandomParameterName, "The pseudo random number generator which should be used for stochastic operators."));
      Parameters.Add(new LookupParameter<QualificationEncoding>(EncodingParameterName, "The encoding properties."));
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      if (!Parameters.ContainsKey(EncodingParameterName)) {
        Parameters.Add(new LookupParameter<QualificationEncoding>(EncodingParameterName, "The encoding properties."));
      }
    }

    public sealed override IOperation InstrumentedApply() {
      ChildParameter.ActualValue = Cross(EncodingParameter.ActualValue, ParentsParameter.ActualValue, RandomParameter.ActualValue);
      return base.InstrumentedApply();
    }

    protected abstract Qualification Cross(QualificationEncoding encoding, ItemArray<Qualification> parents, IRandom random);
  }
}
