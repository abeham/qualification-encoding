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
  [StorableType("A3EE9698-250C-490B-8E4C-F89E2B92550A")]
  [Item("MultiQualificationCrossover", "Randomly selects and applies one of its crossovers every time it is called.")]
  public sealed class MultiQualificationCrossover : StochasticMultiBranch<IQualificationCrossover>, IQualificationCrossover, IStochasticOperator {
    #region Parameter Names
    private const string ParentsParameterName = "Parents";
    private const string ChildParameterName = "Child";
    private const string EncodingParameterName = "Encoding";
    #endregion

    #region Parameters
    public ILookupParameter<ItemArray<Qualification>> ParentsParameter => (ILookupParameter<ItemArray<Qualification>>)Parameters[ParentsParameterName];
    public ILookupParameter<Qualification> ChildParameter => (ILookupParameter<Qualification>)Parameters[ChildParameterName];
    public ILookupParameter<QualificationEncoding> EncodingParameter => (ILookupParameter<QualificationEncoding>)Parameters[EncodingParameterName];
    #endregion

    public override bool CanChangeName => false;
    protected override bool CreateChildOperation => true;

    [StorableConstructor]
    private MultiQualificationCrossover(StorableConstructorFlag _) : base(_) { }
    private MultiQualificationCrossover(MultiQualificationCrossover original, Cloner cloner) : base(original, cloner) { }
    public MultiQualificationCrossover() : base() {
      Parameters.Add(new ScopeTreeLookupParameter<Qualification>(ParentsParameterName, "The parent qualifications which should be crossed."));
      ParentsParameter.ActualName = "Qualification";
      Parameters.Add(new LookupParameter<Qualification>(ChildParameterName, "The child qualification resulting from the crossover."));
      ChildParameter.ActualName = "Qualification";
      Parameters.Add(new LookupParameter<QualificationEncoding>(EncodingParameterName, "The encoding properties."));

      foreach (var type in ApplicationManager.Manager.GetTypes(typeof(IQualificationCrossover)))
        if (!typeof(MultiOperator<IQualificationCrossover>).IsAssignableFrom(type))
          Operators.Add((IQualificationCrossover)Activator.CreateInstance(type), true);

      SelectedOperatorParameter.ActualName = "SelectedCrossoverOperator";
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new MultiQualificationCrossover(this, cloner);
    }

    protected override void Operators_ItemsReplaced(object sender, CollectionItemsChangedEventArgs<IndexedItem<IQualificationCrossover>> e) {
      base.Operators_ItemsReplaced(sender, e);
    }

    protected override void Operators_ItemsAdded(object sender, CollectionItemsChangedEventArgs<IndexedItem<IQualificationCrossover>> e) {
      base.Operators_ItemsAdded(sender, e);
    }

    private void ParameterizeManipulators() {
      foreach (var xo in Operators.OfType<IQualificationCrossover>()) {
        xo.ParentsParameter.ActualName = ParentsParameter.Name;
        xo.ChildParameter.ActualName = ChildParameter.Name;
        xo.EncodingParameter.ActualName = EncodingParameter.Name;
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
