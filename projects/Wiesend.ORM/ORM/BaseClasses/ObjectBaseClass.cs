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
using System.Collections.Generic;
using System.Data;
using Wiesend.DataTypes;
using Wiesend.DataTypes.EventArgs;
using Wiesend.ORM.Interfaces;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.Random.DefaultClasses;
using Wiesend.Validation;

namespace Wiesend.ORM
{
    /// <summary>
    /// Object base class helper. This is not required but automatically sets up basic functions and
    /// properties to simplify things a bit.
    /// </summary>
    /// <typeparam name="IDType">ID type</typeparam>
    /// <typeparam name="ObjectType">Object type (must be the child object type)</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "<Pending>")]
    public abstract class ObjectBaseClass<ObjectType, IDType> : IComparable, IComparable<ObjectType>, IObject<IDType>
        where ObjectType : ObjectBaseClass<ObjectType, IDType>, new()
        where IDType : IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ObjectBaseClass()
        {
            Active = true;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Is the object active?
        /// </summary>
        [BoolGenerator]
        public virtual bool Active { get; set; }

        /// <summary>
        /// Date object was created
        /// </summary>
        [Between("1/1/1900", "1/1/2100")]
        [DateTimeGenerator("1/1/1900", "1/1/2100")]
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Date last modified
        /// </summary>
        [Between("1/1/1900", "1/1/2100")]
        [DateTimeGenerator("1/1/1900", "1/1/2100")]
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// Called when the object is deleted
        /// </summary>
        public EventHandler<DeletedEventArgs> Deleted { get; set; }

        /// <summary>
        /// Called prior to an object is deleting
        /// </summary>
        public EventHandler<DeletingEventArgs> Deleting { get; set; }

        /// <summary>
        /// ID for the object
        /// </summary>
        public virtual IDType ID { get; set; }

        /// <summary>
        /// Called prior to an object being loaded
        /// </summary>
        public EventHandler<LoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Called when the object is saved
        /// </summary>
        public EventHandler<SavedEventArgs> Saved { get; set; }

        /// <summary>
        /// Called prior to an object is saving
        /// </summary>
        public EventHandler<SavingEventArgs> Saving { get; set; }

        /// <summary>
        /// Called prior to an object is loading
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
        public static EventHandler<LoadingEventArgs> Loading;

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> All(params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            var E = new LoadingEventArgs();
            OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.All<ObjectType>(Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the items based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified items</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> All(string Command, CommandType Type, string ConnectionString, params object[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            var E = new LoadingEventArgs();
            OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.All<ObjectType>(Command, Type, ConnectionString, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the criteria specified
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public static ObjectType Any(params IParameter[] Params)
        {
            var instance = new ObjectType();
            var E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = QueryProvider.Any<ObjectType>(Params);
                instance?.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public static ObjectType Any(string Command, CommandType Type, string ConnectionString, params object[] Params)
        {
            var instance = new ObjectType();
            var E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = QueryProvider.Any<ObjectType>(Command, Type, ConnectionString, Params);
                instance?.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// != operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>returns true if they are not equal, false otherwise</returns>
        public static bool operator !=(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// The &lt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is less than the second, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator <(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() < second.GetHashCode();
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>true if the first and second item are the same, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator ==(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if ((object)first == null || (object)second == null)
                return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// The &gt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is greater than the second, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator >(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() > second.GetHashCode();
        }

        /// <summary>
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static int PageCount(int PageSize = 25, params IParameter[] Params)
        {
            return QueryProvider.PageCount<ObjectType>(PageSize, Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> Paged(int PageSize = 25, int CurrentPage = 0, string OrderBy = "", params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            var E = new LoadingEventArgs();
            OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.Paged<ObjectType>(PageSize, CurrentPage, OrderBy, Params);
                foreach (ObjectType Item in instance)
                    Item.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static void Save(IEnumerable<ObjectType> Objects)
        {
            if (Objects == null)
                return;
            Objects.ForEach(x => x.Save());
        }

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            if (obj is ObjectBaseClass<ObjectType, IDType>)
                return CompareTo((ObjectType)obj);
            return -1;
        }

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public virtual int CompareTo(ObjectType other)
        {
            return other.ID.CompareTo(ID);
        }

        /// <summary>
        /// Deletes the item
        /// </summary>
        public virtual void Delete()
        {
            var E = new DeletingEventArgs();
            OnDeleting(E);
            if (!E.Stop)
            {
                QueryProvider.Delete((ObjectType)this);
                var X = new DeletedEventArgs();
                OnDeleted(X);
            }
        }

        /// <summary>
        /// Determines if two items are equal
        /// </summary>
        /// <param name="obj">The object to compare this to</param>
        /// <returns>true if they are the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return obj.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// Returns the hash of this item
        /// </summary>
        /// <returns>the int hash of the item</returns>
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        /// <summary>
        /// Saves the item (if it already exists, it updates the item. Otherwise it inserts the item)
        /// </summary>
        public virtual void Save()
        {
            var E = new SavingEventArgs();
            OnSaving(E);

            if (!E.Stop)
            {
                SetupObject();
                this.Validate();
                QueryProvider.Save<ObjectType, IDType>((ObjectType)this);
                var X = new SavedEventArgs();
                OnSaved(X);
            }
        }

        /// <summary>
        /// Sets up the object for saving purposes
        /// </summary>
        public virtual void SetupObject()
        {
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        /// <param name="sender">Sender item</param>
        protected static void OnLoading(object sender, LoadingEventArgs e)
        {
            Loading.Raise(sender, e);
        }

        /// <summary>
        /// Called when the item is Deleted
        /// </summary>
        /// <param name="e">DeletedEventArgs item</param>
        protected virtual void OnDeleted(DeletedEventArgs e)
        {
            Deleted.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Deleting
        /// </summary>
        /// <param name="e">DeletingEventArgs item</param>
        protected virtual void OnDeleting(DeletingEventArgs e)
        {
            Deleting.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        protected virtual void OnLoaded(LoadedEventArgs e)
        {
            Loaded.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        protected virtual void OnLoading(LoadingEventArgs e)
        {
            Loading.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Saved
        /// </summary>
        /// <param name="e">SavedEventArgs item</param>
        protected virtual void OnSaved(SavedEventArgs e)
        {
            Saved.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Saving
        /// </summary>
        /// <param name="e">SavingEventArgs item</param>
        protected virtual void OnSaving(SavingEventArgs e)
        {
            Saving.Raise(this, e);
        }
    }
}