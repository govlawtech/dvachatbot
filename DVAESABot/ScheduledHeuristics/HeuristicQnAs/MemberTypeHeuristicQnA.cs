using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DVAESABot.Dialogs;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.HeuristicQnAs
{
    public class MemberTypeHeuristicQnA : IHeuristicQnA<UserType>
    {
        public IDialog<UserType> Dialog => new UserTypeDialog();


        public void SetResult(ChatContext chatContext, UserType result)
        {
            chatContext.User.UserType = result;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            return !chatContext.User.UserType.HasValue &&
                   chatContext.FactsheetShortlist.GetCategories().Count() > 1;
        }

    }
}