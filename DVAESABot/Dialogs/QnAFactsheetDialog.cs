using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DVAESABot.Domain;
using DVAESABot.QnaMaker;
using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class QnAFactsheetDialog : IDialog<object>
    {
        private static readonly List<string> FailureMessagesSelection = new List<string>
        {
            "Don't have an answer to that within this topic specifically."
        };

        private static readonly string BACK = "⇦ Back";

        [NonSerialized] private static readonly Random _random = new Random();
        private readonly string _factSheetCode;

        private readonly string _factSheetTitle;
        private readonly string _factSheetUrl;
        private readonly string _kbId;


        [NonSerialized] private QnaMakerClient _qnaMaker;

        public QnAFactsheetDialog()
        {
        }

        public QnAFactsheetDialog(string factSheetTitle, string factSheetUrl)
        {
            _factSheetTitle = factSheetTitle;
            _factSheetUrl = factSheetUrl;
            _factSheetCode = DialogHelper.ExtractFactsheetCodeFromFactSheeTitle(factSheetTitle);
            _kbId = KbId.kbIDs[_factSheetCode];
            _qnaMaker = new QnaMakerClient();
        }

        public async Task StartAsync(IDialogContext context)
        {
            
            var questionsAndAnswers = await _qnaMaker.GetQuestionsAndAnswers(_kbId);

            if (questionsAndAnswers.Any())
            {
                if (questionsAndAnswers.ContainsKey("Purpose"))
                {
                    var outlineMessage = context.MakeMessage();
                    outlineMessage.Text = RemoveFactsheetReferenceFromPurposeSection(ReplaceDoubleNewLineWithMarkDown(questionsAndAnswers["Purpose"]));
                    await context.PostAsync(outlineMessage);
                }
                
                List<string> options = questionsAndAnswers.Keys.Where(q => q != "Purpose")
                    .Select(k => DialogHelper.Wrap(k,35))
                    .ToList();
                options.Add(BACK);
                
                

                PromptDialog.Choice(
                    context,
                    QnAQuestionReceived,
                    options,
                    "Pick one:",
                    "Try again:",
                    10
                    
                );
            }
            else
            {
                await context.SayAsync(
                    $"Try [this factsheet]({_factSheetUrl}) on DVA's website.");

                var cc = context.GetChatContextOrDefault();
                cc.FactsheetShortlist.DropFactsheetWithTitle(_factSheetTitle);
                context.SetChatContext(cc);
                context.Done(false);
            }
        }

        

        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            _qnaMaker = new QnaMakerClient();
        }


        private async Task QnAQuestionReceived(IDialogContext context, IAwaitable<string> activity)
        {
            string message = null;
            try
            {
                message = (await activity);
            }
            catch (TooManyAttemptsException e)
            {
                await context.SayAsync("Let's just go back.");
                context.Done(false);
            }
            if (message == BACK)
            {
                context.Done(true);
            }
            else
            {
                var qnaResult = await _qnaMaker.SearchFaqAsync(message, _kbId);
                string replyContent;

                if (qnaResult == null || !qnaResult.Answers.Any() ||
                    qnaResult.Answers.First().Answer == "No good match found in the KB")
                {
                    replyContent = FailureMessagesSelection[_random.Next(0, FailureMessagesSelection.Count)];
                    await context.PostAsync(replyContent);
                }
                else
                {
                    replyContent = qnaResult.Answers.First().Answer;
                    await context.PostAsync(replyContent);
                }

                context.Wait(async (dialogContext, result) =>
                {
                    var nextMessage = (await result).Text;
                    await QnAQuestionReceived(dialogContext, Awaitable.FromItem(nextMessage));
                });
            }

        }

        private static string RemoveFactsheetReferenceFromPurposeSection(string purposeText)
        {
            return purposeText.Replace("This Factsheet", "This topic");
        }

        private static string ReplaceDoubleNewLineWithMarkDown(string text)
        {
            return text.Replace(@"\n\n",@" ");
        }



    }
}