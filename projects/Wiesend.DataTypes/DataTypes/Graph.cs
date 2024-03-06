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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Edge pointing from vertex source to vertex sink
    /// </summary>
    /// <typeparam name="T">Data type of the data</typeparam>
    public class Edge<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sink">The sink.</param>
        public Edge(Vertex<T> source, Vertex<T> sink)
        {
            Source = source;
            Sink = sink;
        }

        /// <summary>
        /// Gets the sink vertex.
        /// </summary>
        /// <value>The sink vertex.</value>
        public Vertex<T> Sink { get; private set; }

        /// <summary>
        /// Gets the source vertex
        /// </summary>
        /// <value>The source vertex</value>
        public Vertex<T> Source { get; private set; }

        /// <summary>
        /// Removes this edge from the sink and source vertices.
        /// </summary>
        /// <returns>This</returns>
        public Edge<T> Remove()
        {
            Sink.RemoveEdge(this);
            Source.RemoveEdge(this);
            Sink = null;
            Source = null;
            return this;
        }
    }

    /// <summary>
    /// Class used to represent a graph
    /// </summary>
    /// <typeparam name="T">The data type stored in the graph</typeparam>
    public class Graph<T> : IEnumerable<Vertex<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{T}"/> class.
        /// </summary>
        public Graph()
        {
            Vertices = new List<Vertex<T>>();
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public List<Vertex<T>> Vertices { get; private set; }

        /// <summary>
        /// Adds the edge.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sink">The sink.</param>
        /// <returns>The new edge</returns>
        public Edge<T> AddEdge(Vertex<T> source, Vertex<T> sink)
        {
            return source.AddOutgoingEdge(sink);
        }

        /// <summary>
        /// Adds the vertex.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The new vertex</returns>
        public Vertex<T> AddVertex(T data)
        {
            var ReturnValue = new Vertex<T>(data, this);
            Vertices.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>A copy of this graph</returns>
        public Graph<T> Copy()
        {
            var Result = new Graph<T>();
            foreach (var Vertex in Vertices)
            {
                Result.AddVertex(Vertex.Data);
            }
            foreach (var Vertex in Vertices)
            {
                var TempSource = Result.Vertices.First(x => x.Data.Equals(Vertex.Data));
                foreach (var Edge in Vertex.OutgoingEdges)
                {
                    var TempSink = Result.Vertices.First(x => x.Data.Equals(Edge.Sink.Data));
                    Result.AddEdge(TempSource, TempSink);
                }
            }
            return Result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Vertex<T>> GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Documentation", "CA1200:Avoid using cref tags with a prefix", Justification = "<Pending>")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        /// <summary>
        /// Removes the vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>This</returns>
        public Graph<T> RemoveVertex(Vertex<T> vertex)
        {
            vertex.Remove();
            return this;
        }
    }

    /// <summary>
    /// Vertex within the graph
    /// </summary>
    /// <typeparam name="T">Data type saved in the vertex</typeparam>
    public class Vertex<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="graph">The graph.</param>
        public Vertex(T data, Graph<T> graph)
        {
            Data = data;
            Graph = graph;
            IncomingEdges = new List<Edge<T>>();
            OutgoingEdges = new List<Edge<T>>();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets the incoming edges.
        /// </summary>
        /// <value>The incoming edges.</value>
        public List<Edge<T>> IncomingEdges { get; private set; }

        /// <summary>
        /// Gets the outgoing edges.
        /// </summary>
        /// <value>The outgoing edges.</value>
        public List<Edge<T>> OutgoingEdges { get; private set; }

        /// <summary>
        /// Gets or sets the graph.
        /// </summary>
        /// <value>The graph.</value>
        private Graph<T> Graph { get; set; }

        /// <summary>
        /// Adds an outgoing edge to the vertex specified
        /// </summary>
        /// <param name="sink">The sink.</param>
        /// <returns>The new edge</returns>
        public Edge<T> AddOutgoingEdge(Vertex<T> sink)
        {
            var ReturnValue = new Edge<T>(this, sink);
            OutgoingEdges.Add(ReturnValue);
            sink.IncomingEdges.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Removes all edges from this vertex and removes it from the graph.
        /// </summary>
        /// <returns>This</returns>
        public Vertex<T> Remove()
        {
            IncomingEdges.ForEach(x => x.Remove());
            OutgoingEdges.ForEach(x => x.Remove());
            IncomingEdges.Clear();
            OutgoingEdges.Clear();
            Graph.Vertices.Remove(this);
            return this;
        }

        /// <summary>
        /// Removes the edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>This</returns>
        public Vertex<T> RemoveEdge(Edge<T> edge)
        {
            if (edge.Sink == this)
                IncomingEdges.Remove(edge);
            else
                OutgoingEdges.Remove(edge);
            return this;
        }
    }
}