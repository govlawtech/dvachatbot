﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;

namespace DVAESABot.QnAMaker
{
    public class QnaMakerKb
    {
        private readonly TelemetryClient _telemetry;
        private readonly Uri qnaBaseUri = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");

        private readonly string SubscriptionKey = ConfigurationManager.AppSettings["QnaSubscriptionKey"];

        public QnaMakerKb()
        {
            _telemetry = new TelemetryClient();
        }

        // Sample HTTP Request:
        // POST /knowledgebases/{KbId}/generateAnswer
        // Host: https://westus.api.cognitive.microsoft.com/qnamaker/v1.0
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

            if (result == null)
            {
                _telemetry.TrackTrace("QandAQuestionAnswerFound", new Dictionary<string, string>
                {
                    {"Question", question}
                });
                return null;
            }

            _telemetry.TrackTrace("QandAQuestionAnswerFound", new Dictionary<string, string>
            {
                {"Question", question},
                {"Answer", result.Answer}
            });

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
    }
}