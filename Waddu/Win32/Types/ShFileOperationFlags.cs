using System;

namespace Waddu.Win32.Types
{
    [Flags]
    public enum ShFileOperationFlags : ushort
    {
        /// <summary>
        /// Preserve Undo information, if possible. If pFrom does not contain fully qualified path and file names, this flag is ignored.
        /// </summary>
        FOF_ALLOWUNDO = 0x40,

        /// <summary>
        /// Not used.
        /// </summary>
        FOF_CONFIRMMOUSE = 2,

        /// <summary>
        /// Perform the operation on files only if a wildcard file name (*.*) is specified.
        /// </summary>
        FOF_FILESONLY = 0x80,

        /// <summary>
        /// The pTo member specifies multiple destination files (one for each source file) rather than one directory where all source files are to be deposited.
        /// </summary>
        FOF_MULTIDESTFILES = 1,

        /// <summary>
        /// Do not move connected files as a group. Only move the specified files.
        /// </summary>
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,

        /// <summary>
        /// Respond with "Yes to All" for any dialog box that is displayed.
        /// </summary>
        FOF_NOCONFIRMATION = 0x10,

        /// <summary>
        /// Do not confirm the creation of a new directory if the operation requires one to be created.
        /// </summary>
        FOF_NOCONFIRMMKDIR = 0x200,

        /// <summary>
        /// Do not copy the security attributes of the file.
        /// </summary>
        FOF_NOCOPYSECURITYATTRIBS = 0x800,

        /// <summary>
        /// Do not display a user interface if an error occurs.
        /// </summary>
        FOF_NOERRORUI = 0x400,

        /// <summary>
        /// Treat reparse points as objects, not containers. You must set _WIN32_WINNT to 5.01 or later to use this flag
        /// </summary>
        FOF_NORECURSEREPARSE = 0x8000,

        /// <summary>
        /// Only operate in the local directory. Don't operate recursively into subdirectories.
        /// </summary>
        FOF_NORECURSION = 0x1000,

        /// <summary>
        /// Give the file being operated on a new name in a move, copy, or rename operation if a file with the target name already exists.
        /// </summary>
        FOF_RENAMEONCOLLISION = 8,

        /// <summary>
        /// Do not display a progress dialog box.
        /// </summary>
        FOF_SILENT = 4,

        /// <summary>
        /// Display a progress dialog box but do not show the file names.
        /// </summary>
        FOF_SIMPLEPROGRESS = 0x100,

        /// <summary>
        /// If FOF_RENAMEONCOLLISION is specified and any files were renamed, assign a name mapping object containing their old and new names to the hNameMappings member.
        /// </summary>
        FOF_WANTMAPPINGHANDLE = 0x20,

        /// <summary>
        /// Send a warning if a file is being destroyed during a delete operation rather than recycled. This flag partially overrides FOF_NOCONFIRMATION.
        /// </summary>
        FOF_WANTNUKEWARNING = 0x4000
    }
}
