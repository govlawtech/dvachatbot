using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DVAESABot.Utilities
{
    /// <summary>
    /// Logs all incoming and outgoing bot messages to trace logs.
    /// </summary>
    public class FileActivityLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            Trace.TraceInformation($"From:{activity.From.Id} - To:{activity.Recipient.Id} - Message:{activity.AsMessageActivity()?.Text}\r\n");
        }
    }
}