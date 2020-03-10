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

using System;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("E1A4DD9D-0BC6-44B5-9F35-CDDA2972B771")]
  [Item("QualificationEncoding", "An encoding for qualifications.")]
  public sealed class QualificationEncoding : Encoding<IQualificationCreator> {
    #region Encoding Parameters
    [Storable]
    private IFixedValueParameter<IntValue> qualificationsParameter;
    public IFixedValueParameter<IntValue> QualificationsParameter {
      get { return qualificationsParameter; }
      set {
        if (value == null) throw new ArgumentNullException(nameof(QualificationsParameter), "Qualifications parameter must not be null.");
        if (value.Value == null) throw new ArgumentNullException(nameof(QualificationsParameter.Value), "Qualifications parameter value must not be null.");
        if (qualificationsParameter == value) return;

        if (qualificationsParameter != null) Parameters.Remove(qualificationsParameter);
        Parameters.Add(qualificationsParameter = value);
        OnQualificationsParameterChanged();
      }
    }

    [Storable]
    private IFixedValueParameter<IntValue> workersParameter;
    public IFixedValueParameter<IntValue> WorkersParameter {
      get { return workersParameter; }
      set {
        if (value == null) throw new ArgumentNullException(nameof(WorkersParameter), "Workers parameter must not be null.");
        if (value.Value == null) throw new ArgumentNullException(nameof(WorkersParameter.Value), "Workers parameter value must not be null.");
        if (workersParameter == value) return;

        if (workersParameter != null) Parameters.Remove(workersParameter);
        Parameters.Add(workersParameter = value);
        OnWorkersParameterChanged();
      }
    }
    #endregion

    #region Properties
    public int Qualifications {
      get => QualificationsParameter.Value.Value;
      set => QualificationsParameter.Value.Value = value;
    }

    public int Workers {
      get => WorkersParameter.Value.Value;
      set => WorkersParameter.Value.Value = value;
    }
    #endregion

    [StorableConstructor]
    private QualificationEncoding(StorableConstructorFlag _) : base(_) { }
    private QualificationEncoding(QualificationEncoding original, Cloner cloner) : base(original, cloner) {
      qualificationsParameter = cloner.Clone(original.qualificationsParameter);
      workersParameter = cloner.Clone(original.workersParameter);
      RegisterParameterEvents();
    }
    public QualificationEncoding() : this("Qualification", 10, 30) { }
    public QualificationEncoding(string name) : this(name, 10, 30) { }
    public QualificationEncoding(int qualifications, int workers) : this("Qualification", qualifications, workers) { }
    public QualificationEncoding(string name, int qualifications, int workers) : base(name) {
      Parameters.Add(qualificationsParameter = new FixedValueParameter<IntValue>(Name + ".Qualifications", new IntValue(qualifications)));
      Parameters.Add(workersParameter = new FixedValueParameter<IntValue>(Name + ".Workers", new IntValue(workers)));

      SolutionCreator = new RandomQualificationCreator();
      RegisterParameterEvents();
      DiscoverOperators();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new QualificationEncoding(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterParameterEvents();
      DiscoverOperators();
    }

    private void RegisterParameterEvents() {
      RegisterQualificationsParameterEvents();
      RegisterWorkersParameterEvents();
    }

    private void RegisterQualificationsParameterEvents() {
      QualificationsParameter.ValueChanged += (o, s) => ConfigureOperators(Operators);
      QualificationsParameter.Value.ValueChanged += (o, s) => ConfigureOperators(Operators);
    }

    private void RegisterWorkersParameterEvents() {
      QualificationsParameter.ValueChanged += (o, s) => ConfigureOperators(Operators);
      QualificationsParameter.Value.ValueChanged += (o, s) => ConfigureOperators(Operators);
    }

    private void OnQualificationsParameterChanged() {
      RegisterQualificationsParameterEvents();
      ConfigureOperators(Operators);
    }

    private void OnWorkersParameterChanged() {
      RegisterWorkersParameterEvents();
      ConfigureOperators(Operators);
    }

    #region Operator Discovery
    private static readonly IEnumerable<Type> encodingSpecificOperatorTypes;
    static QualificationEncoding() {
      encodingSpecificOperatorTypes = new List<Type>() {
        typeof (IQualificationOperator),
        typeof (IQualificationCreator),
        typeof (IQualificationCrossover),
        typeof (IQualificationManipulator)
      };
    }

    private void DiscoverOperators() {
      var assembly = typeof(IQualificationOperator).Assembly;
      var discoveredTypes = ApplicationManager.Manager.GetTypes(encodingSpecificOperatorTypes, assembly, true, false, false);
      var operators = discoveredTypes.Select(t => (IOperator)Activator.CreateInstance(t));
      var newOperators = operators.Except(Operators, new TypeEqualityComparer<IOperator>()).ToList();

      ConfigureOperators(newOperators);
      foreach (var @operator in newOperators)
        AddOperator(@operator);
    }
    #endregion

    public override void ConfigureOperators(IEnumerable<IOperator> operators) {
      ConfigureCreators(operators.OfType<IQualificationCreator>());
      ConfigureCrossovers(operators.OfType<IQualificationCrossover>());
      ConfigureManipulators(operators.OfType<IQualificationManipulator>());
    }

    private void ConfigureCreators(IEnumerable<IQualificationCreator> creators) {
      foreach (var creator in creators) {
        creator.QualificationParameter.ActualName = Name;
      }
    }

    private void ConfigureCrossovers(IEnumerable<IQualificationCrossover> crossovers) {
      foreach (var crossover in crossovers) {
        crossover.ParentsParameter.ActualName = Name;
        crossover.ChildParameter.ActualName = Name;
      }
    }

    private void ConfigureManipulators(IEnumerable<IQualificationManipulator> manipulators) {
      foreach (var manipulator in manipulators)
        manipulator.QualificationParameter.ActualName = Name;
    }
  }

  public static class IndividualExtensionMethods {
    public static Qualification Qualification(this Individual individual) {
      var encoding = individual.GetEncoding<QualificationEncoding>();
      return individual.Qualification(encoding.Name);
    }

    public static Qualification Qualification(this Individual individual, string name) {
      return (Qualification)individual[name];
    }
  }
}
