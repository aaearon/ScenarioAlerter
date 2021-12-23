using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace ScenarioAlerter
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();
        private static string fileToWatch;
        private static string discordWebhookUri;
        private static string pushoverUser;
        private static string pushoverToken;
        private static readonly string pushoverMessageUri = "https://api.pushover.net/1/messages.json";
        private static string lastReadLine;

        static async Task Main(string[] args)
        {

            fileToWatch = ConfigurationManager.AppSettings.Get("LogFile");
            discordWebhookUri = ConfigurationManager.AppSettings.Get("WebhookUri");
            pushoverUser = ConfigurationManager.AppSettings.Get("PushoverUser");
            pushoverToken = ConfigurationManager.AppSettings.Get("PushoverToken");

            if (fileToWatch == null || discordWebhookUri == null)
            {
                Console.WriteLine("Ensure that LogFile and WebhookUri is defined in app.config!");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                return;
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
            watcher.Error += OnError;

            watcher.EnableRaisingEvents = true;

            Thread t = new Thread(RefreshFile);
            t.IsBackground = true;
            t.Start();

            Console.WriteLine($"Watching {fileToWatch}");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void RefreshFile()
        {
            while (true)
            {

                using (var fs = new FileStream(fileToWatch, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    Thread.Sleep(500);
            }

        }

        private static async void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var lastLine = ReadLines($"{e.FullPath}").LastOrDefault();

            if (lastLine != null && lastLine != lastReadLine)
            {
                //await SendDiscordWebHook($"{lastLine}");
                await SendPushoverMessage($"{lastLine}");
            }

            lastReadLine = lastLine;
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private static async Task SendDiscordWebHook(string message)
        {
            Console.WriteLine($"Sending Discord Webhook with message: {message}");
             
            string webhookUri = discordWebhookUri;
            Dictionary<string, string> webhookContent = new Dictionary<string, string>
            {
                { "content", message }
            };
            var json = JsonConvert.SerializeObject(webhookContent);

            var response = await client.PostAsync(webhookUri, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        private static async Task SendPushoverMessage(string message)
        {
            Console.WriteLine($"Sending Pushover with message: {message}");

            Dictionary<string, string> messageContent = new Dictionary<string, string>
            {
                { "message", message },
                { "user", pushoverUser },
                { "token", pushoverToken }
            };

            var json = JsonConvert.SerializeObject(messageContent);
            var response = await client.PostAsync(pushoverMessageUri, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
    PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}