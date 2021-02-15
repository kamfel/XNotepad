using System;

namespace XNotepad.Core.File.AutoSave
{
    public class AutoSaveConfiguration
    {
        public bool EveryTextChange { get; set; }
        public bool Periodically { get; set; }
        public bool EveryFileChange { get; set; }
        public bool Inactivity { get; set; }

        public int TextChangeCount { get; set; }
        public TimeSpan Period { get; set; }
        public TimeSpan InactivityPeriod { get; set; }
    }
}
