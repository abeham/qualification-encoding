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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("404480D0-5DF3-4258-A61B-96142818C99B")]
  [Item("MultiQualificationManipulator", "Randomly selects and applies one of its manipulators every time it is called.")]
  public sealed class MultiQualificationManipulator : StochasticMultiBranch<IQualificationManipulator>, IQualificationManipulator, IStochasticOperator {
    #region Parameter Names
    private const string QualificationParameterName = "Qualification";
    private const string EncodingParameterName = "Encoding";
    #endregion

    #region Parameters
    public ILookupParameter<Qualification> QualificationParameter => (ILookupParameter<Qualification>)Parameters[QualificationParameterName];
    public ILookupParameter<QualificationEncoding> EncodingParameter => (ILookupParameter<QualificationEncoding>)Parameters[EncodingParameterName];
    #endregion

    public override bool CanChangeName => false;
    protected override bool CreateChildOperation => true;

    [StorableConstructor]
    private MultiQualificationManipulator(StorableConstructorFlag _) : base(_) { }
    private MultiQualificationManipulator(MultiQualificationManipulator original, Cloner cloner) : base(original, cloner) { }
    public MultiQualificationManipulator() : base() {
      Parameters.Add(new LookupParameter<Qualification>(QualificationParameterName, "The qualification object which should be manipulated."));
      Parameters.Add(new LookupParameter<QualificationEncoding>(EncodingParameterName, "The encoding properties."));

      foreach (var type in ApplicationManager.Manager.GetTypes(typeof(IQualificationManipulator)))
        if (!typeof(MultiOperator<IQualificationManipulator>).IsAssignableFrom(type))
          Operators.Add((IQualificationManipulator)Activator.CreateInstance(type), true);

      SelectedOperatorParameter.ActualName = "SelectedManipulationOperator";
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MultiQualificationManipulator(this, cloner);
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      if (!Parameters.ContainsKey(EncodingParameterName)) {
        Parameters.Add(new LookupParameter<QualificationEncoding>(EncodingParameterName, "The encoding properties."));
      }
    }

    protected override void Operators_ItemsReplaced(object sender, CollectionItemsChangedEventArgs<IndexedItem<IQualificationManipulator>> e) {
      base.Operators_ItemsReplaced(sender, e);
    }

    protected override void Operators_ItemsAdded(object sender, CollectionItemsChangedEventArgs<IndexedItem<IQualificationManipulator>> e) {
      base.Operators_ItemsAdded(sender, e);
    }

    private void ParameterizeManipulators() {
      foreach (var manipulator in Operators.OfType<IQualificationManipulator>()) {
        manipulator.QualificationParameter.ActualName = QualificationParameter.Name;
        manipulator.EncodingParameter.ActualName = EncodingParameter.Name;
      }

      foreach (var stochasticOperator in Operators.OfType<IStochasticOperator>())
        stochasticOperator.RandomParameter.ActualName = RandomParameter.Name;
    }

    public override IOperation InstrumentedApply() {
      if (!Operators.Any())
        throw new InvalidOperationException($"{Name}: Please add at least one qualification manipulator to choose from.");

      return base.InstrumentedApply();
    }
  }
}
