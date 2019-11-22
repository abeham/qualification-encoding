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
  [StorableType("A9880EB9-FD4C-41EF-BA70-C5A8BC28229F")]
  [Item("QualificationCreator", "A base class for operators creating qualifications.")]
  public abstract class QualificationCreator : InstrumentedOperator, IQualificationCreator, IStochasticOperator {
    #region Parameter Names
    private const string QualificationsParameterName = "Qualifications";
    private const string WorkersParameterName = "Workers";
    private const string QualificationParameterName = "Qualification";
    private const string RandomParameterName = "Random";
    private const string EncodingParameterName = "Encoding";
    #endregion

    #region Parameters
    public ILookupParameter<Qualification> QualificationParameter => (ILookupParameter<Qualification>)Parameters[QualificationParameterName];
    public ILookupParameter<IRandom> RandomParameter => (ILookupParameter<IRandom>)Parameters[RandomParameterName];
    public ILookupParameter<QualificationEncoding> EncodingParameter => (ILookupParameter<QualificationEncoding>)Parameters[EncodingParameterName];
    #endregion

    public override bool CanChangeName => false;

    [StorableConstructor]
    protected QualificationCreator(StorableConstructorFlag _) : base(_) { }
    protected QualificationCreator(QualificationCreator original, Cloner cloner) : base(original, cloner) { }
    public QualificationCreator() : base() {
      Parameters.Add(new LookupParameter<Qualification>(QualificationParameterName, "The qualification object which should be created."));
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
      QualificationParameter.ActualValue = Create(EncodingParameter.ActualValue, RandomParameter.ActualValue);
      return base.InstrumentedApply();
    }

    protected abstract Qualification Create(QualificationEncoding encoding, IRandom random);
  }
}
