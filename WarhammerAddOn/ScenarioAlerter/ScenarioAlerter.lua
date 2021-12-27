ScenarioAlerter = {}
ScenarioAlerter.logFileName = "ScenarioAlerter"
ScenarioAlerter.logFilePath = StringToWString("logs/"..ScenarioAlerter.logFileName..".log")

function ScenarioAlerter.OnInitialize()
	TextLogCreate(ScenarioAlerter.logFileName, 18000)
	TextLogSetEnabled(ScenarioAlerter.logFileName, true)
	TextLogSetIncrementalSaving(ScenarioAlerter.logFileName, true, ScenarioAlerter.logFilePath)

	RegisterEventHandler(SystemData.Events.SCENARIO_SHOW_JOIN_PROMPT, "ScenarioAlerter.RecordScPop")
end

function ScenarioAlerter.RecordScPop()
	local scenarioName = GetScenarioName(GameData.ScenarioData.startingScenario)

	TextLogAddEntry(ScenarioAlerter.logFileName, 0, towstring("Pop! Scenario: ")..scenarioName)
	TextLogSaveLog(ScenarioAlerter.logFileName, ScenarioAlerter.logFilePath)
end

-- /script ScenarioAlerter.RecordScPop()
-- /script TextLogDestroy("ScenarioAlerter.log")
-- /script ScenarioAlerter.ClearLogFile()
function ScenarioAlerter.ClearLogFile()
	TextLogDestroy(ScenarioAlerter.logFileName)

	TextLogCreate(ScenarioAlerter.logFileName, 18000)
	TextLogSetEnabled(ScenarioAlerter.logFileName, true)
	TextLogSetIncrementalSaving(ScenarioAlerter.logFileName, false, ScenarioAlerter.logFilePath)
end