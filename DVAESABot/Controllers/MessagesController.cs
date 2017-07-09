using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using DVAESABot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DVAESABot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static string[] _greetings = {"hi", "hello"};
        
        /// <summary>
        ///     POST: api/Messages
        ///     Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
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
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
            }

            else
                HandleSystemMessage(activity);
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
                    var botId = WebConfigurationManager.AppSettings["BotId"];
                    if (update.MembersAdded.Any(ma => ma.Name == botId || ma.Name == "Bot")) // "Bot" is for emulator
                    {
                        var greeting =
                            message.CreateReply(
                                "Hello, I'm Chappie, DVA's <b>computerised, electronic</b> chat bot. See (<a href='https://dvachatbot.azurewebsites.net/privacy.html' target='_blank'>Privacy information</a>.)",
                                "en");
                        connector.Conversations.ReplyToActivityAsync(greeting)
                            .ContinueWith(task => Task.Delay(2000).Wait())
                            .ContinueWith(task =>
                            {
                                var instruction =
                                    message.CreateReply(
                                        "Please describe the <b>topic</b> you are interested in.");
                                connector.Conversations.ReplyToActivityAsync(instruction);
                            });
                    }
                }
            }
            return null;
        }
    }
}