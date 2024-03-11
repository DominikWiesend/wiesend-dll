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

using System;
using Wiesend.DataTypes.Patterns;

namespace Wiesend.Workflow.Manager.Interfaces
{
    /// <summary>
    /// Workflow interface
    /// </summary>
    public interface IWorkflow : IFluentInterface
    {
        /// <summary>
        /// Gets the type of the data expected
        /// </summary>
        /// <value>
        /// The type of the data expected
        /// </value>
        Type DataType { get; }

        /// <summary>
        /// Gets the name of the workflow
        /// </summary>
        /// <value>
        /// The name of the workflow
        /// </value>
        string Name { get; }
    }

    /// <summary>
    /// Workflow interface
    /// </summary>
    /// <typeparam name="T">Data type expected</typeparam>
    public interface IWorkflow<T> : IWorkflow
    {
        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IWorkflow<T> And<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new();

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> And(IOperation<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> And(Func<T, T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        IWorkflow<T> And(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Adds an instance of the specified operation type to the workflow
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IWorkflow<T> Do<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new();

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Do(IOperation<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Do(Func<T, T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Appends the workflow specified to this workflow as an operation
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        IWorkflow<T> Do(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Called when an exception of the specified type is thrown in the workflow
        /// </summary>
        /// <typeparam name="ExceptionType">The exception type.</typeparam>
        /// <param name="Operation">The operation to run.</param>
        /// <returns>The resulting workflow object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        IWorkflow<T> On<ExceptionType>(Action<T> Operation)
            where ExceptionType : Exception;

        /// <summary>
        /// Repeats the last operation the specified number of times.
        /// </summary>
        /// <param name="Times">The number of times to repeat</param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Repeat(int Times = 1);

        /// <summary>
        /// Retries the last operation the specified number of times if it fails.
        /// </summary>
        /// <param name="Times">The number of times to retry.</param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Retry(int Times = 1);

        /// <summary>
        /// Starts the workflow with the specified data
        /// </summary>
        /// <param name="Data">The data to pass in to the workflow</param>
        /// <returns>The result from the workflow</returns>
        T Start(T Data);
    }
}