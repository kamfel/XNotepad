using Moq;
using NLog;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Threading;

namespace XNotepad.Core.Tests.Threading
{
    public class JobRunnerTests
    {
        [Test]
        public async Task is_running()
        {
            JobRunner.Start(Mock.Of<Logger>());
            await JobRunner.Run(Mock.Of<IJob>(), new CancellationToken());
            await JobRunner.StopAsync();
        }
    }
}
