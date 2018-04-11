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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Mapper
{
    /// <summary>
    /// Mapping manager
    /// </summary>
    public class Manager : IEnumerable<IMapping>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Manager(IEnumerable<IMapping> Mappings)
        {
            Contract.Requires<ArgumentNullException>(Mappings != null, "Mappings");
            this.Mappings = new ListMapping<Type, IMapping>();
            this.Structures = new Dictionary<Type, Graph<IMapping>>();
            Mappings.ForEach(x => this.Mappings.Add(x.ObjectType, x));
            Mappings.GroupBy(x => x.DatabaseConfigType).ForEach(x => Structures.Add(x.Key, FindGraph(x)));
        }

        /// <summary>
        /// Mappings
        /// </summary>
        protected ListMapping<Type, IMapping> Mappings { get; private set; }

        /// <summary>
        /// Gets the structures.
        /// </summary>
        /// <value>The structures.</value>
        protected Dictionary<Type, Graph<IMapping>> Structures { get; private set; }

        /// <summary>
        /// Gets the enumerator for the mappings
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IMapping> GetEnumerator()
        {
            foreach (IEnumerable<IMapping> MappingList in Mappings.Values)
            {
                foreach (IMapping Mapping in MappingList)
                {
                    yield return Mapping;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Graph{IMapping}"/> with the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        /// <value>The <see cref="Graph{IMapping}"/>.</value>
        public Graph<IMapping> GetStructure(Type Key) => Structures.GetValue(Key) ?? new Graph<IMapping>();

        /// <summary>
        /// Gets the enumerator for the mappings
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (IEnumerable<IMapping> MappingList in Mappings.Values)
            {
                foreach (IMapping Mapping in MappingList)
                {
                    yield return Mapping;
                }
            }
        }

        /// <summary>
        /// Outputs the mapping information as a string
        /// </summary>
        /// <returns>The mapping information as a string</returns>
        public override string ToString()
        {
            return "Mappers: " + Mappings.ToString(x => x.Value.OrderBy(y => y.ToString()).ToString(y => y.ToString())) + "\r\n";
        }

        /// <summary>
        /// Finds the graph.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <returns>The graph for the list of mappings</returns>
        private Graph<IMapping> FindGraph(IEnumerable<IMapping> mappings)
        {
            var Graph = new Graph<IMapping>();
            foreach (var Mapping in mappings)
            {
                Graph.AddVertex(Mapping);
            }
            foreach (var Mapping in mappings)
            {
                Type ObjectType = Mapping.ObjectType;
                Type BaseType = ObjectType.BaseType;
                while (BaseType != typeof(object)
                    && BaseType != null)
                {
                    var BaseMapping = mappings.FirstOrDefault(x => x.ObjectType == BaseType);
                    if (BaseMapping != null)
                    {
                        var SinkVertex = Graph.Vertices.First(x => x.Data == BaseMapping);
                        var SourceVertex = Graph.Vertices.First(x => x.Data == Mapping);
                        SourceVertex.AddOutgoingEdge(SinkVertex);
                    }
                    BaseType = BaseType.BaseType;
                }
                foreach (Type Interface in ObjectType.GetInterfaces())
                {
                    var BaseMapping = mappings.FirstOrDefault(x => x.ObjectType == Interface);
                    if (BaseMapping != null)
                    {
                        var SinkVertex = Graph.Vertices.First(x => x.Data == BaseMapping);
                        var SourceVertex = Graph.Vertices.First(x => x.Data == Mapping);
                        SourceVertex.AddOutgoingEdge(SinkVertex);
                    }
                }
            }
            return Graph;
        }

        /// <summary>
        /// Gets the mapping specified by the object type
        /// </summary>
        /// <param name="Key">The object type</param>
        /// <returns>The mapping specified</returns>
        public IEnumerable<IMapping> this[Type Key] { get { return Mappings.GetValue(Key) ?? new ConcurrentBag<IMapping>(); } }

        /// <summary>
        /// Gets the mapping specified by the object type and source
        /// </summary>
        /// <param name="Key">The object type</param>
        /// <param name="Source">Source information</param>
        /// <returns>The mapping specified</returns>
        public IMapping this[Type Key, ISourceInfo Source] { get { return this[Key].FirstOrDefault(x => x.DatabaseConfigType == Source.Database.GetType()); } }
    }
}