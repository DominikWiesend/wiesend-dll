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
using System.Data.OleDb;
using System.Globalization;
using Wiesend.DataTypes;
using Wiesend.IO.FileFormats.BaseClasses;

namespace Wiesend.IO.FileFormats
{
    /// <summary>
    /// Excel doc helper
    /// </summary>
    public class Excel : StringListFormatBase<Excel, List<string>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Excel()
        {
            ColumnNames = new List<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FilePath">FilePath</param>
        /// <param name="Sheet">Sheet to load</param>
        public Excel(string FilePath, string Sheet)
            : this()
        {
            Parse(FilePath, Sheet);
        }

        /// <summary>
        /// Names of each column
        /// </summary>
        public IList<string> ColumnNames { get; private set; }

        /// <summary>
        /// Gets the value based on the row and column name specified
        /// </summary>
        /// <param name="Value">Row to get</param>
        /// <param name="Name">Column name to look for</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public string this[int Value, string Name]
        {
            get
            {
                if (!(Value >= 0)) throw new ArgumentNullException(nameof(Value), "Value must be greater than or equal to 0");
                if (ColumnNames == null) throw new NullReferenceException("ColumnNames");
                return Records[Value][ColumnNames.IndexOf(Name)];
            }
        }

        /// <summary>
        /// Loads an excel doc/sheet
        /// </summary>
        /// <param name="Location">Location of the file to load</param>
        /// <param name="Sheet">Sheet of the document to load</param>
        /// <returns>The excel doc</returns>
        public static Excel Load(string Location, string Sheet)
        {
            return new Excel(Location, Sheet);
        }

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>A string containing the file information</returns>
        public override string ToString()
        {
            return ColumnNames.ToString(x => x, "\t")
                + System.Environment.NewLine
                + Records.ToString(x => x.ToString(y => y, "\t"), System.Environment.NewLine);
        }

        /// <summary>
        /// Loads data from the excel doc
        /// </summary>
        /// <param name="Location">Location of the file</param>
        /// <returns>The excel doc</returns>
        protected override Excel InternalLoad(string Location)
        {
            return Load(Location, "Sheet1");
        }

        /// <summary>
        /// Loads data from the excel doc
        /// </summary>
        /// <param name="Data">Data to load from</param>
        protected override void LoadFromData(string Data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Sheet">Sheet to parse</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        protected void Parse(string FilePath, string Sheet)
        {
            var ConnectionString = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"", FilePath);
            var Command = string.Format(CultureInfo.CurrentCulture, "select * from [{0}$]", Sheet);
            using var Adapter = new OleDbDataAdapter(Command, ConnectionString);
            using var ds = new DataSet();
            ds.Locale = CultureInfo.CurrentCulture;
            Adapter.Fill(ds, "something");
            int y = 0;
            foreach (DataColumn Column in ds.Tables["something"].Columns)
                ColumnNames.Add(Column.ColumnName);
            foreach (DataRow Row in ds.Tables["something"].Rows)
            {
                Records.Add(new List<string>());
                for (int x = 0; x < Row.ItemArray.Length; ++x)
                {
                    Records[y].Add(Row.ItemArray[x].ToString());
                }
                ++y;
            }
        }
    }
}