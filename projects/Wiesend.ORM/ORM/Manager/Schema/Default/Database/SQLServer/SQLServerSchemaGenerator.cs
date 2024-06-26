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
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Wiesend.DataTypes;
using Wiesend.ORM.Interfaces;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.Schema.Default.Database.SQLServer.Builders;
using Wiesend.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Wiesend.ORM.Manager.Schema.Enums;
using Wiesend.ORM.Manager.Schema.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Schema.Default.Database.SQLServer
{
    /// <summary>
    /// SQL Server schema generator
    /// </summary>
    public class SQLServerSchemaGenerator : ISchemaGenerator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Provider">The provider.</param>
        /// <param name="SourceProvider">The source provider.</param>
        public SQLServerSchemaGenerator(QueryProvider.Manager Provider, SourceProvider.Manager SourceProvider)
        {
            this.Provider = Provider;
            this.SourceProvider = SourceProvider;
        }

        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        public string ProviderName { get { return "System.Data.SqlClient"; } }

        /// <summary>
        /// Query provider object
        /// </summary>
        protected QueryProvider.Manager Provider { get; private set; }

        /// <summary>
        /// Source provider object
        /// </summary>
        protected SourceProvider.Manager SourceProvider { get; private set; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="Source">Source to use</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, ISourceInfo Source)
        {
            var CurrentStructure = GetSourceStructure(Source);
            return BuildCommands(DesiredStructure, CurrentStructure).ToArray();
        }

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="Source">Source to use</param>
        /// <returns>The source structure</returns>
        public ISource GetSourceStructure(ISourceInfo Source)
        {
            var DatabaseName = Regex.Match(Source.Connection, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", "");
            var DatabaseSource = SourceProvider.GetSource(Regex.Replace(Source.Connection, "Initial Catalog=(.*?;)", ""));
            if (!SourceExists(DatabaseName, DatabaseSource))
                return null;
            var Temp = new Database(DatabaseName);
            var Batch = Provider.Batch(Source);
            IBuilder[] Builders = {
                new Tables(),
                new TableColumns(),
                new TableTriggers(),
                new TableForeignKeys(),
                new Views(),
                new StoredProcedures(),
                new StoredProcedureColumns(),
                new Functions()
            };
            Builders.ForEach(x => x.GetCommand(Batch));
            var Results = Batch.Execute();
            Builders.For(0, Builders.Length - 1, (x, y) => y.FillDatabase(Results[x], Temp));
            return Temp;
        }

        /// <summary>
        /// Sets up the specified database schema
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="Database">The database.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="Structure">The structure.</param>
        public void Setup(ListMapping<IDatabase, IMapping> Mappings, IDatabase Database, QueryProvider.Manager QueryProvider, Graph<IMapping> Structure)
        {
            var TempSource = SourceProvider.GetSource(Database.Name);
            var TempDatabase = new Schema.Default.Database.Database(Regex.Match(TempSource.Connection, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", ""));
            SetupTables(Mappings, Database, TempDatabase, Structure);
            SetupJoiningTables(Mappings, Database, TempDatabase);
            SetupAuditTables(Database, TempDatabase);

            foreach (ITable Table in TempDatabase.Tables)
            {
                Table.SetupForeignKeys();
            }
            var Commands = GenerateSchema(TempDatabase, SourceProvider.GetSource(Database.Name)).ToList();
            var Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
            for (int x = 0; x < Commands.Count; ++x)
            {
                if (Commands[x].ToUpperInvariant().Contains("CREATE DATABASE"))
                {
                    QueryProvider.Batch(SourceProvider.GetSource(Regex.Replace(SourceProvider.GetSource(Database.Name).Connection, "Initial Catalog=(.*?;)", ""))).AddCommand(null, null, CommandType.Text, Commands[x]).Execute();
                }
                else if (Commands[x].Contains("CREATE TRIGGER") || Commands[x].Contains("CREATE FUNCTION"))
                {
                    if (Batch.CommandCount > 0)
                    {
                        Batch.Execute();
                        Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
                    }
                    Batch.AddCommand(null, null, CommandType.Text, Commands[x]);
                    if (x < Commands.Count - 1)
                    {
                        Batch.Execute();
                        Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
                    }
                }
                else
                {
                    Batch.AddCommand(null, null, CommandType.Text, Commands[x]);
                }
            }
            Batch.Execute();
        }

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="Info">Source info to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool SourceExists(string Source, ISourceInfo Info)
        {
            return Exists("SELECT * FROM Master.sys.Databases WHERE name=@0", Source, Info);
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool StoredProcedureExists(string StoredProcedure, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.Procedures WHERE name=@0", StoredProcedure, Source);
        }

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TableExists(string Table, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.Tables WHERE name=@0", Table, Source);
        }

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TriggerExists(string Trigger, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.triggers WHERE name=@0", Trigger, Source);
        }

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ViewExists(string View, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.views WHERE name=@0", View, Source);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        private static IEnumerable<string> BuildCommands(ISource DesiredStructure, ISource CurrentStructure)
        {
            var Commands = new List<string>();
            DesiredStructure = DesiredStructure.Check(new Database(""));
            if (CurrentStructure == null)
                Commands.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'CREATE DATABASE {0}'",
                    DesiredStructure.Name));
            CurrentStructure = CurrentStructure.Check(new Database(DesiredStructure.Name));
            foreach (Table Table in DesiredStructure.Tables)
            {
                ITable CurrentTable = CurrentStructure[Table.Name];
                Commands.Add((CurrentTable == null) ? GetTableCommand(Table) : GetAlterTableCommand(Table, CurrentTable));
            }
            foreach (Table Table in DesiredStructure.Tables)
            {
                ITable CurrentTable = CurrentStructure[Table.Name];
                Commands.Add((CurrentTable == null) ? GetForeignKeyCommand(Table) : GetForeignKeyCommand(Table, CurrentTable));
                Commands.Add((CurrentTable == null) ? GetTriggerCommand(Table) : GetAlterTriggerCommand(Table, CurrentTable));
            }
            foreach (Function Function in DesiredStructure.Functions)
            {
                var CurrentFunction = (Function)CurrentStructure.Functions.FirstOrDefault(x => x.Name == Function.Name);
                Commands.Add(CurrentFunction != null ? GetAlterFunctionCommand(Function, CurrentFunction) : GetFunctionCommand(Function));
            }
            foreach (View View in DesiredStructure.Views)
            {
                var CurrentView = (View)CurrentStructure.Views.FirstOrDefault(x => x.Name == View.Name);
                Commands.Add(CurrentView != null ? GetAlterViewCommand(View, CurrentView) : GetViewCommand(View));
            }
            foreach (StoredProcedure StoredProcedure in DesiredStructure.StoredProcedures)
            {
                var CurrentStoredProcedure = (StoredProcedure)CurrentStructure.StoredProcedures.FirstOrDefault(x => x.Name == StoredProcedure.Name);
                Commands.Add(CurrentStoredProcedure != null ? GetAlterStoredProcedure(StoredProcedure, CurrentStoredProcedure) : GetStoredProcedure(StoredProcedure));
            }
            return Commands;
        }

        private static IEnumerable<string> GetAlterFunctionCommand([NotNull] Function Function, [NotNull] Function CurrentFunction)
        {
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (CurrentFunction == null) throw new ArgumentNullException(nameof(CurrentFunction));
            if (!(Function.Definition == CurrentFunction.Definition || !string.IsNullOrEmpty(Function.Definition))) throw new ArgumentException($"Condition not met: [{nameof(Function)}.Definition == Current{nameof(Function)}.Definition || !string.IsNullOrEmpty({nameof(Function)}.Definition)]", nameof(Function));
            var ReturnValue = new List<string>();
            if (Function.Definition != CurrentFunction.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP FUNCTION {0}'",
                    Function.Name));
                ReturnValue.Add(GetFunctionCommand(Function));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterStoredProcedure([NotNull] StoredProcedure StoredProcedure, [NotNull] StoredProcedure CurrentStoredProcedure)
        {
            if (StoredProcedure == null) throw new ArgumentNullException(nameof(StoredProcedure));
            if (CurrentStoredProcedure == null) throw new ArgumentNullException(nameof(CurrentStoredProcedure));
            if (!(StoredProcedure.Definition == CurrentStoredProcedure.Definition || !string.IsNullOrEmpty(StoredProcedure.Definition))) throw new ArgumentException($"Condition not met: [{nameof(StoredProcedure)}.Definition == Current{nameof(StoredProcedure)}.Definition || !string.IsNullOrEmpty({nameof(StoredProcedure)}.Definition)]", nameof(StoredProcedure));
            var ReturnValue = new List<string>();
            if (StoredProcedure.Definition != CurrentStoredProcedure.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP PROCEDURE {0}'",
                    StoredProcedure.Name));
                ReturnValue.Add(GetStoredProcedure(StoredProcedure));
            }
            return ReturnValue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        private static IEnumerable<string> GetAlterTableCommand([NotNull] Table Table, ITable CurrentTable)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException("Table.Columns");
            var ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                IColumn CurrentColumn = CurrentTable[Column.Name];
                string Command = "";
                if (CurrentColumn == null)
                {
                    Command = string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD {1} {2}",
                        Table.Name,
                        Column.Name,
                        Column.DataType.To(SqlDbType.Int).ToString());
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                                        "(MAX)" :
                                        "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                    {
                        var Precision = (Column.Length * 2).Clamp(38, 18);
                        Command += "(" + Precision.ToString(CultureInfo.InvariantCulture) + "," + Column.Length.Clamp(38, 0).ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    Command += "'";
                    ReturnValue.Add(Command);
                    foreach (IColumn ForeignKey in Column.ForeignKey)
                    {
                        Command = string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD FOREIGN KEY ({1}) REFERENCES {2}({3}){4}{5}{6}'",
                            Table.Name,
                            Column.Name,
                            ForeignKey.ParentTable.Name,
                            ForeignKey.Name,
                            Column.OnDeleteCascade ? " ON DELETE CASCADE" : "",
                            Column.OnUpdateCascade ? " ON UPDATE CASCADE" : "",
                            Column.OnDeleteSetNull ? " ON DELETE SET NULL" : "");
                        ReturnValue.Add(Command);
                    }
                }
                else if (CurrentColumn.DataType != Column.DataType
                    || (CurrentColumn.DataType == Column.DataType
                        && CurrentColumn.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        && CurrentColumn.Length != Column.Length
                        && CurrentColumn.Length.Between(0, 4000)
                        && Column.Length.Between(0, 4000)))
                {
                    Command = string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ALTER COLUMN {1} {2}",
                        Table.Name,
                        Column.Name,
                        Column.DataType.To(SqlDbType.Int).ToString());
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                                        "(MAX)" :
                                        "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                    {
                        var Precision = (Column.Length * 2).Clamp(38, 18);
                        Command += "(" + Precision.ToString(CultureInfo.InvariantCulture) + "," + Column.Length.Clamp(38, 0).ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    Command += "'";
                    ReturnValue.Add(Command);
                }
            }
            return ReturnValue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        private static IEnumerable<string> GetAlterTriggerCommand([NotNull] Table Table, ITable CurrentTable)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Triggers != null)) throw new ArgumentNullException("Table.Triggers");
            var ReturnValue = new List<string>();
            foreach (Trigger Trigger in Table.Triggers)
            {
                foreach (Trigger Trigger2 in CurrentTable.Triggers)
                {
                    string Definition1 = Trigger.Definition;
                    var Definition2 = Trigger2.Definition.Replace("Command0", "");
                    if (Trigger.Name == Trigger2.Name && string.Equals(Definition1, Definition2, StringComparison.OrdinalIgnoreCase))
                    {
                        ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'DROP TRIGGER {0}'",
                            Trigger.Name));
                        var Definition = Regex.Replace(Trigger.Definition, "-- (.*)", "");
                        ReturnValue.Add(Definition.Replace("\n", " ").Replace("\r", " "));
                        break;
                    }
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterViewCommand([NotNull] View View, [NotNull] View CurrentView)
        {
            if (View == null) throw new ArgumentNullException(nameof(View));
            if (CurrentView == null) throw new ArgumentNullException(nameof(CurrentView));
            if (!(View.Definition == CurrentView.Definition || !string.IsNullOrEmpty(View.Definition))) throw new ArgumentException($"Condition not met: [{nameof(View)}.Definition == Current{nameof(View)}.Definition || !string.IsNullOrEmpty({nameof(View)}.Definition)]", nameof(View));
            var ReturnValue = new List<string>();
            if (View.Definition != CurrentView.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP VIEW {0}'",
                    View.Name));
                ReturnValue.Add(GetViewCommand(View));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetForeignKeyCommand([NotNull] Table Table, ITable CurrentTable)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException(nameof(Table), $"Condition not met: [{nameof(Table)}.Columns != null]");
            var ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                IColumn CurrentColumn = CurrentTable[Column.Name];
                if (Column.ForeignKey.Count > 0
                    && (CurrentColumn == null || CurrentColumn.ForeignKey.Count != Column.ForeignKey.Count))
                {
                    foreach (IColumn ForeignKey in Column.ForeignKey)
                    {
                        var Command = string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD FOREIGN KEY ({1}) REFERENCES {2}({3})",
                            Column.ParentTable.Name,
                            Column.Name,
                            ForeignKey.ParentTable.Name,
                            ForeignKey.Name);
                        if (Column.OnDeleteCascade)
                            Command += " ON DELETE CASCADE";
                        if (Column.OnUpdateCascade)
                            Command += " ON UPDATE CASCADE";
                        if (Column.OnDeleteSetNull)
                            Command += " ON DELETE SET NULL";
                        Command += "'";
                        ReturnValue.Add(Command);
                    }
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetForeignKeyCommand([NotNull] Table Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException(nameof(Table), $"Condition not met: [{nameof(Table)}.Columns != null]");
            var ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                if (Column.ForeignKey.Count > 0)
                {
                    foreach (IColumn ForeignKey in Column.ForeignKey)
                    {
                        var Command = string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD FOREIGN KEY ({1}) REFERENCES {2}({3})",
                            Column.ParentTable.Name,
                            Column.Name,
                            ForeignKey.ParentTable.Name,
                            ForeignKey.Name);
                        if (Column.OnDeleteCascade)
                            Command += " ON DELETE CASCADE";
                        if (Column.OnUpdateCascade)
                            Command += " ON UPDATE CASCADE";
                        if (Column.OnDeleteSetNull)
                            Command += " ON DELETE SET NULL";
                        Command += "'";
                        ReturnValue.Add(Command);
                    }
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetFunctionCommand([NotNull] Function Function)
        {
            if (Function == null) throw new ArgumentNullException(nameof(Function));
            if (!(Function.Definition != null)) throw new ArgumentNullException(nameof(Function), $"Condition not met: [{nameof(Function)}.Definition != null]");
            var Definition = Regex.Replace(Function.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static IEnumerable<string> GetStoredProcedure([NotNull] StoredProcedure StoredProcedure)
        {
            if (StoredProcedure == null) throw new ArgumentNullException(nameof(StoredProcedure));
            if (string.IsNullOrEmpty(StoredProcedure.Definition)) throw new ArgumentNullException(nameof(StoredProcedure), $"Condition not met: [!string.IsNullOrEmpty({nameof(StoredProcedure)}.Definition)]");
            var Definition = Regex.Replace(StoredProcedure.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        private static IEnumerable<string> GetTableCommand([NotNull] Table Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException("Table.Columns");
            var ReturnValue = new List<string>();
            var Builder = new StringBuilder();
            Builder.Append("EXEC dbo.sp_executesql @statement = N'CREATE TABLE ").Append(Table.Name).Append('(');
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Builder.Append(Splitter).Append(Column.Name).Append(' ').Append(Column.DataType.To(SqlDbType.Int).ToString());
                if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                {
                    Builder.Append((Column.Length < 0 || Column.Length >= 4000) ?
                                    "(MAX)" :
                                    "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")");
                }
                else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                {
                    var Precision = (Column.Length * 2).Clamp(38, 18);
                    Builder.Append('(').Append(Precision).Append(',').Append(Column.Length.Clamp(38, 0)).Append(')');
                }
                if (!Column.Nullable)
                {
                    Builder.Append(" NOT NULL");
                }
                if (Column.Unique)
                {
                    Builder.Append(" UNIQUE");
                }
                if (Column.PrimaryKey)
                {
                    Builder.Append(" PRIMARY KEY");
                }
                if (!string.IsNullOrEmpty(Column.Default))
                {
                    Builder.Append(" DEFAULT ").Append(Column.Default.Replace("(", "").Replace(")", "").Replace("'", "''"));
                }
                if (Column.AutoIncrement)
                {
                    Builder.Append(" IDENTITY");
                }
                Splitter = ",";
            }
            Builder.Append(")'");
            ReturnValue.Add(Builder.ToString());
            int Counter = 0;
            foreach (IColumn Column in Table.Columns)
            {
                if (Column.Index && Column.Unique)
                {
                    ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'CREATE UNIQUE INDEX Index_{0}{1} ON {2}({3})'",
                        Column.Name,
                        Counter.ToString(CultureInfo.InvariantCulture),
                        Column.ParentTable.Name,
                        Column.Name));
                }
                else if (Column.Index)
                {
                    ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'CREATE INDEX Index_{0}{1} ON {2}({3})'",
                        Column.Name,
                        Counter.ToString(CultureInfo.InvariantCulture),
                        Column.ParentTable.Name,
                        Column.Name));
                }
                ++Counter;
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetTriggerCommand([NotNull] Table Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Triggers != null)) throw new ArgumentNullException(nameof(Table));
            var ReturnValue = new List<string>();
            foreach (Trigger Trigger in Table.Triggers.Cast<Trigger>())
            {
                var Definition = Regex.Replace(Trigger.Definition, "-- (.*)", "");
                ReturnValue.Add(Definition.Replace("\n", " ").Replace("\r", " "));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetViewCommand([NotNull] View View)
        {
            if (View == null) throw new ArgumentNullException(nameof(View));
            if (string.IsNullOrEmpty(View.Definition)) throw new ArgumentNullException(nameof(View));
            var Definition = Regex.Replace(View.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static ITable SetupAuditTables([NotNull] ITable Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            var AuditTable = new Schema.Default.Database.Table(Table.Name + "Audit", Table.Source);
            string IDName = Table.Columns.Any(x => string.Equals(x.Name, "ID", StringComparison.OrdinalIgnoreCase)) ? "AuditID" : "ID";
            AuditTable.AddColumn(IDName, DbType.Int32, 0, false, true, true, true, false, "", "", 0);
            AuditTable.AddColumn("AuditType", SqlDbType.NVarChar.To(DbType.Int32), 1, false, false, false, false, false, "", "", "");
            foreach (IColumn Column in Table.Columns)
                AuditTable.AddColumn(Column.Name, Column.DataType, Column.Length, Column.Nullable, false, false, false, false, "", "", "");
            return AuditTable;
        }

        private static void SetupAuditTables([NotNull] IDatabase Key, [NotNull] Schema.Default.Database.Database TempDatabase)
        {
            if (Key == null) throw new ArgumentNullException(nameof(Key));
            if (TempDatabase == null) throw new ArgumentNullException(nameof(TempDatabase));
            if (!(TempDatabase.Tables != null)) throw new ArgumentNullException(nameof(TempDatabase));
            if (!Key.Audit)
                return;
            var TempTables = new List<ITable>();
            foreach (ITable Table in TempDatabase.Tables)
            {
                TempTables.Add(SetupAuditTables(Table));
                SetupInsertUpdateTrigger(Table);
                SetupDeleteTrigger(Table);
            }
            TempDatabase.Tables.Add(TempTables);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1830:Prefer strongly-typed Append and Insert method overloads on StringBuilder", Justification = "<Pending>")]
        private static void SetupDeleteTrigger([NotNull] ITable Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException(nameof(Table));
            var Columns = new StringBuilder();
            var Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_D ON dbo.")
                .Append(Table.Name).Append(" FOR DELETE AS IF @@rowcount=0 RETURN")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append('(');
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",'D' FROM deleted");
            Table.AddTrigger(Table.Name + "_Audit_D", Builder.ToString(), TriggerType.Delete);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1830:Prefer strongly-typed Append and Insert method overloads on StringBuilder", Justification = "<Pending>")]
        private static void SetupInsertUpdateTrigger([NotNull] ITable Table)
        {
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Table.Columns != null)) throw new ArgumentNullException(nameof(Table));
            var Columns = new StringBuilder();
            var Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_IU ON dbo.")
                .Append(Table.Name).Append(" FOR INSERT,UPDATE AS IF @@rowcount=0 RETURN declare @AuditType")
                .Append(" char(1) declare @DeletedCount int SELECT @DeletedCount=count(*) FROM DELETED IF @DeletedCount=0")
                .Append(" BEGIN SET @AuditType='I' END ELSE BEGIN SET @AuditType='U' END")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append('(');
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",@AuditType FROM inserted");
            Table.AddTrigger(Table.Name + "_Audit_IU", Builder.ToString(), TriggerType.Insert);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private static void SetupJoiningTables([NotNull] ListMapping<IDatabase, IMapping> Mappings, IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            if (Mappings == null) throw new NullReferenceException(nameof(Mappings));
            foreach (IMapping Mapping in Mappings[Key].OrderBy(x => x.Order))
            {
                foreach (IProperty Property in Mapping.Properties)
                {
                    if (Property is IMap)
                    {
                        var MapMapping = Mappings[Key].FirstOrDefault(x => x.ObjectType == Property.Type);
                        foreach (IProperty IDProperty in MapMapping.IDProperties)
                        {
                            TempDatabase[Mapping.TableName].AddColumn(Property.FieldName,
                                IDProperty.Type.To(DbType.Int32),
                                IDProperty.MaxLength,
                                !Property.NotNull,
                                false,
                                Property.Index,
                                false,
                                false,
                                MapMapping.TableName,
                                IDProperty.FieldName,
                                "",
                                false,
                                false,
                                Mapping.Properties.Count(x => x.Type == Property.Type) == 1 && Mapping.ObjectType != Property.Type);
                        }
                    }
                    else if (Property is IMultiMapping || Property is ISingleMapping)
                    {
                        SetupJoiningTablesEnumerable(Mappings, Mapping, Property, Key, TempDatabase);
                    }
                }
            }
        }

        private static void SetupJoiningTablesEnumerable(ListMapping<IDatabase, IMapping> Mappings, IMapping Mapping, IProperty Property, IDatabase Key, [NotNull] Schema.Default.Database.Database TempDatabase)
        {
            if (TempDatabase == null) throw new ArgumentNullException(nameof(TempDatabase));
            if (!(TempDatabase.Tables != null)) throw new ArgumentNullException(nameof(TempDatabase));
            if (TempDatabase.Tables.FirstOrDefault(x => x.Name == Property.TableName) != null)
                return;
            var MapMapping = Mappings[Key].FirstOrDefault(x => x.ObjectType == Property.Type);
            if (MapMapping == null)
                return;
            if (MapMapping == Mapping)
            {
                TempDatabase.AddTable(Property.TableName);
                TempDatabase[Property.TableName].AddColumn("ID_", DbType.Int32, 0, false, true, true, true, false, "", "", "");
                TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperties.First().FieldName,
                    Mapping.IDProperties.First().Type.To(DbType.Int32),
                    Mapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    Mapping.TableName,
                    Mapping.IDProperties.First().FieldName,
                    "",
                    false,
                    false,
                    false);
                TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperties.First().FieldName + "2",
                    MapMapping.IDProperties.First().Type.To(DbType.Int32),
                    MapMapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    MapMapping.TableName,
                    MapMapping.IDProperties.First().FieldName,
                    "",
                    false,
                    false,
                    false);
            }
            else
            {
                TempDatabase.AddTable(Property.TableName);
                TempDatabase[Property.TableName].AddColumn("ID_", DbType.Int32, 0, false, true, true, true, false, "", "", "");
                TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperties.First().FieldName,
                    Mapping.IDProperties.First().Type.To(DbType.Int32),
                    Mapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    Mapping.TableName,
                    Mapping.IDProperties.First().FieldName,
                    "",
                    true,
                    false,
                    false);
                TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperties.First().FieldName,
                    MapMapping.IDProperties.First().Type.To(DbType.Int32),
                    MapMapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    MapMapping.TableName,
                    MapMapping.IDProperties.First().FieldName,
                    "",
                    true,
                    false,
                    false);
            }
        }

        private static void SetupProperties([NotNull] ITable Table, [NotNull] IMapping Mapping)
        {
            if (Mapping == null) throw new ArgumentNullException(nameof(Mapping));
            if (Table == null) throw new ArgumentNullException(nameof(Table));
            if (!(Mapping.IDProperties != null)) throw new ArgumentNullException(nameof(Mapping));
            Mapping.IDProperties
                   .ForEach(x =>
            {
                Table.AddColumn(x.FieldName,
                    x.Type.To(DbType.Int32),
                    x.MaxLength,
                    x.NotNull,
                    x.AutoIncrement,
                    x.Index,
                    true,
                    x.Unique,
                    "",
                    "",
                    "");
            });
            Mapping.Properties
                   .Where(x => !(x is IMultiMapping || x is ISingleMapping || x is IMap))
                   .ForEach(x =>
                   {
                       Table.AddColumn(x.FieldName,
                       x.Type.To(DbType.Int32),
                       x.MaxLength,
                       !x.NotNull,
                       x.AutoIncrement,
                       x.Index,
                       false,
                       x.Unique,
                       "",
                       "",
                       "");
                   });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private static void SetupTables([NotNull] ListMapping<IDatabase, IMapping> Mappings, IDatabase Key, Database TempDatabase, Graph<IMapping> Structure)
        {
            if (Mappings == null) throw new NullReferenceException(nameof(Mappings));
            foreach (IMapping Mapping in Mappings[Key].OrderBy(x => x.Order))
            {
                TempDatabase.AddTable(Mapping.TableName);
                SetupProperties(TempDatabase[Mapping.TableName], Mapping);
            }
            foreach (Vertex<IMapping> Vertex in Structure.Where(x => x.OutgoingEdges.Count > 0))
            {
                var Mapping = Vertex.Data;
                var ForeignMappings = Vertex.OutgoingEdges.Select(x => x.Sink.Data);
                foreach (var Property in Mapping.IDProperties)
                {
                    foreach (var ForeignMapping in ForeignMappings)
                    {
                        var ForeignProperty = ForeignMapping.IDProperties.FirstOrDefault(x => x.Name == Property.Name);
                        if (ForeignProperty != null)
                        {
                            TempDatabase[Mapping.TableName].AddForeignKey(Property.FieldName, ForeignMapping.TableName, ForeignProperty.FieldName);
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1829:Use Length/Count property instead of Count() when available", Justification = "<Pending>")]
        private bool Exists(string Command, string Value, [NotNull] ISourceInfo Source)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));
            if (Provider == null) throw new NullReferenceException("Provider");
            return Provider.Batch(Source)
                           .AddCommand(null, null, Command, CommandType.Text, Value)
                           .Execute()[0]
                           .Count() > 0;
        }
    }
}