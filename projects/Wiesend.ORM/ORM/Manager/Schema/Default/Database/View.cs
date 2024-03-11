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

using System.Data;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.Schema.BaseClasses;
using Wiesend.ORM.Manager.Schema.Enums;
using Wiesend.ORM.Manager.Schema.Interfaces;

namespace Wiesend.ORM.Manager.Schema.Default.Database
{
    /// <summary>
    /// View class
    /// </summary>
    public class View : TableBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Source">Source</param>
        public View(string Name, ISource Source)
            : base(Name, Source)
        {
        }

        /// <summary>
        /// Definition of the view
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Adds a column
        /// </summary>
        /// <param name="ColumnName">Column Name</param>
        /// <param name="ColumnType">Data type</param>
        /// <param name="Length">Data length</param>
        /// <param name="Nullable">Nullable?</param>
        /// <param name="Identity">Identity?</param>
        /// <param name="Index">Index?</param>
        /// <param name="PrimaryKey">Primary key?</param>
        /// <param name="Unique">Unique?</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OnDeleteCascade">On Delete Cascade</param>
        /// <param name="OnUpdateCascade">On Update Cascade</param>
        /// <param name="OnDeleteSetNull">On Delete Set Null</param>
        /// <typeparam name="T">Column type</typeparam>
        public override IColumn AddColumn<T>(string ColumnName, DbType ColumnType, int Length = 0, bool Nullable = true, bool Identity = false, bool Index = false, bool PrimaryKey = false, bool Unique = false, string ForeignKeyTable = "", string ForeignKeyColumn = "", T DefaultValue = default, bool OnDeleteCascade = false, bool OnUpdateCascade = false, bool OnDeleteSetNull = false)
        {
            return Columns.AddAndReturn(new Column<T>(ColumnName, ColumnType, Length, Nullable, Identity, Index, PrimaryKey, Unique, ForeignKeyTable, ForeignKeyColumn, DefaultValue, OnDeleteCascade, OnUpdateCascade, OnDeleteSetNull, this));
        }

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        public override void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn)
        {
        }

        /// <summary>
        /// Adds a trigger to the table
        /// </summary>
        /// <param name="Name">Name of the trigger</param>
        /// <param name="Definition">Definition of the trigger</param>
        /// <param name="Type">Trigger type</param>
        /// <returns>The trigger specified</returns>
        public override ITrigger AddTrigger(string Name, string Definition, TriggerType Type)
        {
            return null;
        }
    }
}