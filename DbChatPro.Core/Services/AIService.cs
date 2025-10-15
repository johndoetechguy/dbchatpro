using Amazon.BedrockRuntime;
// ...existing code...
using Azure;
using Azure.AI.Inference;
// AI generated: using Azure.AI.OpenAI 2.0.0 stable API
using System.Collections.Generic;
using Azure.Identity;
using DBChatPro.Models;
using Microsoft.Extensions.AI;
using OpenAI;
using System.Text;
using System.Text.Json;
using System.ClientModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBChatPro.Services
{
    // AIService refactored by GitHub Copilot (AI generated)
    // AIService refactored by GitHub Copilot (AI generated)
    // AIService refactored by GitHub Copilot (AI generated)
    public class AIService(IConfiguration config, IServiceProvider serviceProvider)
    {
        // Hold the last used AI client and service type
        object aiClient;
        string aiClientType;

        public async Task<AIQuery> GetAISQLQuery(string aiModel, string aiService, string userPrompt, DatabaseSchema dbSchema, string databaseType)
        {
            if (aiClient == null || aiClientType != aiService)
            {
                aiClient = CreateChatClient(aiModel, aiService);
                aiClientType = aiService;
            }

    // AI generated: Use correct chat message types for each provider
    var chatHistory = new List<object>();
            var builder = new StringBuilder();
            var maxRows = config.GetValue<string>("MAX_ROWS");

            builder.AppendLine("Your are a helpful, cheerful database assistant. Do not respond with any information unrelated to databases or queries. Use the following database schema when creating your answers:");

            foreach(var table in dbSchema.SchemaRaw)
            {
                builder.AppendLine(table);
            }

            builder.AppendLine("Include column name headers in the query results.");
            builder.AppendLine("Always provide your answer in the JSON format below:");
            builder.AppendLine(@"{ ""summary"": ""your-summary"", ""query"":  ""your-query"" }");
            builder.AppendLine("Output ONLY JSON formatted on a single line. Do not use new line characters.");
            builder.AppendLine(@"In the preceding JSON response, substitute ""your-query"" with the database query used to retrieve the requested data.");
            builder.AppendLine(@"In the preceding JSON response, substitute ""your-summary"" with an explanation of each step you took to create this query in a detailed paragraph.");
            builder.AppendLine($"Only use {databaseType} syntax for database queries.");
            // builder.AppendLine($"Always limit the SQL Query to {maxRows} rows.");
            builder.AppendLine("Always include all of the table columns and details.");

            // Build the AI chat/prompts
            if (aiService == "AzureOpenAI" || aiService == "OpenAI")
            {
                // Use Azure.AI.Inference chat types
                chatHistory.Add(new ChatRequestSystemMessage(builder.ToString()));
                chatHistory.Add(new ChatRequestUserMessage(userPrompt));
            }
            else if (aiService == "Ollama")
            {
                // Use your existing ChatMessage type for Ollama
                chatHistory.Add(new DBChatPro.Models.ChatMessage("System", builder.ToString()));
                chatHistory.Add(new DBChatPro.Models.ChatMessage("User", userPrompt));
            }

            string responseContent = null;
            try
            {
                switch (aiService)
                {
                    case "AzureOpenAI":
                    case "OpenAI":
                    {
                        var endpoint = config.GetValue<string>("AZURE_OPENAI_ENDPOINT");
                        var key = config.GetValue<string>("AZURE_OPENAI_KEY");
                        var deployment = config.GetValue<string>("AZURE_OPENAI_DEPLOYMENT");
                        var client = new ChatCompletionsClient(new Uri(endpoint), new Azure.AzureKeyCredential(key));
                        var options = new ChatCompletionsOptions(chatHistory.Cast<ChatRequestMessage>().ToList());
                        options.Model = deployment;
                        var response = await client.CompleteAsync(options);
                        responseContent = response.Value.Content;
                        break;
                    }
                    case "Ollama":
                    {
                        var ollamaClient = aiClient as OllamaChatClient;
                        // Use your existing method for Ollama
                        var ollamaMessages = chatHistory.Cast<DBChatPro.Models.ChatMessage>()
                            .Select(m => new Microsoft.Extensions.AI.ChatMessage(
                                m.Role == "System" ? Microsoft.Extensions.AI.ChatRole.System :
                                m.Role == "User" ? Microsoft.Extensions.AI.ChatRole.User :
                                Microsoft.Extensions.AI.ChatRole.Assistant, m.Text)).ToList();
                        var ollamaResponse = await ollamaClient.GetResponseAsync(ollamaMessages);
                        responseContent = ollamaResponse.Messages[0].Text;
                        break;
                    }
                    case "GitHubModels":
                        throw new NotImplementedException("GitHubModels integration not implemented in this AI generated patch.");
                    case "AWSBedrock":
                    {
                        var bedrockClient = aiClient as AWSBedrockClient;
                        var bedrockMessages = chatHistory.Cast<DBChatPro.Models.ChatMessage>()
                            .Select(m => new Microsoft.Extensions.AI.ChatMessage(
                                m.Role == "System" ? Microsoft.Extensions.AI.ChatRole.System :
                                m.Role == "User" ? Microsoft.Extensions.AI.ChatRole.User :
                                Microsoft.Extensions.AI.ChatRole.Assistant, m.Text)).ToList();
                        var bedrockResponse = await bedrockClient.GetResponseAsync(bedrockMessages);
                        responseContent = bedrockResponse.Messages[0].Text;
                        break;
                    }
                    default:
                        throw new Exception($"Unknown AI service: {aiService}");
                }

                responseContent = responseContent?.Replace("```json", "").Replace("```", "").Replace("\\n", " ");
                return JsonSerializer.Deserialize<AIQuery>(responseContent);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to parse AI response as a SQL Query. The AI response was: {responseContent}\nError: {e.Message}");
            }
        }

        // AI generated: create the correct client for each AI provider
        // AI generated: Only Ollama and AWSBedrock need a client instance now
        private object CreateChatClient(string aiModel, string aiService)
        {
            switch (aiService)
            {
                case "Ollama":
                    return new OllamaChatClient(config.GetValue<string>("OLLAMA_ENDPOINT"), aiModel);
                case "AWSBedrock":
                    var bedrockClient = serviceProvider.GetRequiredService<IAmazonBedrockRuntime>();
                    return new AWSBedrockClient(bedrockClient, aiModel);
            }
            return null;
        }

        public async Task<string> ChatPrompt(List<DBChatPro.Models.ChatMessage> prompt, string aiModel, string aiService)
        {
            if (aiClient == null)
            {
                aiClient = CreateChatClient(aiModel, aiService);
            }

            switch (aiService)
            {
                case "AzureOpenAI":
                case "OpenAI":
                {
                    // Use Azure.AI.Inference
                    var endpoint = config.GetValue<string>("AZURE_OPENAI_ENDPOINT");
                    var key = config.GetValue<string>("AZURE_OPENAI_KEY");
                    var deployment = config.GetValue<string>("AZURE_OPENAI_DEPLOYMENT");
                    var client = new ChatCompletionsClient(new Uri(endpoint), new Azure.AzureKeyCredential(key));
                    var chatHistory = new List<ChatRequestMessage>();
                    foreach (var msg in prompt)
                    {
                        if (msg.Role == "System")
                            chatHistory.Add(new ChatRequestSystemMessage(msg.Text));
                        else if (msg.Role == "User")
                            chatHistory.Add(new ChatRequestUserMessage(msg.Text));
                        else
                            chatHistory.Add(new ChatRequestAssistantMessage(msg.Text));
                    }
                    var options = new ChatCompletionsOptions(chatHistory);
                    options.Model = deployment;
                    var response = await client.CompleteAsync(options);
                    return response.Value.Content;
                }
                case "Ollama":
                {
                    var ollamaClient = aiClient as OllamaChatClient;
                    var ollamaMessages = prompt
                        .Select(m => new Microsoft.Extensions.AI.ChatMessage(
                            m.Role == "System" ? Microsoft.Extensions.AI.ChatRole.System :
                            m.Role == "User" ? Microsoft.Extensions.AI.ChatRole.User :
                            Microsoft.Extensions.AI.ChatRole.Assistant, m.Text)).ToList();
                    var ollamaResponse = await ollamaClient.GetResponseAsync(ollamaMessages);
                    return ollamaResponse.Messages[0].Text;
                }
                case "AWSBedrock":
                {
                    var bedrockClient = aiClient as AWSBedrockClient;
                    var bedrockMessages = prompt
                        .Select(m => new Microsoft.Extensions.AI.ChatMessage(
                            m.Role == "System" ? Microsoft.Extensions.AI.ChatRole.System :
                            m.Role == "User" ? Microsoft.Extensions.AI.ChatRole.User :
                            Microsoft.Extensions.AI.ChatRole.Assistant, m.Text)).ToList();
                    var bedrockResponse = await bedrockClient.GetResponseAsync(bedrockMessages);
                    return bedrockResponse.Messages[0].Text;
                }
                default:
                    throw new Exception($"Unknown AI service: {aiService}");
            }
        }
    }
}
