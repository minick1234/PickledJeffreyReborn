using Discord.WebSocket;
using Discord;
using System;
using System.Net.Http.Headers;
using System.Text;
using Discord.Commands;
using Newtonsoft.Json;

namespace MrJeffreyThePickle
{
    public class ChatGPTCommandHandlerService : IRegisterSlashCommands
    {
        private readonly DiscordSocketClient _client;
        private readonly string CHATGPT_API_KEY;
        private readonly HttpClient _httpClient;

        public ChatGPTCommandHandlerService(DiscordSocketClient client)
        {
            _client = client;
            CHATGPT_API_KEY = Environment.GetEnvironmentVariable("CHATGPT_API_KEY");
            _httpClient = new HttpClient();
        }

        [Command("picklegpt")]
        private async Task HandlePickleGPTCommand(SocketSlashCommand command)
        {
            string messageToAsk = (string)command.Data.Options.First(option => option.Name == "question").Value;

            await command.DeferAsync();

            string responseFromGPT = await GetChatGPTResponse(messageToAsk.ToString());

            await command.FollowupAsync(
                "**You asked:**\n" + messageToAsk.ToString() + "\n\n**PickledGPT Responded:**\n" + responseFromGPT,
                null, TTSStateHandlerService.IsResponsesTts);
        }

        private async Task<string> GetChatGPTResponse(string question)
        {
            Console.WriteLine(CHATGPT_API_KEY);
            if (string.IsNullOrEmpty(CHATGPT_API_KEY))
            {
                Console.WriteLine("No api key is provided.");
                return null;
            }

            var requestBody = new
            {
                model = "gpt-4o",
                messages = new[]
                {
                    new
                    {
                        role = "system", content =
                            "You are named PickleGPT, and are a helpful AI assitant, who also has some attitude, and attempts to be sassy with replies. You hate carrots," +
                            " and fight for the Pickle Mafia, stomping and insulting the carrot gang whenever you have the chance."
                    },
                    new { role = "user", content = question }
                },
                max_tokens = 1000
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8,
                "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CHATGPT_API_KEY);

            Console.WriteLine("Starting pickleGPT request...");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            Console.WriteLine("Finished request to PickleGPT");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                return "Sorry friend, but I couldn't generate a response from PickleGPT.";
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            dynamic parsedJson = JsonConvert.DeserializeObject(responseJson);
            string reply = parsedJson.choices[0].message.content;
            return reply.Trim();
        }
    }
}