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
// =================================================================================
//    I wrote this source totally on my own, so this piece of code is 
//    totally mine, fuck yeah, I'm the best!
// =================================================================================
#endregion of where is the original source code
#region Licenses [MIT Licenses]
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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following set of attributes.
// Change these attribute values to modify the information associated with an assembly.
[assembly: CLSCompliant(true)]
[assembly: AssemblyTitle("Wiesend.IO")]
[assembly: AssemblyCompany("Dominik Wiesend Cloud (Solutions)")]
[assembly: AssemblyProduct("Dominik Wiesend's [Dynamic Link Library] - Wiesend.IO")]
[assembly: AssemblyDescription("Wiesend's Dynamic Link Library is a collection of reusable code that I've written, or found throughout my programming career. It includes code to help with tasks including encryption, file management, compression, serialization, email, image manipulation, SQL, various file formats (CSV, iCal, etc.), randomization, validation, various data types, reflection, code gen, events, code profiling, math related classes, etc.")]
[assembly: AssemblyCopyright("Copyright(c) 2018 Dominik Wiesend. All rights reserved.")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible to COM components. If
// you need to access a type in this assembly from COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("121edd48-84fa-41c8-8215-8a5794b0247c")]

// Version information for an assembly consists of the following four values:
// Major Version Minor Version Build Number Revision
// You can specify all the values or you can default the Build and Revision Numbers by using the '*'
// as shown below: [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]