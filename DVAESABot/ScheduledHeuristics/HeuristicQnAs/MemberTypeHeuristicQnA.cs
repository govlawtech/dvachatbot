using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Dialogs;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.HeuristicQnAs
{
    public class MemberTypeHeuristicQnA : IHeuristicQnA<UserType>
    {
        
        public IDialog<UserType> Dialog => new UserTypeDialog();

        
        public void ApplyResult(ChatContext chatContext, object dialogResult)
        {
            chatContext.User.UserType = dialogResult as UserType?;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            return !chatContext.User.UserType.HasValue;
        }
    }
}