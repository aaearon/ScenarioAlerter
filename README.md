# ScenarioAlerter

Inspired by the original [ScenarioAlerter](https://www.returnofreckoning.com/forum/viewtopic.php?f=66&t=20524). It monitors the log file that the ScenarioAlerter addon creates and sends a message via the provided Discord Webhook or sends a push notification via [Pushover](https://pushover.net/) which will alert you of a scenario pop.

## Setup

1. From the above link copy over the ScenarioAlert addon.
1. Compile the project.
1. Copy `app.config.example` to `app.config` and define the location of ScenarioAlert.log and the Discord Webhook or Pushover details.
1. Open the compiled executable and in game type `/script ScenarioAlerter.recordScPop()` to test to see if it works.
