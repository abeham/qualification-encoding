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

using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;

namespace HeuristicLab.Problems.DataAnalysis.Symbolic {
  public static class LinearModelToTreeConverter {
    public static ISymbolicExpressionTree CreateTree(string[] variableNames, double[] coefficients,
      double @const = 0) {
      return CreateTree(variableNames, new int[variableNames.Length], coefficients, @const);
    }

    public static ISymbolicExpressionTree CreateTree(
      IEnumerable<KeyValuePair<string, IEnumerable<string>>> factors, double[] factorCoefficients,
      string[] variableNames, double[] coefficients,
      double @const = 0) {

      if (factorCoefficients.Length == 0 && coefficients.Length == 0 && @const==0) throw new ArgumentException();

      // Combine both trees
      ISymbolicExpressionTreeNode add = (new Addition()).CreateTreeNode();

      // Create tree for double variables
      if (coefficients.Length > 0) {
        var varTree = CreateTree(variableNames, new int[variableNames.Length], coefficients);
        foreach (var varNode in varTree.IterateNodesPrefix().OfType<VariableTreeNode>())
          add.AddSubtree(varNode);
      }

      // Create tree for string variables
      if (factorCoefficients.Length > 0) {
        var factorTree = CreateTree(factors, factorCoefficients);
        foreach (var binFactorNode in factorTree.IterateNodesPrefix().OfType<BinaryFactorVariableTreeNode>())
          add.AddSubtree(binFactorNode);
      }

      if (@const!=0.0) {
        ConstantTreeNode cNode = (ConstantTreeNode)new Constant().CreateTreeNode();
        cNode.Value = @const;
        add.AddSubtree(cNode);
      }

      ISymbolicExpressionTree tree = new SymbolicExpressionTree(new ProgramRootSymbol().CreateTreeNode());
      ISymbolicExpressionTreeNode startNode = new StartSymbol().CreateTreeNode();
      tree.Root.AddSubtree(startNode);
      startNode.AddSubtree(add);
      return tree;
    }

    public static ISymbolicExpressionTree CreateTree(string[] variableNames, int[] lags, double[] coefficients,
      double @const = 0) {
      if (variableNames.Length == 0 ||
        variableNames.Length != coefficients.Length ||
        variableNames.Length != lags.Length)
        throw new ArgumentException("The length of the variable names, lags, and coefficients vectors must match");

      ISymbolicExpressionTree tree = new SymbolicExpressionTree(new ProgramRootSymbol().CreateTreeNode());
      ISymbolicExpressionTreeNode startNode = new StartSymbol().CreateTreeNode();
      tree.Root.AddSubtree(startNode);
      ISymbolicExpressionTreeNode addition = new Addition().CreateTreeNode();
      startNode.AddSubtree(addition);

      for (int i = 0; i < variableNames.Length; i++) {
        if (lags[i] == 0) {
          VariableTreeNode vNode = (VariableTreeNode)new Variable().CreateTreeNode();
          vNode.VariableName = variableNames[i];
          vNode.Weight = coefficients[i];
          addition.AddSubtree(vNode);
        } else {
          LaggedVariableTreeNode vNode = (LaggedVariableTreeNode)new LaggedVariable().CreateTreeNode();
          vNode.VariableName = variableNames[i];
          vNode.Weight = coefficients[i];
          vNode.Lag = lags[i];
          addition.AddSubtree(vNode);
        }
      }

      if (!@const.IsAlmost(0.0)) {
        ConstantTreeNode cNode = (ConstantTreeNode)new Constant().CreateTreeNode();
        cNode.Value = @const;
        addition.AddSubtree(cNode);
      }
      return tree;
    }

    public static ISymbolicExpressionTree CreateTree(IEnumerable<KeyValuePair<string, IEnumerable<string>>> factors,
      double[] factorCoefficients,
      double @const = 0) {

      ISymbolicExpressionTree tree = new SymbolicExpressionTree(new ProgramRootSymbol().CreateTreeNode());
      ISymbolicExpressionTreeNode startNode = new StartSymbol().CreateTreeNode();
      tree.Root.AddSubtree(startNode);
      ISymbolicExpressionTreeNode addition = new Addition().CreateTreeNode();
      startNode.AddSubtree(addition);

      int i = 0;
      foreach (var factor in factors) {
        var varName = factor.Key;
        foreach (var factorValue in factor.Value) {
          var node = (BinaryFactorVariableTreeNode)new BinaryFactorVariable().CreateTreeNode();
          node.VariableValue = factorValue;
          node.VariableName = varName;
          node.Weight = factorCoefficients[i];
          addition.AddSubtree(node);
          i++;
        }
      }

      if (!@const.IsAlmost(0.0)) {
        ConstantTreeNode cNode = (ConstantTreeNode)new Constant().CreateTreeNode();
        cNode.Value = @const;
        addition.AddSubtree(cNode);
      }
      return tree;
    }
  }
}
