using System;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Threading;

namespace XNotepad.Core.Jobs
{
    public class PeriodicJob : IJob
    {
        private TimeSpan period;
        private Func<Task> onPeriodElapsed;
        private string actionDesc;

        public PeriodicJob(
            TimeSpan period,
            Func<Task> onPeriodElapsed,
            string actionDesc = null)
        {
            this.period = period;
            this.onPeriodElapsed = onPeriodElapsed;
            this.actionDesc = actionDesc;
        }

        public string GetJobDescription()
        {
            return $"Periodic job: {this.actionDesc}.";
        }

        public async Task RunAsync(CancellationToken ct)
        {
            // First wait assigned period rather than fire given action immediately.
            await Task.Delay(period);

            while (!ct.IsCancellationRequested)
            {
                // Fire action right after verifying if cancellation was requested. Prevents potential long waiting for background work.
                await onPeriodElapsed();
                await Task.Delay(period, ct);
            }
        }
    }
}
