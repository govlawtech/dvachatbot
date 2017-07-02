using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    /// <summary>
    ///     This class handles factsheet specific QnA processing. Code inspired by IgniteBot.
    /// </summary>
    [Serializable]
    public class QnAFactsheetDialog : IDialog<string>
    {
        private const string COUNT_OF_NO = "CountOfNos";

        // Maximum number of No's before finishing this dialog
        private const int MAX_NO_FOR_DIALOG = 3;

        private readonly List<string> _failureMessagesSelection = new List<string>
        {
            "Try asking something else or putting your question differently.",
            "Fan't find anything on that in this topic.  Ask something else?",
            "Sorry, don't know anything about that.  Try asking something else."
        };


        [NonSerialized] private QnaMakerKb _qnaMaker;

        public QnAFactsheetDialog()
        {
            _setup();
        }

        public async Task StartAsync(IDialogContext context)
        {
            ResetCountOfNos(context);
            string factsheetName;
            context.UserData.TryGetValue(DialogHelper.FACTSHEET_NAME, out factsheetName);

            await context.PostAsync($"Ask me a question about '{factsheetName}'.");
            context.Wait(MessageReceived);
        }

        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            _setup();
        }

        private void _setup()
        {
            _qnaMaker = new QnaMakerKb();
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;

            string kbId;
            context.UserData.TryGetValue(DialogHelper.KB_ID, out kbId);
            if (string.IsNullOrEmpty(kbId))
                kbId = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];

            var qnaResult = await _qnaMaker.SearchFaqAsync(message.Text, kbId);
            string replyContent;

            if (qnaResult == null || qnaResult.Score <= 10 ||
                qnaResult.Answer == "No good match found in the KB")
            {
                var random = new Random();
                replyContent = _failureMessagesSelection[random.Next(0, _failureMessagesSelection.Count - 1)];
            }
            else
            {
                replyContent = qnaResult.Answer;
            }

            await DialogHelper.PostMessageWithFeedback(context, replyContent);
            context.Wait(QnAFeedbackReceived);
        }

        // For processing QnA feedback from users
        public async Task QnAFeedbackReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await ProcessFeedbackInternal(context, item);
        }

        // Helper method for processing feedback
        private async Task ProcessFeedbackInternal(IDialogContext context, IAwaitable<IMessageActivity> item,
            bool launchQnA = false)
        {
            var reply = (Activity) await item;
            var message = $"Thanks for the feedback.";

            if (DialogHelper.POSITIVE_RESPONSES.Contains(reply.Text, StringComparer.OrdinalIgnoreCase))
            {
                message = "Great!";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                IncrementCountOfNos(context);
                await context.PostAsync(message);

                int countOfNos;
                context.UserData.TryGetValue(COUNT_OF_NO, out countOfNos);
                if (countOfNos >= 3)
                    context.Done(countOfNos.ToString());
                else
                    context.Wait(MessageReceived);
            }
        }

        // Increment the count of no's for this user
        private void IncrementCountOfNos(IDialogContext context)
        {
            int countOfNos;
            context.UserData.TryGetValue(COUNT_OF_NO, out countOfNos);

            countOfNos++;
            context.UserData.SetValue(COUNT_OF_NO, countOfNos);
        }

        // Reset the count of no's for this user
        private void ResetCountOfNos(IDialogContext context)
        {
            context.UserData.SetValue(COUNT_OF_NO, 0);
        }
    }
}