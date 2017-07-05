using System;
using System.Linq;
using System.Threading.Tasks;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.UserData.SetValue(typeof(ChatContext).Name, ChatContext.CreateEmpty());
            context.Call(new AzureSearchDialog(), ResumeAfterFactsheetChosen);
        }
        private async Task ResumeAfterFactsheetChosen(IDialogContext context, IAwaitable<string> result)
        {
            var chosenFactSheetTitle = await result;
            if (context.UserData.TryGetValue(typeof(ChatContext).Name,out ChatContext cc))
            {
                var factsheet =
                    cc.FactsheetShortlist.FirstOrDefault(f => f.FactSheet.FactsheetId == chosenFactSheetTitle);
                if (factsheet != null)
                {
                    var url = factsheet.FactSheet.Url;
                    context.Call(new QnAFactsheetDialog(chosenFactSheetTitle,url), LandingPad);
                }
            }
        }

        private async Task LandingPad(IDialogContext context, IAwaitable<string> result)
        {
            context.Call(new AzureSearchDialog(), ResumeAfterFactsheetChosen);
        }
     
    }
}