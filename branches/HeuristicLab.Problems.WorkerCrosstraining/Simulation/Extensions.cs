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

using System;
using System.Collections.Generic;
using System.Linq;
using SimSharp;

namespace HeuristicLab.Problems.WorkerCrosstraining {
  static class Extensions {

    /// <summary>
    /// Chooses one elements from a sequence giving each element an equal chance.
    /// </summary>
    /// <remarks>
    /// Runtime complexity is O(1) for sequences that are of type <see cref="IList{T}"/> and
    /// O(N) for all other.
    /// </remarks>
    /// <exception cref="ArgumentException">If the sequence is empty.</exception>
    /// <typeparam name="T">The type of the items to be selected.</typeparam>
    /// <param name="source">The sequence of elements.</param>
    /// <param name="random">The random number generator to use, its NextDouble() method must produce values in the range [0;1)</param>
    /// <param name="count">The number of items to be selected.</param>
    /// <returns>An element that has been chosen randomly from the sequence.</returns>
    public static T SampleRandom<T>(this IEnumerable<T> source, IRandom random) {
      if (!source.Any()) throw new ArgumentException("sequence is empty.", "source");
      return source.SampleRandom(random, 1).First();
    }

    /// <summary>
    /// Chooses <paramref name="count"/> elements from a sequence with repetition with equal chances for each element.
    /// </summary>
    /// <remarks>
    /// Runtime complexity is O(count) for sequences that are <see cref="IList{T}"/> and
    /// O(N * count) for all other. No exception is thrown if the sequence is empty.
    /// 
    /// The method is online.
    /// </remarks>
    /// <typeparam name="T">The type of the items to be selected.</typeparam>
    /// <param name="source">The sequence of elements.</param>
    /// <param name="random">The random number generator to use, its NextDouble() method must produce values in the range [0;1)</param>
    /// <param name="count">The number of items to be selected.</param>
    /// <returns>A sequence of elements that have been chosen randomly.</returns>
    public static IEnumerable<T> SampleRandom<T>(this IEnumerable<T> source, IRandom random, int count) {
      var listSource = source as IList<T>;
      if (listSource != null) {
        while (count > 0) {
          yield return listSource[random.Next(listSource.Count)];
          count--;
        }
      } else {
        while (count > 0) {
          var enumerator = source.GetEnumerator();
          enumerator.MoveNext();
          T selectedItem = enumerator.Current;
          int counter = 1;
          while (enumerator.MoveNext()) {
            counter++;
            if (counter * random.NextDouble() < 1.0)
              selectedItem = enumerator.Current;
          }
          yield return selectedItem;
          count--;
        }
      }
    }
  }
}
