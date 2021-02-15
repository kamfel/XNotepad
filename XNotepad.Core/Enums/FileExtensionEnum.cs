using System.Collections.Generic;

namespace XNotepad.Core.Enums
{
    public enum FileExtensionEnum
    {
        Txt,

        All
    }

    public static class FileExtension
    {
        private const string Txt = "Text files (*.txt)|*.txt";

        private const string All = "All files (*.*)|*.*";

        private static Dictionary<FileExtensionEnum, string> dictionary = new Dictionary<FileExtensionEnum, string>()
        {
            { FileExtensionEnum.Txt, Txt },
            { FileExtensionEnum.All, All },
        };

        public static IReadOnlyDictionary<FileExtensionEnum, string> Dictionary => dictionary;
    }
}
