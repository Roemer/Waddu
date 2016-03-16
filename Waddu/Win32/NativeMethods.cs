using System;
using System.Runtime.InteropServices;
using Waddu.Win32.Types;

namespace Waddu.Win32
{
    public class NativeMethods
    {
        /// <summary>
        /// Simple Check if we're running on a 64-Bit System
        /// </summary>
        public static bool Is64Bit()
        {
            var bits = IntPtr.Size * 8;
            return (bits == 64);
        }

        // Imports
        [DllImport("shell32.dll", EntryPoint = "SHFileOperation", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SHFileOperation32([In, Out] SHFILEOPSTRUCT lpFileOp);
        [DllImport("shell32.dll", EntryPoint = "SHFileOperation", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SHFileOperation64([In, Out] SHFILEOPSTRUCT64 lpFileOp);

        // Effective Function
        public static int SHFileOperation(SHFILEOPSTRUCT lpFileOp)
        {
            if (!Is64Bit())
            {
                return SHFileOperation32(lpFileOp);
            }
            var shfileopstruct1 = new SHFILEOPSTRUCT64();
            shfileopstruct1.hwnd = lpFileOp.hwnd;
            shfileopstruct1.wFunc = lpFileOp.wFunc;
            shfileopstruct1.pFrom = lpFileOp.pFrom;
            shfileopstruct1.pTo = lpFileOp.pTo;
            shfileopstruct1.fFlags = lpFileOp.fFlags;
            shfileopstruct1.fAnyOperationsAborted = lpFileOp.fAnyOperationsAborted;
            shfileopstruct1.hNameMappings = lpFileOp.hNameMappings;
            shfileopstruct1.lpszProgressTitle = lpFileOp.lpszProgressTitle;
            var ret = SHFileOperation64(shfileopstruct1);
            lpFileOp.fAnyOperationsAborted = shfileopstruct1.fAnyOperationsAborted;
            return ret;
        }

        // Helpers
        public static int MoveToRecycleBin(string filePath)
        {
            var shf = new SHFILEOPSTRUCT();
            shf.wFunc = SHFileOperationType.FO_DELETE;
            shf.fFlags = ShFileOperationFlags.FOF_ALLOWUNDO | ShFileOperationFlags.FOF_NOCONFIRMATION;
            shf.pFrom = filePath + "\0" + "\0";

            var ret = SHFileOperation(shf);
            return ret;
        }
    }
}
