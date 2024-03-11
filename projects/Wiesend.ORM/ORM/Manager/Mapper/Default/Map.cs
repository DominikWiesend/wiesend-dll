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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.Aspect.Interfaces;
using Wiesend.ORM.Manager.Mapper.BaseClasses;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Mapper.Default
{
    /// <summary>
    /// Map class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public class Map<ClassType, DataType> : PropertyBase<ClassType, DataType, Map<ClassType, DataType>>, IMap
        where ClassType : class
        where DataType : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the Map</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        public Map([NotNull] Expression<Func<ClassType, DataType>> Expression, [NotNull] IMapping Mapping)
            : base(Expression, Mapping)
        {
            if (Expression == null) throw new ArgumentNullException(nameof(Expression));
            if (Mapping == null) throw new ArgumentNullException(nameof(Mapping));
            if (typeof(DataType).Is(typeof(IEnumerable)))
                throw new ArgumentException("Expression is an IEnumerable, use ManyToOne or ManyToMany instead");
            SetTableName(Mapping.TableName);
            SetFieldName(typeof(DataType).Name + "_" + Name + "_ID");
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public override string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// Does a cascade delete of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        public override IBatch CascadeDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            var MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            var Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            var Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties.Where(x => x.Cascade))
                Batch.AddCommand(Property.CascadeDelete(Item, Source, ObjectsSeen.ToList()));
            Batch.AddCommand(Provider.Generate<DataType>(Source, PropertyMapping, Structure).Delete(Item));
            IoC.Manager.Bootstrapper.Resolve<DataTypes.Caching.Manager>().Cache().RemoveByTag(typeof(DataType).GetName());
            return Batch;
        }

        /// <summary>
        /// Called to create a batch that deletes items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        public override IBatch CascadeJoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            var MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            var Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            var Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties)
            {
                if (!Property.Cascade && (Property is ISingleMapping || Property is IMultiMapping))
                    Batch.AddCommand(Property.JoinsDelete(Item, Source, ObjectsSeen.ToList()));
                else if (Property.Cascade)
                    Batch.AddCommand(Property.CascadeJoinsDelete(Item, Source, ObjectsSeen.ToList()));
            }
            return Batch;
        }

        /// <summary>
        /// Called to create a batch that saves items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        public override IBatch CascadeJoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            var MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            var Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            var Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties)
            {
                if (!Property.Cascade && (Property is IMultiMapping || Property is ISingleMapping))
                    Batch.AddCommand(Property.JoinsSave(Item, Source, ObjectsSeen.ToList()));
                else if (Property.Cascade)
                    Batch.AddCommand(Property.CascadeJoinsSave(Item, Source, ObjectsSeen.ToList()));
            }
            return Batch;
        }

        /// <summary>
        /// Does a cascade save of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "<Pending>")]
        public override IBatch CascadeSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            var MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            var Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            var Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties.Where(x => x.Cascade))
                Batch.AddCommand(Property.CascadeSave(Item, Source, ObjectsSeen.ToList()));
            Batch.AddCommand(((IProperty<DataType>)PropertyMapping.IDProperties.FirstOrDefault()).CascadeSave(Item, Source, ObjectsSeen.ToList()));
            IoC.Manager.Bootstrapper.Resolve<DataTypes.Caching.Manager>().Cache().RemoveByTag(typeof(DataType).GetName());
            return Batch;
        }

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        public override object GetParameter(object Object)
        {
            var Item = CompiledExpression(Object as ClassType);
            return ForeignMapping.IDProperties.FirstOrDefault().GetValue(Item);
        }

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        public override object GetParameter(Dynamo Object)
        {
            return ForeignMapping.IDProperties.FirstOrDefault().GetValue(GetValue(Object));
        }

        /// <summary>
        /// Called to create a batch that deletes items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch JoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            return Provider.Batch(Source);
        }

        /// <summary>
        /// Called to create a batch that saves items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch JoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            var Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            return Provider.Batch(Source);
        }

        /// <summary>
        /// Sets up the property
        /// </summary>
        /// <param name="MappingProvider">Mapping provider</param>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source info</param>
        public override void Setup(ISourceInfo Source, Mapper.Manager MappingProvider, QueryProvider.Manager QueryProvider)
        {
            ForeignMapping = MappingProvider[Type, Source];
            Structure = MappingProvider.GetStructure(Mapping.DatabaseConfigType);
            QueryProvider.Generate<ClassType>(Source, Mapping, Structure).SetupLoadCommands(this);
        }
    }
}