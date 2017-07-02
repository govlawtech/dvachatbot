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
        // TODO: Replace in future with LUIS
        private static string[] _greetings = { "hi", "hello" };

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                if (_greetings.Contains(activity.Text, StringComparer.OrdinalIgnoreCase))
                {
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    var replyMessage = activity.CreateReply("Hello!", "en");
                    await connector.Conversations.ReplyToActivityAsync(replyMessage);
                }
                else
                {
                    // Funnelling with LUIS - improved from user testing 1/6
                    //await Conversation.SendAsync(activity, () => new LuisMRCADialog());
                    // Funnelling with Azure Search - Stage 2
                    await Conversation.SendAsync(activity, () => new AzureSearchDialog());
                }
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
            if (message.Type == ActivityTypes.ConversationUpdate)
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
                            var replyMessage = message.CreateReply("Hello, I'm Dewey, DVA's virtual assistant.  I can help you with general veterans enquiries.  (<a href='https://dvachatbot.azurewebsites.net/privacy.html'>Privacy information</a>.)", "en");
                            connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                    }
                }
            }
            return null;
        }
    }
}