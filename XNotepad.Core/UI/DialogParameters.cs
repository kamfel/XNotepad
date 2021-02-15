using System.Collections.Generic;
using XNotepad.Core.Enums;

namespace XNotepad.Core.UI
{
    public class DialogParameters
    {
        public List<FileExtensionEnum> FileExtensions { get; set; } = new List<FileExtensionEnum>();
        public bool FileMustExist { get; set; }
    }
}
