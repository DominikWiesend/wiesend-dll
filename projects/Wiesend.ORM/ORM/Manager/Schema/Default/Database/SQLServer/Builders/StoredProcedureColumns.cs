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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Wiesend.ORM.Manager.Schema.Interfaces;

namespace Wiesend.ORM.Manager.Schema.Default.Database.SQLServer.Builders
{
    /// <summary>
    /// StoredProcedure column builder, gets info and does diffs for StoredProcedures
    /// </summary>
    public class StoredProcedureColumns : IBuilder
    {
        /// <summary>
        /// Fills the database.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="database">The database.</param>
        public void FillDatabase(IEnumerable<dynamic> values, Database database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (values == null || values.Count() == 0)
                return;
            foreach (dynamic Item in values)
            {
                SetupStoredProcedures(database.StoredProcedures.FirstOrDefault(x => x.Name == Item.Procedure), Item);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            if (batch == null)
                throw new ArgumentNullException(nameof(batch));
            batch.AddCommand(null, null, CommandType.Text, @"SELECT sys.procedures.name as [Procedure],sys.systypes.name as TYPE,sys.parameters.name as NAME,
sys.parameters.max_length as LENGTH,sys.parameters.default_value as [DEFAULT VALUE]
FROM sys.procedures
INNER JOIN sys.parameters on sys.procedures.object_id=sys.parameters.object_id
INNER JOIN sys.systypes on sys.systypes.xusertype=sys.parameters.system_type_id
WHERE sys.systypes.xusertype <> 256");
        }

        /// <summary>
        /// Setups the stored procedures.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="item">The item.</param>
        private static void SetupStoredProcedures(ITable storedProcedure, dynamic item)
        {
            if (storedProcedure == null)
                throw new ArgumentNullException(nameof(storedProcedure));
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            string Type = item.TYPE;
            string Name = item.NAME;
            int Length = item.LENGTH;
            if (Type == "nvarchar")
                Length /= 2;
            string Default = item.DEFAULT_VALUE;
            storedProcedure.AddColumn<string>(Name, Type.To<string, SqlDbType>().To(DbType.Int32), Length, DefaultValue: Default);
        }
    }
}