using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XNotepad.Core.Threading
{
    public static class JobRunner
    {
        private static object _lock = new object();
        private static volatile bool IsRunning = false;

        private static List<Task> tasks = new List<Task>();
        private static List<CancellationTokenSource> tokenSources = new List<CancellationTokenSource>();
        private static Logger logger = LogManager.GetLogger(nameof(JobRunner));

        public static void Start(Logger logger)
        {
            lock (_lock)
            {
                JobRunner.logger = logger;

                IsRunning = true;
            }
            logger.Info("Job runner stared.");
        }

        public static Task Run(IJob job, CancellationToken ct)
        {
            if (!IsRunning)
            {
                logger.Warn("Job runner isn't running.");
                return Task.CompletedTask;
            }

            var task = Task.Run(async () =>
                {
                    try
                    {
                        logger.Info($"{job.GetJobDescription()} - started");
                        await job.RunAsync(ct);
                        logger.Info($"{job.GetJobDescription()} - finished");
                    }
                    catch (TaskCanceledException)
                    {
                        logger.Info($"{job.GetType().Name} was canceled.");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{job.GetType().Name} crashed.");
                        logger.Error(ex);
                    }
                }, ct);

            tasks.Add(task);
            return task;
        }

        public static async Task StopAsync()
        {
            logger.Info("Finishing background work...");

            IsRunning = false;
            await Task.WhenAll(tasks);

            logger.Info("Job runner stopped.");
        }
    }
}
