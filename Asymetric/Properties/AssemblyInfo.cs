using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Asymmetric Encryption")]
[assembly: AssemblyDescription("Tools for asymmetric encryption")]

[assembly: AssemblyCompany("Nehemiah Langness")]
[assembly: AssemblyProduct("Simple Security")]
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
[assembly: Guid("ebc94bce-872f-44e7-9a18-fb2c039066af")]

[assembly: AssemblyVersion("1.0.*")]
