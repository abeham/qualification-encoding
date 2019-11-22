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
  [StorableType("C056F0E9-CBBE-4469-B1A9-48046C3F7DE1")]
  [Item("QualificationManipulator", "A base class for operators manipulating qualifications.")]
  public abstract class QualificationManipulator : InstrumentedOperator, IQualificationManipulator, IStochasticOperator {
    #region Parameter Names
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
    protected QualificationManipulator(StorableConstructorFlag _) : base(_) { }
    protected QualificationManipulator(QualificationManipulator original, Cloner cloner) : base(original, cloner) { }
    public QualificationManipulator() : base() {
      Parameters.Add(new LookupParameter<Qualification>(QualificationParameterName, "The qualification object which should be manipulated."));
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
      Manipulate(EncodingParameter.ActualValue, QualificationParameter.ActualValue, RandomParameter.ActualValue);
      return base.InstrumentedApply();
    }

    protected abstract void Manipulate(QualificationEncoding encoding, Qualification qualification, IRandom random);
  }
}
