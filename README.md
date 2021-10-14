# ScenarioDiscordAlerter

Inspired by [ScenarioAlerter](https://www.returnofreckoning.com/forum/viewtopic.php?f=66&t=20524). Monitors the log file that the ScenarioAlerter addon creates and posts a message to the provided Discord Webhook which can alert you of a scenario pop.

## Setup

1. From the above link copy over the ScenarioAlert addon.
1. Compile the project
1. Copy `app.config.example` to `app.config` and define the location of ScenarioAlert.log and the Discord Webhook to post to.
1. Open the compiled executable and in game type `/script ScenarioAlerter.recordScPop()` to test to see if it works.
