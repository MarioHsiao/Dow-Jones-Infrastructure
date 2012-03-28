// Type: VsWebSite.AssemblyReferences
// Assembly: VsWebSite.Interop, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: C:\Software\MSVS10\Common7\IDE\PublicAssemblies\VsWebSite.Interop.dll

using EnvDTE;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DowJones.VisualStudio.Nuget.Wizard.NugetInternal
{
	[Guid("2C264A1A-DBFB-43FE-9434-997B5BE0FCCC")]
	[TypeLibType(4160)]
	[ComImport]
	public interface AssemblyReferences : IEnumerable
	{
		[DispId(3)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		AssemblyReference Item([MarshalAs(UnmanagedType.Struct), In] object index);

		[DispId(-4)]
		[TypeLibFunc(1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new IEnumerator GetEnumerator();

		[DispId(4)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		AssemblyReference AddFromGAC([MarshalAs(UnmanagedType.BStr), In] string bstrAssemblyName);

		[DispId(5)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		AssemblyReference AddFromFile([MarshalAs(UnmanagedType.BStr), In] string bstrPath);

		[DispId(6)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddFromProject([MarshalAs(UnmanagedType.Interface), In] Project pProj);

		[DispId(0)]
		DTE DTE { [DispId(0)]
		get; }

		[DispId(1)]
		Project ContainingProject { [DispId(1)]
		get; }

		[DispId(2)]
		int Count { [DispId(2)]
		get; }
	}
}
