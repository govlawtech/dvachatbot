using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DVAESABot.Dialogs
{
    /// <summary>
    /// This class handles factsheet specific QnA processing. Code inspired by IgniteBot.
    /// </summary>
    [Serializable]
    public class QnAFactsheetDialog : IDialog<string>
    {
        [NonSerialized]
        private QnaMakerKb _qnaMaker;

        private const string COUNT_OF_NO = "CountOfNos";
        // Maximum number of No's before finishing this dialog
        private const int MAX_NO_FOR_DIALOG = 3;

        public QnAFactsheetDialog()
        {
            _setup();
        }

        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            try
            {
                _setup();
            }
            catch (Exception e)
            {
                // XXX: Code from IgniteBot
                var message = e.Message;
            }
        }

        void _setup()
        {
            _qnaMaker = new QnaMakerKb();
        }

        public async Task StartAsync(IDialogContext context)
        {
            ResetCountOfNos(context);

            string factsheetName;
            context.UserData.TryGetValue<string>(DialogHelper.FACTSHEET_NAME, out factsheetName);

            await context.PostAsync($"What would you like to know about {factsheetName}");
            context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;

            string kbId;
            context.UserData.TryGetValue<string>(DialogHelper.KB_ID, out kbId);
            if (String.IsNullOrEmpty(kbId))
            {
                kbId = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];
            }

            var qnaResult = await _qnaMaker.SearchFaqAsync(message.Text, kbId);
            string replyContent = "No good match found in the factsheet\n\n\n\n";

            if (qnaResult == null || qnaResult.Score <= 30 ||
                qnaResult.Answer == "No good match found in the KB")
            {
                // Nothing else to do
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
        private async Task ProcessFeedbackInternal(IDialogContext context, IAwaitable<IMessageActivity> item, bool launchQnA = false)
        {
            var reply = (Activity)await item;
            string message = $"Sorry to hear that. We will incorporate your feedback.";

            if (DialogHelper.POSITIVE_RESPONSES.Contains(reply.Text, StringComparer.OrdinalIgnoreCase))
            {
                message = "Great to hear!";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                IncrementCountOfNos(context);
                await context.PostAsync(message);

                int countOfNos;
                context.UserData.TryGetValue<int>(COUNT_OF_NO, out countOfNos);
                if (countOfNos >= 3)
                {
                    context.Done(countOfNos.ToString());
                }
                else
                {
                    context.Wait(MessageReceived);
                }
            }
        }

        // Increment the count of no's for this user
        private void IncrementCountOfNos(IDialogContext context)
        {
            int countOfNos;
            context.UserData.TryGetValue<int>(COUNT_OF_NO, out countOfNos);

            countOfNos++;
            context.UserData.SetValue<int>(COUNT_OF_NO, countOfNos);
        }

        // Reset the count of no's for this user
        private void ResetCountOfNos(IDialogContext context)
        {
            context.UserData.SetValue<int>(COUNT_OF_NO, 0);
        }
    }
}