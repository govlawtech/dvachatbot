using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVAESABot.Utilities
{
    /// <summary>
    /// Logs all incoming and outgoing bot messages to Application Insights.
    /// </summary>
    public class AppInsightsActivityLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            var telemetry = new TelemetryClient();
            telemetry.TrackTrace($"{activity.AsMessageActivity()?.Text}", 
                SeverityLevel.Information,
                new Dictionary<string, string> {
                    { "from", activity.From.Id },
                    { "to", activity.Recipient.Id }
                });
        }
    }
}