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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Interfaces;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.Schema.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Schema
{
    /// <summary>
    /// Schema manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SchemaGenerators">The schema generators.</param>
        public Manager([NotNull] IEnumerable<ISchemaGenerator> SchemaGenerators)
        {
            if (SchemaGenerators == null) throw new ArgumentNullException(nameof(SchemaGenerators));
            this.SchemaGenerators = SchemaGenerators.ToDictionary(x => x.ProviderName);
        }

        /// <summary>
        /// Schema generators
        /// </summary>
        protected IDictionary<string, ISchemaGenerator> SchemaGenerators { get; private set; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="Source">Source to use</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, [NotNull] ISourceInfo Source)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));
            return SchemaGenerators.ContainsKey(Source.SourceType) ?
                SchemaGenerators.GetValue(Source.SourceType).GenerateSchema(DesiredStructure, Source) :
                new List<string>();
        }

        /// <summary>
        /// Sets up the specified databases
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="Database">The database.</param>
        /// <param name="Source">The source.</param>
        /// <param name="Structure">The structure.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public void Setup([NotNull] ListMapping<IDatabase, IMapping> Mappings, QueryProvider.Manager QueryProvider, IDatabase Database, ISourceInfo Source, Graph<IMapping> Structure)
        {
            if (Mappings == null) throw new NullReferenceException(nameof(Mappings));
            SchemaGenerators[Source.SourceType].Setup(Mappings, Database, QueryProvider, Structure);
        }

        /// <summary>
        /// Outputs the schema generator information as a string
        /// </summary>
        /// <returns>The schema generator information as a string</returns>
        public override string ToString()
        {
            return "Schema Providers: " + SchemaGenerators.OrderBy(x => x.Key).ToString(x => x.Key) + "\r\n";
        }
    }
}