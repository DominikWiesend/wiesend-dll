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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Wiesend.DataTypes.DataMapper.Interfaces;

namespace Wiesend.DataTypes.DataMapper.BaseClasses
{
    /// <summary>
    /// Type mapping base class
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public abstract class TypeMappingBase<Left, Right> : ITypeMapping<Left, Right>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TypeMappingBase()
        {
            this.Mappings = new ConcurrentBag<IMapping<Left, Right>>();
        }

        /// <summary>
        /// List of mappings
        /// </summary>
        protected ConcurrentBag<IMapping<Left, Right>> Mappings { get; private set; }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set action</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set function</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet);

        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns>This</returns>
        public virtual ITypeMapping AutoMap()
        {
            if (!Mappings.IsEmpty)
                return this;
            Type LeftType = typeof(Left);
            Type RightType = typeof(Right);
            if (RightType.Is<IDictionary<string, object>>() && LeftType.Is<IDictionary<string, object>>())
            {
                AddIDictionaryMappings();
            }
            else if (RightType.Is<IDictionary<string, object>>())
            {
                AddRightIDictionaryMapping(LeftType, RightType);
            }
            else if (LeftType.Is<IDictionary<string, object>>())
            {
                AddLeftIDictionaryMapping(LeftType, RightType);
            }
            else
            {
                var Properties = typeof(Left).GetProperties();
                Parallel.For(0, Properties.Length, x =>
                {
                    var DestinationProperty = RightType.GetProperty(Properties[x].Name);
                    if (DestinationProperty != null)
                    {
                        var LeftGet = Properties[x].PropertyGetter<Left>();
                        var RightGet = DestinationProperty.PropertyGetter<Right>();
                        this.AddMapping(LeftGet, RightGet);
                    }
                });
            }
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy(object Source, object Destination)
        {
            Copy((Left)Source, (Right)Destination);
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public abstract void Copy(Left Source, Right Destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public abstract void Copy(Right Source, Left Destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyLeftToRight(Left Source, Right Destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyRightToLeft(Right Source, Left Destination);

        private void AddIDictionaryMappings()
        {
            this.AddMapping(x => x,
            new Action<Left, object>((x, y) =>
            {
                var LeftSide = (IDictionary<string, object>)x;
                var RightSide = (IDictionary<string, object>)y;
                RightSide.CopyTo(LeftSide);
            }),
            x => x,
            new Action<Right, object>((x, y) =>
            {
                var LeftSide = (IDictionary<string, object>)y;
                var RightSide = (IDictionary<string, object>)x;
                LeftSide.CopyTo(RightSide);
            }));
        }

        private void AddLeftIDictionaryMapping([NotNull] Type LeftType, [NotNull] Type RightType)
        {
            if (LeftType == null) throw new ArgumentNullException(nameof(LeftType));
            if (RightType == null) throw new ArgumentNullException(nameof(RightType));
            var Properties = RightType.GetProperties();
            Parallel.For(0, Properties.Length, x =>
            {
                PropertyInfo Property = Properties[x];
                var RightGet = Properties[x].PropertyGetter<Right>();
                var RightSet = RightGet.PropertySetter<Right>().Compile();
                var LeftProperty = LeftType.GetProperty(Property.Name);
                if (LeftProperty != null)
                {
                    var LeftGet = LeftProperty.PropertyGetter<Left>();
                    this.AddMapping(LeftGet, RightGet);
                }
                else
                {
                    this.AddMapping(new Func<Left, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y;
                        if (Temp.ContainsKey(Property.Name))
                            return Temp[Property.Name];
                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace("_", ""), Property.Name, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(Key))
                            return Temp[Key];
                        return null;
                    }),
                    new Action<Left, object>((y, z) =>
                    {
                        var LeftSide = (IDictionary<string, object>)y;
                        if (LeftSide.ContainsKey(Property.Name))
                            LeftSide[Property.Name] = z;
                        else
                            LeftSide.Add(Property.Name, z);
                    }),
                    RightGet.Compile(),
                    new Action<Right, object>((y, z) =>
                    {
                        if (z != null)
                            RightSet(y, z);
                    }));
                }
            });
        }

        private void AddRightIDictionaryMapping([NotNull] Type LeftType, [NotNull] Type RightType)
        {
            if (LeftType == null) throw new ArgumentNullException(nameof(LeftType));
            if (RightType == null) throw new ArgumentNullException(nameof(RightType));
            var Properties = LeftType.GetProperties();
            Parallel.For(0, Properties.Length, x =>
            {
                PropertyInfo Property = Properties[x];
                var LeftGet = Property.PropertyGetter<Left>();
                var LeftSet = LeftGet.PropertySetter<Left>().Compile();
                var RightProperty = RightType.GetProperty(Property.Name);
                if (RightProperty != null)
                {
                    var RightGet = RightProperty.PropertyGetter<Right>();
                    this.AddMapping(LeftGet, RightGet);
                }
                else
                {
                    this.AddMapping(LeftGet.Compile(),
                    new Action<Left, object>((y, z) =>
                    {
                        if (z != null)
                            LeftSet(y, z);
                    }),
                    new Func<Right, object>(y =>
                    {
                        var Temp = (IDictionary<string, object>)y;
                        if (Temp.ContainsKey(Property.Name))
                            return Temp[Property.Name];
                        var Key = Temp.Keys.FirstOrDefault(z => string.Equals(z.Replace("_", ""), Property.Name, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(Key))
                            return Temp[Key];
                        return null;
                    }),
                    new Action<Right, object>((y, z) =>
                    {
                        var LeftSide = (IDictionary<string, object>)y;
                        if (LeftSide.ContainsKey(Property.Name))
                            LeftSide[Property.Name] = z;
                        else
                            LeftSide.Add(Property.Name, z);
                    }));
                }
            });
        }
    }
}