using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Runtime.CompilerServices;

namespace CrystalReportsApplication1
{
    [ComImport, TypeLibType((short)0x10d0), Guid("4A054EF8-20BE-47AD-9401-B58EE16EAA16"),
      InterfaceType(ComInterfaceType.InterfaceIsIDispatch )]
    public interface _exSQLHolder
    {
        [DispId(0x6803000d)]
        string OriginalSQL { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6803000d)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6803000d)] set; }
        [DispId(0x6803000c)]
        string NewSQL { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6803000c)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6803000c)] set; }
    }
    [ComImport, TypeLibType((short)2), Guid("7D0DDCF8-C3E0-4A93-BEE6-F45A09A9100B"), ClassInterface((short)0)]
    public class exSQLHolderClass
    {

    }
}
