using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs;
using DVAESABot.Dialogs;

namespace DVAESABot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                // Funnelling with LUIS - improved from user testing 1/6
                //await Conversation.SendAsync(activity, () => new LuisMRCADialog());
                // Funnelling with Azure Search - Stage 2
                await Conversation.SendAsync(activity, () => new AzureSearchDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                IConversationUpdateActivity update = message;
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var replyMessage = message.CreateReply("Hi, I am the DVA ESA chat bot.\n\nI can help you with MRCA related questions.\n\nTo start, just ask me a question or tell me about your situation.", "en");
                            connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                    }
                }   
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}