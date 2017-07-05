using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DVAESABot.Exceptions;
using Microsoft.ApplicationInsights;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace DVAESABot.QnAMaker
{
    public class QnaMakerKb
    {
        private readonly TelemetryClient _telemetry;
        private readonly Uri qnaBaseUri = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");

        private readonly string SubscriptionKey = ConfigurationManager.AppSettings["QnaSubscriptionKey"];

        public QnaMakerKb()
        {
            _telemetry = new TelemetryClient();
        }

        // Sample HTTP Request:
        // POST /knowledgebases/{KbId}/generateAnswer
        // Host: https://westus.api.cognitive.microsoft.com/qnamaker/v2.0
        // Ocp-Apim-Subscription-Key: {SubscriptionKey}
        // Content-Type: application/json
        // {"question":"hi"}
        public async Task<QnaMakerResult> SearchFaqAsync(string question, string kbId)
        {
            var responseString = string.Empty;

            var uri = new UriBuilder($"{qnaBaseUri}/knowledgebases/{kbId}/generateAnswer").Uri;

            var postBody = $"{{\"question\": \"{question}\"}}";

            //Send the POST request
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                responseString = await client.UploadStringTaskAsync(uri, postBody);
            }

            var result = ConvertResponseFromJson(responseString);
            
            return result;
        }

        private QnaMakerResult ConvertResponseFromJson(string responseString)
        {
            QnaMakerResult response;
            try
            {
                response = JsonConvert.DeserializeObject<QnaMakerResult>(responseString);
            }
            catch
            {
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }

            return response;
        }

        public async Task<Dictionary<string,string>> GetQuestionsAndAnswers(string kbId)
        {
            var uriToGetUriForKb = new UriBuilder($"{qnaBaseUri}/knowledgebases/{kbId}").Uri;

            using (var client = new HttpClient())
            {
                client.BaseAddress = uriToGetUriForKb;
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                var response = await client.GetAsync(uriToGetUriForKb);
                if (response.IsSuccessStatusCode)
                {
                    var knowledgeBaseDownloadUrl = (await response.Content.ReadAsStringAsync()).TrimStart('"')
                        .TrimEnd('"');
                    client.DefaultRequestHeaders.Clear();
                    var kbTsv = await client.GetAsync(knowledgeBaseDownloadUrl);
                    var kbTsvString = await kbTsv.Content.ReadAsStringAsync();
                    var questionsAndAnswers = GetQuestionsAndAnsnwersFromTsv(kbTsvString);
                    return questionsAndAnswers;
                }

                return new Dictionary<string, string>();
            }

        }

        private static Dictionary<string,string> GetQuestionsAndAnsnwersFromTsv(string tsv)
        {
            var blackListedQuestions = new[]
            {
                "hi",
                "disclaimer"
            };

            var questionsAndAnswers = new Dictionary<string,string>();
            using (var parser = new TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(tsv))))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("\t");
                parser.ReadLine(); // skip header
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (!blackListedQuestions.Contains(fields[0].ToLowerInvariant()))
                    {
                        questionsAndAnswers.Add(fields[0], fields[1]);
                    }
                }
            }
            return questionsAndAnswers;
        }
    }
}