#region License Information
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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HEAL.Attic;

namespace HeuristicLab.Encodings.IntegerVectorEncoding {
  /// <summary>
  /// Generates a new random integer vector with each element uniformly distributed in a specified range.
  /// </summary>
  [Item("UniformRandomIntegerVectorCreator", "An operator which creates a new random int vector with each element uniformly distributed in a specified range.")]
  [StorableType("23B6374A-EE3B-4487-B728-635395D5F1B7")]
  public class UniformRandomIntegerVectorCreator : IntegerVectorCreator {
    [StorableConstructor]
    protected UniformRandomIntegerVectorCreator(StorableConstructorFlag _) : base(_) { }
    protected UniformRandomIntegerVectorCreator(UniformRandomIntegerVectorCreator original, Cloner cloner) : base(original, cloner) { }
    public UniformRandomIntegerVectorCreator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new UniformRandomIntegerVectorCreator(this, cloner);
    }

    /// <summary>
    /// Generates a new random integer vector with the given <paramref name="length"/>.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="length">The length of the int vector.</param>
    /// <param name="bounds">A matrix containing the inclusive lower and inclusive upper bound in the first and second column and a step size in the third column.
    /// Each line represents the bounds for a certain dimension. If fewer lines are given, the lines are cycled.</param>
    /// <returns>The newly created integer vector.</returns>
    public static IntegerVector Apply(IRandom random, int length, IntMatrix bounds) {
      var result = new IntegerVector(length);
      result.Randomize(random, bounds);
      return result;
    }

    /// <summary>
    /// Forwards the call to <see cref="Apply(IRandom, int, int, int)"/>.
    /// </summary>
    /// <param name="random">The pseudo random number generator to use.</param>
    /// <param name="length">The length of the int vector.</param>
    /// <param name="bounds">Contains in each row for each dimension minimum (inclusive), maximum (inclusive), and step size.</param>
    /// <returns>The newly created int vector.</returns>
    protected override IntegerVector Create(IRandom random, IntValue length, IntMatrix bounds) {
      return Apply(random, length.Value, bounds);
    }
  }
}
