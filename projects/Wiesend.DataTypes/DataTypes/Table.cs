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
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using System.Linq;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Holds an individual row
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnNames">Column names</param>
        /// <param name="ColumnValues">Column values</param>
        /// <param name="ColumnNameHash">Column name hash</param>
        public Row(Hashtable ColumnNameHash, string[] ColumnNames, [NotNull] params object[] ColumnValues)
        {
            if (ColumnValues == null) throw new ArgumentNullException(nameof(ColumnValues));
            this.ColumnNameHash = ColumnNameHash;
            this.ColumnNames = ColumnNames;
            this.ColumnValues = (object[])ColumnValues.Clone();
        }

        /// <summary>
        /// Column names
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Column names
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Column values
        /// </summary>
        public object[] ColumnValues { get; protected set; }

        /// <summary>
        /// Returns a column based on the column name specified
        /// </summary>
        /// <param name="ColumnName">Column name to search for</param>
        /// <returns>The value specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public object this[string ColumnName]
        {
            get
            {
                if (string.IsNullOrEmpty(ColumnName)) throw new ArgumentNullException(nameof(ColumnName));
                if (ColumnNameHash == null) throw new NullReferenceException($"Condition {nameof(ColumnNameHash)} != null not met.");
                if (ColumnValues == null) throw new NullReferenceException($"Condition {nameof(ColumnValues)} != null not met.");
                var Column = (int)ColumnNameHash[ColumnName];//.PositionOf(ColumnName);
                if (Column <= -1)
                    throw new ArgumentOutOfRangeException(ColumnName + " is not present in the row");
                return this[Column];
            }
        }

        /// <summary>
        /// Returns a column based on the value specified
        /// </summary>
        /// <param name="Column">Column number</param>
        /// <returns>The value specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public object this[int Column]
        {
            get
            {
                if (!(Column >= 0)) throw new ArgumentOutOfRangeException(nameof(Column), $"Condition {nameof(Column)} >= 0 not met.");
                if (ColumnValues == null) throw new NullReferenceException($"Condition {nameof(ColumnValues)} != null not met.");
                if (ColumnValues.Length <= Column)
                    return null;
                return ColumnValues[Column];
            }
        }
    }

    /// <summary>
    /// Holds tabular information
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnNames">Column names</param>
        public Table([NotNull] params string[] ColumnNames)
        {
            if (ColumnNames == null) throw new ArgumentNullException(nameof(ColumnNames));
            this.ColumnNames = (string[])ColumnNames.Clone();
            this.Rows = new List<Row>();
            this.ColumnNameHash = new Hashtable();
            int x = 0;
            foreach (string ColumnName in ColumnNames)
            {
                if (!this.ColumnNameHash.ContainsKey(ColumnName))
                    this.ColumnNameHash.Add(ColumnName, x++);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Reader">Data reader to get the data from</param>
        public Table([NotNull] IDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            if (!(Reader.FieldCount >= 0)) throw new ArgumentOutOfRangeException(nameof(Reader), "Reader.FieldCount needs to have at least 0 fields");
            this.ColumnNames = new string[Reader.FieldCount];
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                this.ColumnNames[x] = Reader.GetName(x);
            }
            this.ColumnNameHash = new Hashtable();
            int y = 0;
            foreach (string ColumnName in ColumnNames)
            {
                if (!this.ColumnNameHash.ContainsKey(ColumnName))
                    this.ColumnNameHash.Add(ColumnName, y++);
            }
            this.Rows = new List<Row>();
            while (Reader.Read())
            {
                object[] Values = new object[ColumnNames.Length];
                for (int x = 0; x < Reader.FieldCount; ++x)
                {
                    Values[x] = Reader[x];
                }
                this.Rows.Add(new Row(this.ColumnNameHash, this.ColumnNames, Values));
            }
        }

        /// <summary>
        /// Column Name hash table
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Column names for the table
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Rows within the table
        /// </summary>
        public ICollection<Row> Rows { get; private set; }

        /// <summary>
        /// Gets a specific row
        /// </summary>
        /// <param name="RowNumber">Row number</param>
        /// <returns>The row specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public Row this[int RowNumber]
        {
            get
            {
                if (Rows == null) throw new NullReferenceException($"Condition {nameof(Rows)} != null not met.");
                return Rows.Count > RowNumber ? Rows.ElementAt(RowNumber) : null;
            }
        }

        /// <summary>
        /// Adds a row using the objects passed in
        /// </summary>
        /// <param name="Objects">Objects to create the row from</param>
        /// <returns>This</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public virtual Table AddRow([NotNull] params object[] Objects)
        {
            if (Objects == null) throw new ArgumentNullException(nameof(Objects));
            if (Rows == null) throw new NullReferenceException($"Contract assertion not met: {nameof(Rows)} != null");
            this.Rows.Add(new Row(ColumnNameHash, ColumnNames, Objects));
            return this;
        }
    }
}