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
using System.Globalization;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;

namespace Wiesend.ORM.Manager.QueryProvider.Default
{
    /// <summary>
    /// Command class
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">SQL Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="CallBack">Called when command has been executed</param>
        /// <param name="Object">Object</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public Command(Action<Command, IList<dynamic>> CallBack, object Object, string SQLCommand, CommandType CommandType, IParameter[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.CallBack = CallBack ?? ((x, y) => { });
            this.Object = Object;
            this.Parameters = Parameters.Check(new IParameter[0]);
            this.Finalizable = SQLCommand.Check("").ToUpperInvariant().Contains("SELECT");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">SQL Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="CallBack">Called when command has been executed</param>
        /// <param name="Object">Object</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1829:Use Length/Count property instead of Count() when available", Justification = "<Pending>")]
        public Command(Action<Command, IList<dynamic>> CallBack, object Object, string SQLCommand, CommandType CommandType, string ParameterStarter, object[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.Parameters = new List<IParameter>();
            this.CallBack = CallBack ?? ((x, y) => { });
            this.Object = Object;
            this.Finalizable = SQLCommand.Check("").ToUpperInvariant().Contains("SELECT");
            if (Parameters != null)
            {
                foreach (object Parameter in Parameters)
                {
                    var TempParameter = Parameter as string;
                    if (Parameter == null)
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), default(DbType), null, ParameterDirection.Input, ParameterStarter));
                    else if (TempParameter != null)
                        this.Parameters.Add(new StringParameter(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), TempParameter, ParameterDirection.Input, ParameterStarter));
                    else
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), Parameter, ParameterDirection.Input, ParameterStarter));
                }
            }
        }

        /// <summary>
        /// Call back
        /// </summary>
        public Action<Command, IList<dynamic>> CallBack { get; private set; }

        /// <summary>
        /// Command type
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Used to determine if Finalize should be called.
        /// </summary>
        public bool Finalizable { get; private set; }

        /// <summary>
        /// Object
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public object Object { get; private set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public ICollection<IParameter> Parameters { get; private set; }

        /// <summary>
        /// SQL command
        /// </summary>
        public string SQLCommand { get; set; }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Determines if the commands are equal</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "<Pending>")]
        public override bool Equals(object obj)
        {
            var OtherCommand = obj as Command;
            if (OtherCommand == null)
                return false;

            if (OtherCommand.SQLCommand != SQLCommand
                || OtherCommand.CommandType != CommandType
                || Parameters.Count != OtherCommand.Parameters.Count)
                return false;

            foreach (IParameter Parameter in Parameters)
                if (!OtherCommand.Parameters.Contains(Parameter))
                    return false;

            foreach (IParameter Parameter in OtherCommand.Parameters)
                if (!Parameters.Contains(Parameter))
                    return false;

            return true;
        }

        /// <summary>
        /// Called after the command is run
        /// </summary>
        /// <param name="Result">Result of the command</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public void Finalize(IList<dynamic> Result)
        {
            if (CallBack == null) throw new NullReferenceException("CallBack");
            CallBack(this, Result);
        }

        /// <summary>
        /// Returns the hash code for the command
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int ParameterTotal = 0;
                foreach (IParameter Parameter in Parameters)
                {
                    ParameterTotal += Parameter.GetHashCode();
                }
                if (ParameterTotal > 0)
                    return (SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode()) * 23 + ParameterTotal;
                return SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode();
            }
        }

        /// <summary>
        /// Returns the string representation of the command
        /// </summary>
        /// <returns>The string representation of the command</returns>
        public override string ToString()
        {
            var TempCommand = SQLCommand.Check("");
            Parameters.ForEach(x => { TempCommand = x.AddParameter(TempCommand); });
            return TempCommand;
        }
    }
}