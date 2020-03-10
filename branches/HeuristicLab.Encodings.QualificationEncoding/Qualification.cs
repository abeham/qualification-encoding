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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Encodings.QualificationEncoding {
  [StorableType("9C92D610-1067-4623-AD43-3536BF00D3E2")]
  [Item("Qualification", "Represents qualification groups for workers.")]
  public class Qualification : Item {

    [Storable]
    public IDictionary<Pool, int> Groups { get; private set; } = new Dictionary<Pool, int>();

    public int Pools => Groups.Count;

    public int NumberOfQualifications => Groups.FirstOrDefault().Key?.Count ?? 0;
    public int NumberOfWorkers => Groups.Values.Sum();
    public int SumQualifications => Groups.Sum(x => x.Key.CountTrue * x.Value);

    [StorableConstructor]
    protected Qualification(StorableConstructorFlag _) : base(_) { }
    protected Qualification(Qualification original, Cloner cloner) : base(original, cloner) {
      Groups = original.Groups.ToDictionary(x => x.Key, x => x.Value);
    }
    public Qualification() : base() { }
    public Qualification(IEnumerable<KeyValuePair<Pool, int>> groups) {
      Groups = groups.ToDictionary(x => x.Key, x => x.Value);
    }
    public Qualification(Pool[] pools, int[] sizes) {
      if (pools.Length != sizes.Length) throw new ArgumentException("Unequal length of arrays", "sizes");
      Groups = pools.Zip(sizes, (p, s) => new { Key = p, Value = s }).ToDictionary(x => x.Key, x => x.Value);
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Qualification(this, cloner);
    }

    public bool[] ToBoolArray() {
      var result = new bool[NumberOfQualifications * NumberOfWorkers];
      int dstIndex = 0;

      var orderedGroups = Groups.OrderBy(x => string.Join("", x.Key));

      foreach (var group in orderedGroups) {
        var qualifications = group.Key.ToArray();
        for (int i = 0; i < group.Value; i++) {
          Array.Copy(qualifications, 0, result, dstIndex, NumberOfQualifications);
          dstIndex += NumberOfQualifications;
        }
      }

      return result;
    }

    public void AddOrIncrease(Pool pool, int size) {
      if (Groups.TryGetValue(pool, out int exist)) {
        Groups[pool] = size + exist;
      } else Groups.Add(pool, size);
    }

    public void DecreaseOrRemove(Pool pool, int toRemove) {
      if (Groups.TryGetValue(pool, out int exist)) {
        if (toRemove >= exist) Groups.Remove(pool);
        else Groups[pool] = exist - toRemove;
      } else throw new ArgumentException("Pool is not present.", "pool");
    }

    public bool Validate(QualificationEncoding encoding) {
      if (NumberOfQualifications != encoding.Qualifications)
        return false;

      if (NumberOfWorkers != encoding.Workers)
        return false;

      if (Groups.Values.Any(x => x < 1))
        return false;

      for (var q = 0; q < NumberOfQualifications; q++) {
        var set = false;
        foreach (var vec in Groups.Keys)
          if (vec[q]) {
            set = true;
            break;
          }
        if (!set) return false;
      }

      return true;
    }

    [StorableType("97BE4AA3-0EDD-409D-8247-8EA0550C5121")]
    public class Pool : IReadOnlyList<bool> {
      [Storable]
      private bool[] qualifications;

      [StorableConstructor]
      protected Pool(StorableConstructorFlag _) { }
      private Pool() { }

      public bool this[int index] => qualifications[index];

      public int Count => qualifications.Length;

      public int CountTrue => qualifications.Count(x => x);

      public IEnumerable<int> GetTrueIndices() {
        for (var i = 0; i < qualifications.Length; i++)
          if (qualifications[i]) yield return i;
      }
      public IEnumerable<int> GetFalseIndices() {
        for (var i = 0; i < qualifications.Length; i++)
          if (!qualifications[i]) yield return i;
      }

      public IEnumerator<bool> GetEnumerator() {
        return ((IEnumerable<bool>)qualifications).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() {
        return qualifications.GetEnumerator();
      }

      public override int GetHashCode() {
        if (qualifications == null) return base.GetHashCode();
        unchecked {
          int hash = 17;
          for (var i = 0; i < qualifications.Length; i++)
            hash = hash * 29 + (qualifications[i] ? 1231 : 1237);
          return hash;
        }
      }

      public override bool Equals(object obj) {
        if (ReferenceEquals(this, obj)) return true;
        var other = obj as Pool;
        if (other == null) return false;
        if (qualifications == null && other.qualifications == null) return ReferenceEquals(this, obj);
        if (qualifications?.Length != other.qualifications?.Length) return false;
        for (var i = 0; i < qualifications.Length; i++)
          if (qualifications[i] != other.qualifications[i]) return false;
        return true;
      }

      public Builder GetBuilder() {
        return Builder.CreateFrom(this);
      }

      public class Builder {
        private bool[] qualifications;

        private Builder() { }

        public static Builder CreateEmpty(int length) {
          return new Builder() { qualifications = new bool[length] };
        }

        public static Builder CreateFrom(Pool p) {
          var q = new bool[p.Count];
          Array.Copy(p.qualifications, q, q.Length);
          return new Builder() { qualifications = q };
        }

        public static Builder CreateFrom(bool[] a) {
          var q = new bool[a.Length];
          Array.Copy(a, q, q.Length);
          return new Builder() { qualifications = q };
        }

        public int Count {
          get { return qualifications.Length; }
        }

        public bool this[int index] {
          get { return qualifications[index]; }
          set { qualifications[index] = value; }
        }

        public Builder Set(int index, bool value = true) {
          qualifications[index] = value;
          return this;
        }

        public Builder Flip(int index) {
          qualifications[index] = !qualifications[index];
          return this;
        }

        public Builder Set(IEnumerable<int> indices, bool value = true) {
          foreach (var idx in indices)
            qualifications[idx] = value;
          return this;
        }

        public Builder Flip(IEnumerable<int> indices) {
          foreach (var idx in indices)
            qualifications[idx] = !qualifications[idx];
          return this;
        }

        public Builder UnionWith(Pool other) {
          for (var i = 0; i < other.Count; i++)
            if (other[i]) qualifications[i] = true;
          return this;
        }

        public Builder ExceptWith(Pool other) {
          for (var i = 0; i < other.Count; i++)
            if (other[i]) qualifications[i] = false;
          return this;
        }

        public Pool Build() {
          var result = new Pool() { qualifications = qualifications };
          qualifications = null;
          return result;
        }
      }
    }
  }
}
