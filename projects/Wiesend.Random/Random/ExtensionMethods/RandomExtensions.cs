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
//    Copyright(c) 2018 Dominik Wiesend. All rights reserved.
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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.Random.DefaultClasses;
using Wiesend.Random.Interfaces;

namespace Wiesend.Random
{
    /// <summary>
    /// Extension methods for the Random class
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RandomExtensions
    {
        private static Dictionary<Type, IGenerator> Generators;

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T Next<T>([NotNull] this System.Random Random, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T Next<T>([NotNull] this System.Random Random, T Min, T Max, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Generator.Next(Random, Min, Max);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> Next<T>([NotNull] this System.Random Random, int Amount, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Amount.Times(x => Generator.Next(Random));
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> Next<T>([NotNull] this System.Random Random, int Amount, T Min, T Max, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Amount.Times(x => Generator.Next(Random, Min, Max));
        }

        /// <summary>
        /// Picks a random item from the list
        /// </summary>
        /// <typeparam name="T">Type of object in the list</typeparam>
        /// <param name="Random">Random number generator</param>
        /// <param name="List">List to pick from</param>
        /// <returns>Item that is returned</returns>
        public static T Next<T>([NotNull] this System.Random Random, IEnumerable<T> List)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            if (List == null)
                return default;
            int x = 0;
            var Position = Random.Next(0, List.Count());
            foreach (T Item in List)
            {
                if (x == Position)
                    return Item;
                ++x;
            }
            return default;
        }

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        public static T NextClass<T>([NotNull] this System.Random Random, IGenerator<T> Generator = null)
            where T : class,new()
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
                Generator = new ClassGenerator<T>();
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        public static IEnumerable<T> NextClass<T>([NotNull] this System.Random Random, int Amount, IGenerator<T> Generator = null)
            where T : class,new()
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            if (Generator == null)
                Generator = new ClassGenerator<T>();
            return Amount.Times(x => Generator.Next(Random));
        }

        /// <summary>
        /// Randomly generates a value of the specified enum type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T NextEnum<T>([NotNull] this System.Random Random, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            Generator = Generator.Check(new EnumGenerator<T>());
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified enum type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> NextEnum<T>([NotNull] this System.Random Random, int Amount, IGenerator<T> Generator = null)
        {
            if (Random == null) throw new ArgumentNullException(nameof(Random));
            SetupGenerators();
            Generator = Generator.Check(new EnumGenerator<T>());
            return Amount.Times(x => Generator.Next(Random));
        }

        /// <summary>
        /// Registers a generator with a type
        /// </summary>
        /// <typeparam name="T">Type to associate with the generator</typeparam>
        /// <param name="Rand">Random number generator</param>
        /// <param name="Generator">Generator to associate with the type</param>
        /// <returns>The random number generator</returns>
        public static System.Random RegisterGenerator<T>(this System.Random Rand, IGenerator Generator)
        {
            return Rand.RegisterGenerator(typeof(T), Generator);
        }

        /// <summary>
        /// Registers a generator with a type
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <param name="Generator">Generator to associate with the type</param>
        /// <param name="Type">Type to associate with the generator</param>
        /// <returns>The random number generator</returns>
        public static System.Random RegisterGenerator(this System.Random Rand, Type Type, IGenerator Generator)
        {
            if (Generators.ContainsKey(Type))
                Generators[Type] = Generator;
            else
                Generators.Add(Type, Generator);
            return Rand;
        }

        /// <summary>
        /// Resets the generators to the defaults
        /// </summary>
        /// <param name="Random">Random object</param>
        /// <returns>The random object sent in</returns>
        public static System.Random ResetGenerators(this System.Random Random)
        {
            Generators = null;
            SetupGenerators();
            return Random;
        }

        /// <summary>
        /// Shuffles a list randomly
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="List">List of objects to shuffle</param>
        /// <returns>The shuffled list</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1827:Do not use Count() or LongCount() when Any() can be used", Justification = "<Pending>")]
        public static IEnumerable<T> Shuffle<T>(this System.Random Random, IEnumerable<T> List)
        {
            if (List == null || List.Count() == 0)
                return List;
            return List.OrderBy(x => Random.Next());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
        private static void SetupGenerators()
        {
            if (Generators != null)
                return;
            Generators = new Dictionary<Type, IGenerator>();
            Generators.Add(typeof(bool), new BoolGenerator());
            Generators.Add(typeof(decimal), new DecimalGenerator<decimal>());
            Generators.Add(typeof(double), new DecimalGenerator<double>());
            Generators.Add(typeof(float), new DecimalGenerator<float>());
            Generators.Add(typeof(byte), new IntegerGenerator<byte>());
            Generators.Add(typeof(char), new IntegerGenerator<char>());
            Generators.Add(typeof(int), new IntegerGenerator<int>());
            Generators.Add(typeof(long), new IntegerGenerator<long>());
            Generators.Add(typeof(sbyte), new IntegerGenerator<sbyte>());
            Generators.Add(typeof(short), new IntegerGenerator<short>());
            Generators.Add(typeof(uint), new IntegerGenerator<uint>());
            Generators.Add(typeof(ulong), new IntegerGenerator<ulong>());
            Generators.Add(typeof(ushort), new IntegerGenerator<ushort>());
            Generators.Add(typeof(DateTime), new DateTimeGenerator());
            Generators.Add(typeof(TimeSpan), new TimeSpanGenerator());
            Generators.Add(typeof(Color), new ColorGenerator());
            Generators.Add(typeof(string), new StringGenerator());
            Generators.Add(typeof(Guid), new GuidGenerator());
        }
    }
}