# ScenarioAlerter

Inspired by the original [ScenarioAlerter](https://www.returnofreckoning.com/forum/viewtopic.php?f=66&t=20524). It monitors the log file that the ScenarioAlerter addon creates and sends a message via the provided Discord Webhook or sends a push notification via [Pushover](https://pushover.net/) which will alert you of a scenario pop.

## Usage

1. Copy over the `ScenarioAlerter` folder in the `WarhammerAddOn` folder to `Interface\AddOns\` under your Warhammer Online game folder.
1. [Download and install the .NET 6.0 Runtime.](https://dotnet.microsoft.com/en-us/download)
1. Rename `app.config.example` to `app.config`.
1. Define the location of the logFile (keep `\logs\ScenarioAlerter.log`), the method you want to recieve the alert by in `alertMethod` (Pushover or Discord), and the method-specific settings. Read further for information on each method.
1. After correctly defining the values in `app.config`, start `ScenarioAlerter.exe` and test the alert method via executing `/script ScenarioAlerter.RecordScPop()` ingame. You should get the message "Pop! Scenario: " via your selected alert method (when a scenario actually pops, the name of the scenario will be included in the alert.)

### Discord

> **_NOTE:_**  If you are looking to use Discord as a way to get notified on a mobile device, be aware that mobile push notifications will only be sent when you are inactive at your computer for a certain amount of time (determined by the value set as Push Notification Inactive Timeout in the desktop app: `App Settings > Notifications > Push Notification Inactive Timeout`.) At the moment, there is no way to recieve mobile push notifications for each message without closing Discord completely.

Using a [webhook](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks), this method will alert via a message to a Discord text channel when a scenaro pops ingame. For the Discord channel you want the message to be posted in, provide the webhook URL in `app.config` for the value of the `webhookUri` element.

### Pushover

Using the [Pushover](https://pushover.net/) desktop or mobile applications, recieve a push notification on your device when a scenario pops ingame. This requires signing up for a Pushover account, using the desktop and/or mobile applications, and creating your own application to use it's application token along with your user token in sending alerts.

#### Setting up Pushover

1. [Create a Pushover account](https://pushover.net/signup).
1. Use your User Key as the value for the `userToken` element in `app.config`.
1. [Create an Application](https://pushover.net/apps/build) by defining at least a Name (`ScenarioAlerter` for example.)
1. Use the API Token/Key for the application as the value for the `applicationToken` element in `app.config`.
