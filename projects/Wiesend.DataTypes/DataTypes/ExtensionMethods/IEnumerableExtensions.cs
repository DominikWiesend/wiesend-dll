#region Project Description [About this]
// =================================================================================
//            The whole Project is Licensed under the MIT License
// =================================================================================
// =================================================================================
//    Wiesend's Dynamic Link Library is a collection of reusable code that 
//    I've written, or found throughout my programming career. 
//
//    I tried my very best to mention all of the original copyright holders. 
//    I hope that all code which I've copied from others is mentioned and all 
//    their copyrights are given. The copied code (or code snippets) could 
//    already be modified by me or others.
// =================================================================================
#endregion of Project Description
#region Original Source Code [Links to all original source code]
// =================================================================================
//          Original Source Code [Links to all original source code]
// =================================================================================
// https://github.com/JaCraig/Craig-s-Utility-Library
// =================================================================================
//    I didn't wrote this source totally on my own, this class could be nearly a 
//    clone of the project of James Craig, I did highly get inspired by him, so 
//    this piece of code isn't totally mine, shame on me, I'm not the best!
// =================================================================================
#endregion of where is the original source code
#region Licenses [MIT Licenses]
#region MIT License [James Craig]
// =================================================================================
//    Copyright(c) 2014 <a href="http://www.gutgames.com">James Craig</a>
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [James Craig] 
#region MIT License [Dominik Wiesend]
// =================================================================================
//    Copyright(c) 2016 Dominik Wiesend. All rights reserved.
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [Dominik Wiesend] 
#endregion of Licenses [MIT Licenses]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using JetBrains.Annotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Combines multiple IEnumerables together and returns a new IEnumerable containing all of
        /// the values
        /// </summary>
        /// <typeparam name="T">Type of the data in the IEnumerable</typeparam>
        /// <param name="Enumerable1">IEnumerable 1</param>
        /// <param name="Additions">IEnumerables to concat onto the first item</param>
        /// <returns>A new IEnumerable containing all values</returns>
        /// <example>
        /// <code>
        /// int[] TestObject1 = new int[] { 1, 2, 3 }; int[] TestObject2 = new int[] { 4, 5, 6
        /// }; int[] TestObject3 = new int[] { 7, 8, 9 }; TestObject1 =
        /// TestObject1.Concat(TestObject2, TestObject3).ToArray();
        /// </code>
        /// </example>
        public static IEnumerable<T> Concat<T>([NotNull] this IEnumerable<T> Enumerable1, [NotNull] params IEnumerable<T>[] Additions)
        {
            if (Enumerable1 == null) throw new ArgumentNullException(nameof(Enumerable1));
            if (Additions == null) throw new ArgumentNullException(nameof(Additions));
            Additions.ThrowIfAny(x => x == null, new ArgumentNullException(nameof(Additions)));
            var Results = new List<T>();
            Results.AddRange(Enumerable1);
            for (int x = 0; x < Additions.Length; ++x)
                Results.AddRange(Additions[x]);
            return Results;
        }

        /// <summary>
        /// Returns only distinct items from the IEnumerable based on the predicate
        /// </summary>
        /// <typeparam name="T">Object type within the list</typeparam>
        /// <param name="Enumerable">List of objects</param>
        /// <param name="Predicate">
        /// Predicate that is used to determine if two objects are equal. True if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>An IEnumerable of only the distinct items</returns>
        public static IEnumerable<T> Distinct<T>([NotNull] this IEnumerable<T> Enumerable, [NotNull] Func<T, T, bool> Predicate)
        {
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Enumerable == null) throw new ArgumentNullException(nameof(Enumerable));
            var Results = new List<T>();
            foreach (T Item in Enumerable)
            {
                bool Found = false;
                foreach (T Item2 in Results)
                {
                    if (Predicate(Item, Item2))
                    {
                        Found = true;
                        break;
                    }
                }
                if (!Found)
                    Results.Add(Item);
            }
            return Results;
        }

        /// <summary>
        /// Returns elements starting at the index and ending at the end index
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List to search</param>
        /// <param name="Start">Start index (inclusive)</param>
        /// <param name="End">End index (exclusive)</param>
        /// <returns>The items between the start and end index</returns>
        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> List, int Start, int End)
        {
            if (List == null)
                return List;
            if (End > List.Count())
                End = List.Count();
            if (Start < 0)
                Start = 0;
            var ReturnList = new List<T>();
            for (int x = Start; x < End; ++x)
                ReturnList.Add(List.ElementAt(x));
            return ReturnList;
        }

        /// <summary>
        /// Removes values from a list that meet the criteria set forth by the predicate
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="Value">List to cull items from</param>
        /// <param name="Predicate">Predicate that determines what items to remove</param>
        /// <returns>An IEnumerable with the objects that meet the criteria removed</returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> Value, [NotNull] Func<T, bool> Predicate)
        {
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Value == null)
                return Value;
            return Value.Where(x => !Predicate(x));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>([NotNull] this IEnumerable<T> List, int Start, int End, [NotNull] Action<T> Action)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            if (!(End + 1 - Start >= 0)) throw new ArgumentException("End must be greater than start", nameof(End));
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and
        /// returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> For<T, R>([NotNull] this IEnumerable<T> List, int Start, int End, [NotNull] Func<T, R> Function)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (!(End + 1 - Start >= 0)) throw new InvalidOperationException("End must be greater than start");
            var ReturnValues = new List<R>();
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>([NotNull] this IEnumerable<T> List, [NotNull] Action<T> Action)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            foreach (T Item in List)
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> ForEach<T, R>([NotNull] this IEnumerable<T> List, [NotNull] Func<T, R> Function)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            var ReturnValues = new List<R>();
            foreach (T Item in List)
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>([NotNull] this IEnumerable<T> List, [NotNull] Action<T> Action, [NotNull] Action<T, Exception> CatchAction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            if (CatchAction == null) throw new ArgumentNullException(nameof(CatchAction));
            foreach (T Item in List)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> ForEach<T, R>([NotNull] this IEnumerable<T> List, [NotNull] Func<T, R> Function, [NotNull] Action<T, Exception> CatchAction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (CatchAction == null) throw new ArgumentNullException(nameof(CatchAction));
            var ReturnValues = new List<R>();
            foreach (T Item in List)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>([NotNull] this IEnumerable<T> List, [NotNull] Action<T> Action)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            Parallel.ForEach(List, Action);
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> ForEachParallel<T, R>([NotNull] this IEnumerable<T> List, [NotNull] Func<T, R> Function)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            return List.ForParallel(0, List.Count() - 1, Function);
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>([NotNull] this IEnumerable<T> List, [NotNull] Action<T> Action, [NotNull] Action<T, Exception> CatchAction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            if (CatchAction == null) throw new ArgumentNullException(nameof(CatchAction));
            Parallel.ForEach<T>(List, delegate(T Item)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> ForEachParallel<T, R>([NotNull] this IEnumerable<T> List, [NotNull] Func<T, R> Function, [NotNull] Action<T, Exception> CatchAction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (CatchAction == null) throw new ArgumentNullException(nameof(CatchAction));
            var ReturnValues = new List<R>();
            Parallel.ForEach<T>(List, delegate(T Item)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>([NotNull] this IEnumerable<T> List, int Start, int End, [NotNull] Action<T> Action)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            if (!(End + 1 - Start >= 0)) throw new InvalidOperationException("End must be greater than start");
            Parallel.For(Start, End + 1, new Action<int>(x => Action(List.ElementAt(x))));
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> ForParallel<T, R>([NotNull] this IEnumerable<T> List, int Start, int End, [NotNull] Func<T, R> Function)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (!(End + 1 - Start >= 0)) throw new InvalidOperationException("End must be greater than start");
            R[] Results = new R[(End + 1) - Start];
            Parallel.For(Start, End + 1, new Action<int>(x => Results[x - Start] = Function(List.ElementAt(x))));
            return Results;
        }

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Count">Numbers of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>([NotNull] this IEnumerable<T> List, int Count)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            return List.ElementsBetween(List.Count() - Count, List.Count());
        }

        /// <summary>
        /// Does a left join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a left join of the two lists</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> LeftJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            [NotNull] IEnumerable<T2> inner,
            [NotNull] Func<T1, Key> outerKeySelector,
            [NotNull] Func<T2, Key> innerKeySelector,
            [NotNull] Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            comparer = comparer ?? new GenericEqualityComparer<Key>();
            return outer.ForEach(x => new { left = x, right = inner.FirstOrDefault(y => comparer.Equals(innerKeySelector(y), outerKeySelector(x))) })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Does an outer join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns an outer join of the two lists</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> OuterJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            [NotNull] IEnumerable<T2> inner,
            [NotNull] Func<T1, Key> outerKeySelector,
            [NotNull] Func<T2, Key> innerKeySelector,
            [NotNull] Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var Left = outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            var Right = outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            return Left.Union(Right);
        }

        /// <summary>
        /// Determines the position of an object if it is present, otherwise it returns -1
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List of objects to search</param>
        /// <param name="Object">Object to find the position of</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is present
        /// </param>
        /// <returns>The position of the object if it is present, otherwise -1</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static int PositionOf<T>([NotNull] this IEnumerable<T> List, T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            EqualityComparer = EqualityComparer.Check(() => new GenericEqualityComparer<T>());
            int Count = 0;
            foreach (T Item in List)
            {
                if (EqualityComparer.Equals(Object, Item))
                    return Count;
                ++Count;
            }
            return -1;
        }

        /// <summary>
        /// Does a right join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a right join of the two lists</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<R> RightJoin<T1, T2, Key, R>([NotNull] this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            [NotNull] Func<T1, Key> outerKeySelector,
            [NotNull] Func<T2, Key> innerKeySelector,
            [NotNull] Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            comparer = comparer ?? new GenericEqualityComparer<Key>();
            return inner.ForEach(x => new { left = outer.FirstOrDefault(y => comparer.Equals(innerKeySelector(x), outerKeySelector(y))), right = x })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>([NotNull] this IEnumerable<T> List, [NotNull] Predicate<T> Predicate, [NotNull] Func<Exception> Exception)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Exception == null) throw new ArgumentNullException(nameof(Exception));
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception();
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>([NotNull] this IEnumerable<T> List, [NotNull] Predicate<T> Predicate, [NotNull] Exception Exception)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Exception == null) throw new ArgumentNullException(nameof(Exception));
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>([NotNull] this IEnumerable<T> List, [NotNull] Predicate<T> Predicate, [NotNull] Func<Exception> Exception)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Exception == null) throw new ArgumentNullException(nameof(Exception));
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception();
            }
            return List;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>([NotNull] this IEnumerable<T> List, [NotNull] Predicate<T> Predicate, [NotNull] Exception Exception)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Exception == null) throw new ArgumentNullException(nameof(Exception));
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception;
            }
            return List;
        }

        /// <summary>
        /// Converts the IEnumerable to a DataTable
        /// </summary>
        /// <typeparam name="T">Type of the objects in the IEnumerable</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="Columns">Column names (if empty, uses property names)</param>
        /// <returns>The list as a DataTable</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1827:Do not use Count() or LongCount() when Any() can be used", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public static DataTable To<T>(this IEnumerable<T> List, params string[] Columns)
        {
            var ReturnValue = new DataTable();
            ReturnValue.Locale = CultureInfo.CurrentCulture;
            if (List == null || List.Count() == 0)
                return ReturnValue;
            var Properties = typeof(T).GetProperties();
            if (Columns.Length == 0)
                Columns = Properties.ToArray(x => x.Name);
            Columns.ForEach(x => ReturnValue.Columns.Add(x, Properties.FirstOrDefault(z => z.Name == x).PropertyType));
            object[] Row = new object[Columns.Length];
            foreach (T Item in List)
            {
                for (int x = 0; x < Row.Length; ++x)
                {
                    Row[x] = Properties.FirstOrDefault(z => z.Name == Columns[x]).GetValue(Item, new object[] { });
                }
                ReturnValue.Rows.Add(Row);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts the IEnumerable to a DataTable
        /// </summary>
        /// <param name="List">List to convert</param>
        /// <param name="Columns">Column names (if empty, uses property names)</param>
        /// <returns>The list as a DataTable</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public static DataTable To([NotNull] this IEnumerable List, params string[] Columns)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            var ReturnValue = new DataTable();
            ReturnValue.Locale = CultureInfo.CurrentCulture;
            int Count = 0;
            var i = List.GetEnumerator();
            while (i.MoveNext())
                ++Count;
            if (List == null || Count == 0)
                return ReturnValue;
            var ListEnumerator = List.GetEnumerator();
            ListEnumerator.MoveNext();
            var Properties = ListEnumerator.Current.GetType().GetProperties();
            if (Columns.Length == 0)
                Columns = Properties.ToArray(x => x.Name);
            Columns.ForEach(x => ReturnValue.Columns.Add(x, Properties.FirstOrDefault(z => z.Name == x).PropertyType));
            object[] Row = new object[Columns.Length];
            foreach (object Item in List)
            {
                for (int x = 0; x < Row.Length; ++x)
                {
                    Row[x] = Properties.FirstOrDefault(z => z.Name == Columns[x]).GetValue(Item, new object[] { });
                }
                ReturnValue.Rows.Add(Row);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static Target[] ToArray<Source, Target>([NotNull] this IEnumerable<Source> List, [NotNull] Func<Source, Target> ConvertingFunction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (ConvertingFunction == null) throw new ArgumentNullException(nameof(ConvertingFunction));
            return List.ForEach(ConvertingFunction).ToArray();
        }

        /// <summary>
        /// Converts an IEnumerable to a list
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">IEnumerable to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The list containing the items from the IEnumerable</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static List<Target> ToList<Source, Target>([NotNull] this IEnumerable<Source> List, [NotNull] Func<Source, Target> ConvertingFunction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (ConvertingFunction == null) throw new ArgumentNullException(nameof(ConvertingFunction));
            return List.ForEach(ConvertingFunction).ToList();
        }

        /// <summary>
        /// Converts the IEnumerable to an observable list
        /// </summary>
        /// <typeparam name="Source">The type of the source.</typeparam>
        /// <typeparam name="Target">The type of the target.</typeparam>
        /// <param name="List">The list to convert</param>
        /// <param name="ConvertingFunction">The converting function.</param>
        /// <returns>The observable list version of the original list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static ObservableList<Target> ToObservableList<Source, Target>([NotNull] this IEnumerable<Source> List, [NotNull] Func<Source, Target> ConvertingFunction)
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            if (ConvertingFunction == null) throw new ArgumentNullException(nameof(ConvertingFunction));
            return new ObservableList<Target>(List.ForEach(ConvertingFunction));
        }

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ItemOutput">
        /// Used to convert the item to a string (defaults to calling ToString)
        /// </param>
        /// <param name="Seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>([NotNull] this IEnumerable<T> List, Func<T, string> ItemOutput = null, string Seperator = ",")
        {
            if (List == null) throw new ArgumentNullException(nameof(List));
            Seperator = Seperator.Check("");
            ItemOutput = ItemOutput.Check(x => x.ToString());
            var Builder = new StringBuilder();
            string TempSeperator = "";
            List.ForEach(x =>
            {
                Builder.Append(TempSeperator).Append(ItemOutput(x));
                TempSeperator = Seperator;
            });
            return Builder.ToString();
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="collection">The collection hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> property)
        {
            if (collection == null)
                yield break;

            foreach (T item in collection)
            {
                yield return item;

                foreach (T inner in Transverse(property(item), property))
                    yield return inner;
            }
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="item">The root node of the hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this T item, Func<T, IEnumerable<T>> property)
        {
            if (item == null)
                yield break;

            yield return item;

            foreach (T inner in Transverse(property(item), property))
                yield return inner;
        }
    }
}