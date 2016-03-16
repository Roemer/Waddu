namespace Waddu.Win32.Types
{
    public enum SHFileOperationType : uint
    {
        /// <summary>
        /// Copies the files specified in the pFrom member to the location specified in the pTo member. 
        /// </summary>
        FO_COPY = 2,

        /// <summary>
        /// Deletes the files specified in pFrom.
        /// </summary>
        FO_DELETE = 3,

        /// <summary>
        /// Moves the files specified in pFrom to the location specified in pTo. 
        /// </summary>
        FO_MOVE = 1,

        /// <summary>
        /// Renames the file specified in pFrom. You cannot use this flag to rename multiple files with a single function call. Use FO_MOVE instead
        /// </summary>
        FO_RENAME = 4
    }
}
