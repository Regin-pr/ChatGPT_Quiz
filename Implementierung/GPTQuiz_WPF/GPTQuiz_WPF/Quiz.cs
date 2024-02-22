using OpenAI_API.Chat;
using OpenAI_API.Models;

using OpenAI_API;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GPTQuiz_WPF
{
    public class Quiz
    {
        //KEY ENTFERNEN VOR DEM HOCHLADEN!!!
        OpenAIAPI api = new OpenAIAPI("API-KEY-HIER");

        //6. Funktion zum Auslagern nutzen
        public Task<ChatResult> AnfrageMachen(ChatMessage[] messages)
        {
            ChatRequest request = new ChatRequest();

            request.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
            request.Temperature = 0.1;
            request.MaxTokens = 100;
            request.Messages = messages;

            Task<ChatResult> result = api.Chat.CreateChatCompletionAsync(request);

            //result.Wait();
            
            return result;
        }
    }
}
