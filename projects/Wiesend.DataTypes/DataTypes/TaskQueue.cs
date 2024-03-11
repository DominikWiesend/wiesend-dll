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

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Class that helps with running tasks in parallel on a set of objects (that will come in on an
    /// ongoing basis, think producer/consumer situations)
    /// </summary>
    /// <typeparam name="T">Object type to process</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "<Pending>")]
    public class TaskQueue<T> : BlockingCollection<T>, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Capacity">
        /// Number of items that are allowed to be processed in the queue at one time
        /// </param>
        /// <param name="ProcessItem">Action that is used to process each item</param>
        /// <param name="HandleError">
        /// Handles an exception if it occurs (defaults to eating the error)
        /// </param>
        public TaskQueue(int Capacity, Action<T> ProcessItem, Action<Exception> HandleError = null)
            : base(new ConcurrentQueue<T>())
        {
            if (!(Capacity > 0)) throw new ArgumentException("Capacity must be greater than 0", nameof(Capacity));
            this.ProcessItem = ProcessItem;
            this.HandleError = HandleError.Check(x => { });
            this.CancellationToken = new CancellationTokenSource();
            Tasks = new Task[Capacity];
            Capacity.Times(x => Tasks[x] = Task.Factory.StartNew(Process));
        }

        /// <summary>
        /// Determines if it has been cancelled
        /// </summary>
        public bool IsCanceled
        {
            get { return CancellationToken.IsCancellationRequested; }
        }

        /// <summary>
        /// Determines if it has completed all tasks
        /// </summary>
        public bool IsComplete
        {
            get { return Tasks.All(x => x.IsCompleted); }
        }

        /// <summary>
        /// Token used to signal cancellation
        /// </summary>
        private CancellationTokenSource CancellationToken { get; set; }

        /// <summary>
        /// Called when an exception occurs when processing the queue
        /// </summary>
        private Action<Exception> HandleError { get; set; }

        /// <summary>
        /// Action used to process an individual item in the queue
        /// </summary>
        private Action<T> ProcessItem { get; set; }

        /// <summary>
        /// Group of tasks that the queue uses
        /// </summary>
        private Task[] Tasks { get; set; }

        /// <summary>
        /// Cancels the queue from processing
        /// </summary>
        /// <param name="Wait">
        /// Determines if the function should wait for the tasks to complete before returning
        /// </param>
        public void Cancel(bool Wait = false)
        {
            if (IsCompleted || IsCanceled)
                return;
            CancellationToken.Cancel(false);
            if (Wait)
                Task.WaitAll(Tasks);
        }

        /// <summary>
        /// Adds the item to the queue to be processed
        /// </summary>
        /// <param name="Item">Item to process</param>
        public void Enqueue(T Item)
        {
            if (!(!IsCompleted && !IsCanceled)) throw new InvalidOperationException("TaskQueue has been stopped");
            Add(Item);
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="disposing">
        /// True to dispose of all resources, false only disposes of native resources
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (Tasks != null)
            {
                Cancel(true);
                foreach (Task Task in Tasks)
                {
                    Task.Dispose();
                }
                Tasks = null;
            }
            if (CancellationToken != null)
            {
                CancellationToken.Dispose();
                CancellationToken = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Processes the queue
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private void Process()
        {
            if (CancellationToken == null) throw new NullReferenceException($"Condition {nameof(CancellationToken)} != null not met.");
            if (ProcessItem == null) throw new NullReferenceException($"Condition {nameof(ProcessItem)} != null not met.");
            while (true)
            {
                try
                {
                    ProcessItem(Take(CancellationToken.Token));
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }
            }
        }
    }
}