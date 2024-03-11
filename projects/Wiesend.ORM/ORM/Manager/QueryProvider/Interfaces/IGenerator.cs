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

using System.Collections.Generic;
using Wiesend.ORM.Manager.Mapper.Default;
using Wiesend.ORM.Manager.Mapper.Interfaces;

namespace Wiesend.ORM.Manager.QueryProvider.Interfaces
{
    /// <summary>
    /// Generator interface, used to generate commands
    /// </summary>
    /// <typeparam name="T">Class type to generate</typeparam>
    public interface IGenerator<T>
        where T : class
    {
        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch All(params IParameter[] Parameters);

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Limit">Max number of items to return</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch All(int Limit, params IParameter[] Parameters);

        /// <summary>
        /// Generates a batch that will get the first item that satisfies the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch Any(params IParameter[] Parameters);

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Object">Object to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch Delete(T Object);

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch Delete(IEnumerable<T> Objects);

        /// <summary>
        /// Generates a batch that will insert the data from the object
        /// </summary>
        /// <param name="Object">Object to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch Insert(T Object);

        /// <summary>
        /// Generates a batch that will insert the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch Insert(IEnumerable<T> Objects);

        /// <summary>
        /// Deletes items from the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IBatch JoinsDelete<P>(IProperty<T, P> Property, T Object);

        /// <summary>
        /// Saves items to the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <typeparam name="ItemType">Item type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IBatch JoinsSave<P, ItemType>(IProperty<T, P> Property, T Object);

        /// <summary>
        /// Generates a batch that will get the specific property for the object
        /// </summary>
        /// <typeparam name="P">Property type</typeparam>
        /// <param name="Object">Object to get the property for</param>
        /// <param name="Property">Property to get</param>
        /// <returns>Batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IBatch LoadProperty<P>(T Object, IProperty Property);

        /// <summary>
        /// Generates a batch that will get the number of pages for a given page size given the
        /// parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="PageSize">Page size</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch PageCount(int PageSize, params IParameter[] Parameters);

        /// <summary>
        /// Generates a batch that will get a specific page of data that satisfies the parameters specified
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch Paged(int PageSize, int CurrentPage, string OrderBy, params IParameter[] Parameters);

        /// <summary>
        /// Saves the object to the source
        /// </summary>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object to save</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IBatch Save<PrimaryKeyType>(T Object);

        /// <summary>
        /// Sets up the commands for the mapping
        /// </summary>
        /// <param name="Mapping">Mapping to set up</param>
        void SetupCommands(IMapping<T> Mapping);

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(Map<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a IEnumerableManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">IEnumerableManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(IEnumerableManyToOne<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ListManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ListManyToOne<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ListManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ListManyToMany<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ListManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(IListManyToMany<T, D> Property)
            where D : class;

        /// <summary>
        /// Setups the load commands.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="Property">The property.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(IListManyToOne<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ListManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ICollectionManyToMany<T, D> Property)
            where D : class;

        /// <summary>
        /// Setups the load commands.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="Property">The property.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ICollectionManyToOne<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ManyToMany<T, D> Property)
            where D : class;

        /// <summary>
        /// Sets up the default load command for a ManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        void SetupLoadCommands<D>(ManyToOne<T, D> Property)
            where D : class;

        /// <summary>
        /// Generates a batch that will update the data from the object
        /// </summary>
        /// <param name="Object">Object to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch Update(T Object);

        /// <summary>
        /// Generates a batch that will update the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        IBatch Update(IEnumerable<T> Objects);
    }
}