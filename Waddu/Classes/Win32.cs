using System;
using System.Runtime.InteropServices;

namespace Waddu.Classes
{
    public class Win32
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public short fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);
        const int FO_DELETE = 3;
        const int FOF_ALLOWUNDO = 0x40;
        const int FOF_NOCONFIRMATION = 0x10; // Don't prompt for Dialogs

        public static void MoveFileToRecycleBin(string filePath)
        {
            SHFILEOPSTRUCT shf = new SHFILEOPSTRUCT();
            shf.wFunc = FO_DELETE;
            shf.fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION;
            shf.pFrom = filePath + "\0";

            SHFileOperation(ref shf); 
        }
    }
}
