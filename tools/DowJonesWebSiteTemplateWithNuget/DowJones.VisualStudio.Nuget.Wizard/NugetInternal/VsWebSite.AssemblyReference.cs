// Type: VsWebSite.AssemblyReference
// Assembly: VsWebSite.Interop, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: C:\Software\MSVS10\Common7\IDE\PublicAssemblies\VsWebSite.Interop.dll

using EnvDTE;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DowJones.VisualStudio.Nuget.Wizard.NugetInternal
{
	[TypeLibType(4160)]
	[Guid("229F3491-6E60-4E50-90E5-7DB14B0DC004")]
	[ComImport]
	public interface AssemblyReference
	{
		[DispId(3)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Remove();

		[DispId(0)]
		DTE DTE { [DispId(0)]
		get; }

		[DispId(1)]
		Project ContainingProject { [DispId(1)]
		get; }

		[DispId(2)]
		string Name { [DispId(2)]
		get; }

		[DispId(4)]
		AssemblyReferenceType ReferenceKind { [DispId(4)]
		get; }

		[DispId(5)]
		string FullPath { [DispId(5)]
		get; }

		[DispId(6)]
		string StrongName { [DispId(6)]
		get; }

		[DispId(7)]
		Project ReferencedProject { [DispId(7)]
		get; }
	}

    [Guid("0F2BB482-5EDE-45A8-979D-13700E2B2520")]
    public enum AssemblyReferenceType
    {
        AssemblyReferenceBin,
        AssemblyReferenceConfig,
        AssemblyReferenceClientProject,
    }
}
