using System;
using System.Diagnostics;
using System.Threading.Tasks;
using XNotepad.Core.File;
using XNotepad.Core.File.AutoSave;
using XNotepad.Core.Interfaces;
using XNotepad.Core.UI;

namespace XNotepad.UI.ViewModel
{
    public class AutoSaveOptionsViewModel : DialogBaseViewModel
    {
        private IWindowManager windowManager;
        private IAutoSaveManager autoSaveManager;

        public override string Title => "Autosave options";

        public bool AutoSaveEnabled { get; set; }
        public bool EveryTextChange { get; set; }
        public bool Periodically { get; set; }
        public bool EveryFileChange { get; set; }
        public bool OnInactivity { get; set; }

        public int TextChangeCount { get; set; }
        public int PeriodMinutes { get; set; }
        public int PeriodSeconds { get; set; }
        public int InactivityPeriodMinutes { get; set; }
        public int InactivityPeriodSeconds { get; set; }

        public AutoSaveOptionsViewModel(
            IWindowManager windowManager,
            IAutoSaveManager autoSaveManager)
        {
            this.windowManager = windowManager;
            this.autoSaveManager = autoSaveManager;

            this.LoadCurrentConfiguration();

            base.AcceptCommand = new Command<ICloseable>(Accept, x => true, x => Trace.WriteLine(x));
            base.CancelCommand = new Command<ICloseable>(Cancel, x => true, x => Trace.WriteLine(x));
        }

        public void Accept(ICloseable closeable)
        {
            var config = new AutoSaveConfiguration()
            {
                EveryFileChange = this.EveryFileChange,
                EveryTextChange = this.EveryTextChange,
                TextChangeCount = this.TextChangeCount,
                Inactivity = this.OnInactivity,
                InactivityPeriod = new TimeSpan(0, this.InactivityPeriodMinutes, this.InactivityPeriodSeconds),
                Periodically = this.Periodically,
                Period = new TimeSpan(0, this.PeriodMinutes, this.PeriodSeconds),
            };

            this.windowManager.DoWorkWithProgress(
                "Finishing save operations...",
                async () =>
                {
                    await autoSaveManager.UpdateConfigurationAsync(config);
                    autoSaveManager.Enable();
                });

            closeable.Close();
        }

        public void Cancel(ICloseable closeable)
        {
            closeable.Close();
        }

        private void LoadCurrentConfiguration()
        {
            this.AutoSaveEnabled = this.autoSaveManager.IsEnabled;

            if (this.autoSaveManager.Configuration != null)
            {
                this.EveryFileChange = this.autoSaveManager.Configuration.EveryFileChange;
                this.EveryTextChange = this.autoSaveManager.Configuration.EveryTextChange;
                this.TextChangeCount = this.autoSaveManager.Configuration.TextChangeCount;
                this.Periodically = this.autoSaveManager.Configuration.Periodically;
                this.PeriodMinutes = this.autoSaveManager.Configuration.Period.Minutes;
                this.PeriodSeconds = this.autoSaveManager.Configuration.Period.Seconds;
                this.OnInactivity = this.autoSaveManager.Configuration.Inactivity;
                this.InactivityPeriodMinutes = this.autoSaveManager.Configuration.InactivityPeriod.Minutes;
                this.InactivityPeriodSeconds = this.autoSaveManager.Configuration.InactivityPeriod.Seconds;
            }
        }
    }
}
