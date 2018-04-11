using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following set of attributes.
// Change these attribute values to modify the information associated with an assembly.
[assembly: AssemblyTitle("Wiesend.Random")]
[assembly: AssemblyDescription("Wiesend's Dynamic Link Library is a collection of reusable code that I've written, or found throughout my programming career. It includes code to help with tasks including encryption, file management, compression, serialization, email, image manipulation, SQL, various file formats (CSV, iCal, etc.), randomization, validation, various data types, reflection, code gen, events, code profiling, math related classes, etc.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Dominik Wiesend Cooperation")]
[assembly: AssemblyProduct("Dominik Wiesend's [Dynamic Link Library] - Wiesend.Random")]
[assembly: AssemblyCopyright("Copyright(c) 2018 Dominik Wiesend. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

// Setting ComVisible to false makes the types in this assembly not visible to COM components. If
// you need to access a type in this assembly from COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("940a392d-0279-4dce-a5a0-9f22c0654c5f")]

// Version information for an assembly consists of the following four values:
//
// Major Version Minor Version Build Number Revision
//
// You can specify all the values or you can default the Build and Revision Numbers by using the '*'
// as shown below: [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]


[assembly: CLSCompliant(true)]