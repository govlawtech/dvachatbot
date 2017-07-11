using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVAESABot.Dialogs;
using DVAESABot.Domain;
using Microsoft.Bot.Builder.Dialogs;

namespace DVAESABot.ScheduledHeuristics.HeuristicQnAs
{
    public class MemberTypeHeuristicQnA : IHeuristicQnA<UserType>,IHeuristicQnA
    {
        
        public IDialog<UserType> Dialog => new UserTypeDialog();


        IDialog<object> IHeuristicQnA<object>.Dialog => new UserTypeDialog();

        public void ApplyResult(ChatContext chatContext, object dialogResult)
        {
            chatContext.User.UserType = dialogResult as UserType?;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            return !chatContext.User.UserType.HasValue;
        }
    }

    public class SeekingTreatmentQnA : IHeuristicQnA<bool>, IHeuristicQnA
    {
        private YesNoDialog _dialog;

        public SeekingTreatmentQnA()
        {
            _dialog = new YesNoDialog("Are you seeking treatment or rehabilitation for an injury or illness?");
        }

        IDialog<bool> IHeuristicQnA<bool>.Dialog => _dialog;

        public void ApplyResult(ChatContext chatContext, object dialogResult)
        {
            chatContext.User.SeekingTreatmentOrRehab = dialogResult as bool?;
        }

        public bool IsRelevant(ChatContext chatContext)
        {
            return chatContext.User.UserType == UserType.Member && !chatContext.User.SeekingTreatmentOrRehab.HasValue;
        }

        IDialog<object> IHeuristicQnA<object>.Dialog => _dialog;
     
    }
}