using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Simple Security")]
[assembly: AssemblyTitle("Symmetric Encryption")]
[assembly: AssemblyDescription("Tools for symmetric encryption")]
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
[assembly: Guid("258076be-5be2-4f2a-b86b-34d8a0e4cd2e")]

[assembly: AssemblyVersion("1.0.*")]
