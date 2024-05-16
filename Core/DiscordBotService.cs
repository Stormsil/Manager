using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Manager.Core
{
    public class DiscordBotService
    {
        private DiscordClient _discordClient;
        private CommandsNextExtension _commands;
        private Dictionary<string, Dictionary<string, string>> _channelKeywords;

        public async Task StartAsync(string token)
        {
            var discordConfig = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };

            _discordClient = new DiscordClient(discordConfig);
            _commands = _discordClient.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" }
            });

            _discordClient.Ready += OnReady;
            _discordClient.MessageCreated += OnMessageCreated;

            LoadChannelKeywords();

            await _discordClient.ConnectAsync();
        }

        public async Task StopAsync()
        {
            await _discordClient.DisconnectAsync();
            _discordClient.Dispose();
        }

        private Task OnReady(DiscordClient client, ReadyEventArgs e)
        {
            Console.WriteLine($"Bot {client.CurrentUser.Username} has successfully connected to Discord!");
            return Task.CompletedTask;
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot || e.Channel.IsPrivate) return;

            // Выводим исходное сообщение
            Console.WriteLine($"Received raw message: {e.Message.Content}");

            Console.WriteLine($"Processing message in channel {e.Channel.Name}: {e.Message.Content}");

            if (_channelKeywords.TryGetValue(e.Channel.Name, out var keywordsToChannels))
            {
                Console.WriteLine($"Keywords found for channel {e.Channel.Name}");
                foreach (var kvp in keywordsToChannels)
                {
                    if (e.Message.Content.Contains(kvp.Key))
                    {
                        var targetChannel = e.Guild.Channels.Values.FirstOrDefault(c => c.Name == kvp.Value);
                        if (targetChannel != null)
                        {
                            await targetChannel.SendMessageAsync(e.Message.Content);
                            Console.WriteLine($"Message '{e.Message.Content}' sent to channel {targetChannel.Name}.");
                        }
                        else
                        {
                            Console.WriteLine($"Target channel '{kvp.Value}' not found.");
                        }
                        break;
                    }
                }
            }
        }

        private void LoadChannelKeywords()
        {
            var filePath = @"C:\Files\Discord Bot\channel_keywords.json";
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _channelKeywords = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            }
            else
            {
                _channelKeywords = new Dictionary<string, Dictionary<string, string>>();
            }
        }
    }
}
