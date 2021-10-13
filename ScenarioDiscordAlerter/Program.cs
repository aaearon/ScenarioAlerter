using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioDiscordAlerter
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();
        public static string discordWebhookUri;


        static Task Main(string[] args)
        {
            string fileToWatch;

            if (args.Length == 0)
            {
                // fileToWatch = @"C:\Warhammer Online Age of Reckoning\logs\launcher.log";
                Console.WriteLine("You must define arguments!");
                return Task.CompletedTask;
            }
            else
            {
                fileToWatch = args[0];
                discordWebhookUri = args[1];
                
            }


            string fileDirectory = Path.GetDirectoryName(fileToWatch);
            string fileName = Path.GetFileName(fileToWatch);

            using var watcher = new FileSystemWatcher(fileDirectory);
            watcher.Filter = fileName;

            watcher.NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watching {fileToWatch}");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            return Task.CompletedTask;
        }

        private static async void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var lastLine = File.ReadLines($"{e.FullPath}").Last();
            await SendDiscordWebHook(lastLine);
            //Console.WriteLine(lastLine);
        }

        private static async Task SendDiscordWebHook(string message)
        {
            Console.WriteLine(message);

            string webhookUri = discordWebhookUri;
            Dictionary<string, string> webhookContent = new Dictionary<string, string>();
            webhookContent.Add("content", message);
            var json = JsonConvert.SerializeObject(webhookContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            await client.PostAsync(webhookUri, stringContent);
        }
    }
}
