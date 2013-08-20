// *************************************************************************************************
//
// Horde3D .NET wrapper
// ----------------------------------
// Copyright (C) 2007 Martin Burkhard
//
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
// *************************************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Horde3DNET
{
    /// <summary>
    /// Separates native methods from managed code.
    /// </summary>
    internal static class NativeMethodsUtils
    {
        public const string UTILS_DLL = "Horde3DUtils.dll";
        
        // Utilities
        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool initOpenGL(int hDC);

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void releaseOpenGL();

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void swapBuffers();

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool loadResourcesFromDisk(string contentDir);

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool dumpMessages();

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void showText(string text, float x, float y, float size,
                                             uint layer, uint fontMatRes);

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void freeMem(IntPtr ptr);

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool createTGAImage(IntPtr pixels, uint width, uint height, uint bpp, out IntPtr outData, out uint outSize);

        [DllImport(UTILS_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint pickNode(float nwx, float nwy);
    }
}
