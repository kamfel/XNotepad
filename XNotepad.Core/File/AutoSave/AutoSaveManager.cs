using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.File.AutoSave;
using XNotepad.Core.Jobs;
using XNotepad.Core.Threading;
using XNotepad.Core.Utilities;

namespace XNotepad.Core.File
{
    public class AutoSaveManager : IAutoSaveManager
    {
        private IFileManager fileManager;
        private DateTime lastSaveDateTime;

        private List<Task> periodicSaves;
        private List<CancellationTokenSource> cancellationTokenSources;
        private Dictionary<string, int> textChangesInFiles;

        public AutoSaveConfiguration Configuration { get; private set; }

        public bool IsEnabled { get; private set; }

        public bool IsPaused { get; set; }

        public AutoSaveManager(
            IFileManager fileManager,
            AutoSaveConfiguration configuration)
        {
            this.fileManager = fileManager;
            this.Configuration = configuration;

            this.IsEnabled = false;
            this.periodicSaves = new List<Task>();
            this.cancellationTokenSources = new List<CancellationTokenSource>();
            this.textChangesInFiles = new Dictionary<string, int>();

            this.lastSaveDateTime = DateTime.Now;
        }

        public void DocumentHasChanged(string prevFileId)
        {
            if (this.IsEnabled && this.Configuration.EveryFileChange)
            {
                this.fileManager.SaveDocument(prevFileId);
            }
        }

        public void TextHasChanged(string fileId, int changeLength)
        {
            if (this.IsEnabled && this.Configuration.EveryTextChange)
            {
                this.textChangesInFiles.TryAdd(fileId, 0);
                this.textChangesInFiles[fileId] += changeLength;

                if (this.textChangesInFiles[fileId] >= this.Configuration.TextChangeCount)
                {
                    this.fileManager.SaveDocument(fileId);
                    this.textChangesInFiles[fileId] = 0;
                }
            }
        }

        public async Task UpdateConfigurationAsync(AutoSaveConfiguration config)
        {
            await this.Disable();
            this.Configuration = config;
        }

        public void Enable()
        {
            if (this.IsEnabled)
            {
                return;
            }

            this.IsEnabled = true;

            if (this.Configuration.Inactivity)
            {
                var job = new PeriodicJob(
                    new TimeSpan(0, 0, 1),
                    this.PerformSaveOnIdle,
                    "Inactivity save");

                var cts = new CancellationTokenSource();
                cancellationTokenSources.Add(cts);
                JobRunner.Run(job, cts.Token);
            }

            if (this.Configuration.Periodically)
            {
                var job = new PeriodicJob(
                    this.Configuration.Period,
                    this.PerformPeriodicSave,
                    "Periodic save");

                var cts = new CancellationTokenSource();
                cancellationTokenSources.Add(cts);
                JobRunner.Run(job, cts.Token);
            }
        }

        public async Task Disable()
        {
            if (!this.IsEnabled)
            {
                return;
            }

            this.IsEnabled = false;

            foreach (var tokenSource in cancellationTokenSources)
            {
                tokenSource.Cancel();
            }

            await Task.WhenAll(periodicSaves);

            foreach (var tokenSource in cancellationTokenSources)
            {
                tokenSource.Dispose();
            }

            cancellationTokenSources.Clear();
            periodicSaves.Clear();
        }

        private Task PerformSaveOnIdle()
        {
            var idleTime = IdleTimeDetector.GetIdleTimeInfo();

            var isUserInactive = idleTime >= this.Configuration.InactivityPeriod;
            var isFileSaved = DateTime.Now - lastSaveDateTime < idleTime;

            if (isUserInactive && !isFileSaved && !this.IsPaused)
            {
                this.lastSaveDateTime = DateTime.Now;
                return this.fileManager.SaveAllDocuments();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private Task PerformPeriodicSave()
        {
            if (this.IsPaused)
            {
                return Task.CompletedTask;
            }
            else
            {
                return this.fileManager.SaveAllDocuments();
            }
        }
    }
}
