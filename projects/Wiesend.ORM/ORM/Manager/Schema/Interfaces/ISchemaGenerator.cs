﻿#region Project Description [About this]
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

using System.Collections.Generic;
using Wiesend.DataTypes;
using Wiesend.ORM.Interfaces;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Schema.Interfaces
{
    /// <summary>
    /// Schema generator interface
    /// </summary>
    public interface ISchemaGenerator
    {
        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="Source">Source to use</param>
        /// <returns>List of commands generated</returns>
        IEnumerable<string> GenerateSchema(ISource DesiredStructure, ISourceInfo Source);

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="Source">Source information</param>
        /// <returns>The source structure</returns>
        ISource GetSourceStructure(ISourceInfo Source);

        /// <summary>
        /// Sets up the specified database schema
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="Database">The database.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="Structure">The structure.</param>
        void Setup(ListMapping<IDatabase, IMapping> Mappings, IDatabase Database, QueryProvider.Manager QueryProvider, Graph<IMapping> Structure);

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="Info">Source info</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool SourceExists(string Source, ISourceInfo Info);

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="Source">Source information</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool StoredProcedureExists(string StoredProcedure, ISourceInfo Source);

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="Source">Source information</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool TableExists(string Table, ISourceInfo Source);

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="Source">Source information</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool TriggerExists(string Trigger, ISourceInfo Source);

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="Source">Source information</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool ViewExists(string View, ISourceInfo Source);
    }
}