using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.QnaMaker;
using DVAESABot.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot.Dialogs
{
    public class CuratedQuestionsDialog : IDialog<Tuple<bool,string>>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(Resume); 
        }
        private async Task Resume(IDialogContext dialogContext, IAwaitable<IMessageActivity> message)
        {
            
            var text = (await message).Text;
            var qnaClient = new QnaMakerClient();
            var result = await qnaClient.SearchFaqAsync(text, KbId.TopQsKbId);
            if (result.Answers.Any(a => a.Score > 40D))
            {
                await dialogContext.SayAsync(result.Answers.First().Answer);
                dialogContext.Done(new Tuple<bool,string>(true,text));
            }
            dialogContext.Done(new Tuple<bool,string>(false,text));
        }
    }
}