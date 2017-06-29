using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using DVAESABot.Heuristics;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// Simple dialog for processing yes/no heuristic questions.
    /// </summary>
    [Serializable]
    public class HeuristicYesNoDialog : IDialog<string>
    {
        public const string HEURISTIC_NAME = "HeuristicName";

        public async Task StartAsync(IDialogContext context)
        {
            await DialogHelper.PostMessage(context, $"{GetHeuristic(context).defaultQuestion()}");
            context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var reply = (Activity)await item;

            IHeuristic heuristic = GetHeuristic(context);

            // Apply the heuristic to the user yes/no response
            List<string> result = heuristic.applyHeuristic(
                DialogHelper.POSITIVE_RESPONSES.Contains(reply.Text, StringComparer.OrdinalIgnoreCase),
                DialogHelper.GetFactsheetShortlist(context));

            DialogHelper.SetFactsheetShortlist(context, result);

            context.Done(heuristic.name());
        }

        // Helper method for getting the current heuristic
        private static IHeuristic GetHeuristic(IDialogContext context)
        {
            string heuristicName;
            IHeuristic result = null;
            context.UserData.TryGetValue<string>(HEURISTIC_NAME, out heuristicName);

            // TODO Refactor this
            if (typeof(JobLinedUpHeuristic).Name.Equals(heuristicName, StringComparison.OrdinalIgnoreCase))
            {
                result = new JobLinedUpHeuristic();
            }
            else if (typeof(IsMemberHeuristic).Name.Equals(heuristicName, StringComparison.OrdinalIgnoreCase))
            {
                result = new IsMemberHeuristic();
            }
            else if (typeof(EnlistBefore1July2004Heuristic).Name.Equals(heuristicName, StringComparison.OrdinalIgnoreCase))
            {
                result = new EnlistBefore1July2004Heuristic();
            }
            else
            {
                throw new InvalidOperationException($"Unrecognised heuristic {heuristicName}");
            }

            return result;
        }
    }
}