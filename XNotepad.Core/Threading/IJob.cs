using System.Threading;
using System.Threading.Tasks;

namespace XNotepad.Core.Threading
{
    public interface IJob
    {
        Task RunAsync(CancellationToken ct);

        string GetJobDescription();
    }
}
