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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Interfaces;
using Wiesend.ORM.Manager.Mapper.Interfaces;

namespace Wiesend.ORM.Manager
{
    /// <summary>
    /// ORM Manager class
    /// </summary>
    public class ORMManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MapperProvider">The mapper provider.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="SchemaProvider">The schema provider.</param>
        /// <param name="SourceProvider">The source provider.</param>
        /// <param name="Databases">The databases.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "<Pending>")]
        public ORMManager([NotNull] Mapper.Manager MapperProvider, [NotNull] QueryProvider.Manager QueryProvider, [NotNull] Schema.Manager SchemaProvider, [NotNull] SourceProvider.Manager SourceProvider, [NotNull] IEnumerable<IDatabase> Databases)
        {
            if (MapperProvider == null) throw new ArgumentNullException(nameof(MapperProvider));
            if (QueryProvider == null) throw new ArgumentNullException(nameof(QueryProvider));
            if (SchemaProvider == null) throw new ArgumentNullException(nameof(SchemaProvider));
            if (SourceProvider == null) throw new ArgumentNullException(nameof(SourceProvider));
            if (Databases == null) throw new ArgumentNullException(nameof(Databases));
            this.Mappings = new ListMapping<IDatabase, IMapping>();
            this.MapperProvider = MapperProvider;
            this.QueryProvider = QueryProvider;
            this.SchemaProvider = SchemaProvider;
            this.SourceProvider = SourceProvider;
            SetupMappings(Databases);
            SortMappings();
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Update))
                this.SchemaProvider.Setup(Mappings, QueryProvider, Database, SourceProvider.GetSource(Database.GetType()), MapperProvider.GetStructure(Database.GetType()));
        }

        /// <summary>
        /// Mapper provider
        /// </summary>
        private Mapper.Manager MapperProvider { get; set; }

        /// <summary>
        /// Mappings associate with a source
        /// </summary>
        private ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// Query provider
        /// </summary>
        private QueryProvider.Manager QueryProvider { get; set; }

        /// <summary>
        /// Schema provider
        /// </summary>
        private Schema.Manager SchemaProvider { get; set; }

        /// <summary>
        /// Source provider
        /// </summary>
        private SourceProvider.Manager SourceProvider { get; set; }

        /// <summary>
        /// Outputs information from the manager
        /// </summary>
        /// <returns>String version of the ORMManager</returns>
        public override string ToString()
        {
            return "ORM Manager\r\n";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        private List<Vertex<IMapping>> FindStartingVertices(Graph<IMapping> graph)
        {
            return graph.Vertices.Where(x => x.IncomingEdges.Count == 0).ToList();
        }

        private IEnumerable<IMapping> KahnSort(Graph<IMapping> graph)
        {
            var ResultList = new List<IMapping>();
            var StartingNodes = FindStartingVertices(graph);
            while (StartingNodes.Count > 0)
            {
                var Vertex = StartingNodes.First();
                StartingNodes.Remove(Vertex);
                ResultList.AddIfUnique(Vertex.Data);
                foreach (Edge<IMapping> Edge in Vertex.OutgoingEdges.ToList())
                {
                    Vertex<IMapping> Sink = Edge.Sink;
                    Edge.Remove();
                    if (Sink.IncomingEdges.Count == 0)
                    {
                        ResultList.AddIfUnique(Sink.Data);
                        StartingNodes.AddIfUnique(Sink);
                    }
                }
            }
            return ResultList;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private void SetupMappings(IEnumerable<IDatabase> Databases)
        {
            if (MapperProvider == null) throw new NullReferenceException("MapperProvider");
            foreach (IMapping Mapping in MapperProvider)
                Mappings.Add(Databases.FirstOrDefault(x => x.GetType() == Mapping.DatabaseConfigType), Mapping);

            foreach (IDatabase Database in Mappings.Keys)
                foreach (IMapping Mapping in Mappings[Database])
                    Mapping.Setup(SourceProvider.GetSource(Database.GetType()), MapperProvider, QueryProvider);
        }

        private void SortMappings()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                var Order = 0;
                var Graph = MapperProvider.GetStructure(Database.GetType());
                foreach (var Mapping in KahnSort(Graph.Copy()).Reverse())
                {
                    Mapping.Order = Order;
                    ++Order;
                }
            }
        }
    }
}