/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Interop;
using System.Runtime.InteropServices;

namespace DiskInfoToolkit.Vendors
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetControllerCountDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetControllerInfoDelegate(int id, out HPT_CONTROLLER_INFO info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetControllerInfoV2Delegate(int id, out HPT_CONTROLLER_INFO_V2 info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetControllerInfoV3Delegate(int id, out HPT_CONTROLLER_INFO_V3 info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetDeviceInfoDelegate(uint id, out HPT_LOGICAL_DEVICE_INFO info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetDeviceInfoV2Delegate(uint id, out HPT_LOGICAL_DEVICE_INFO_V2 info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetDeviceInfoV3Delegate(uint id, out HPT_LOGICAL_DEVICE_INFO_V3 info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetDeviceInfoV4Delegate(uint id, out HPT_LOGICAL_DEVICE_INFO_V4 info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptGetPhysicalDevicesDelegate([Out] uint[] ids, int maxCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate uint HptGetVersionDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptIdePassThroughDelegate(IntPtr header);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptIdePassThroughV2Delegate(IntPtr header);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptNvmePassThroughDelegate(IntPtr inBuffer, uint inSize, IntPtr outBuffer, uint outSize);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int HptScsiPassThroughDelegate(IntPtr inBuffer, uint inSize, IntPtr outBuffer, uint outSize);
}
