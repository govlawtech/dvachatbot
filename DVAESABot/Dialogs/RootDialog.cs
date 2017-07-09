using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.UserData.SetValue(typeof(ChatContext).Name, ChatContext.CreateEmpty());
            context.Call(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog);
        }

        private async Task ResumeAfterCuratedQuestionsDialog(IDialogContext context,
            IAwaitable<Tuple<bool,string>> answerWasDisplayed)
        {
            var answerShown = await answerWasDisplayed;
            if (answerShown.Item1)
                context.Call(new CuratedQuestionsDialog(), (dialogContext, result) => this.StartAsync(context));
            else
            {
                await context.Forward(new AzureSearchDialog(), ResumeAfterFactsheetChosen, new Activity() {Text = answerShown.Item2});
            }
        }
        private async Task ResumeAfterFactsheetChosen(IDialogContext context, IAwaitable<Tuple<bool,string>> result)
        {
            var r = await result;
            if (r.Item1)
            {
                var chosenFactSheetTitle = r.Item2;
                if (context.UserData.TryGetValue(typeof(ChatContext).Name, out ChatContext cc))
                {
                    var factsheet =
                        cc.FactsheetShortlist.FirstOrDefault(f => f.FactSheet.FactsheetId == chosenFactSheetTitle);
                    if (factsheet != null)
                    {
                        var url = factsheet.FactSheet.Url;
                        context.Call(new QnAFactsheetDialog(chosenFactSheetTitle, url), LandingPad);
                    }
                }
            }
            else
            {
                context.Call(new CuratedQuestionsDialog(), (dialogContext, i) => this.StartAsync(context));
            }
        }

        private async Task LandingPad(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(new CuratedQuestionsDialog(), ResumeAfterCuratedQuestionsDialog);
        }
     
    }
}