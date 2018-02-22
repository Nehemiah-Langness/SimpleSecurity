using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Simple Security")]
[assembly: AssemblyTitle("Encryption Common")]
[assembly: AssemblyDescription("Tools for encryption")]
[assembly: AssemblyCompany("Nehemiah Langness")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: InternalsVisibleTo("UnitTest")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ba3c021c-9f6b-445c-91b9-db2a131c2b5b")]

[assembly: AssemblyVersion("1.0.*")]
