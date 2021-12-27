<?xml version="1.0" encoding="UTF-8"?>
<ModuleFile xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<UiMod name="ScenarioAlerter" version="1.0" date="27/12/2021" >
		<VersionSettings gameVersion="1.4.8" windowsVersion="1.0" savedVariablesVersion="1.0" />
		<Author name="aaearon"/>
		<Description text="Keeps a log of scenario pops. Code originally by Caffeine." />
		<Files>
			<File name="ScenarioAlerter.lua" />
		</Files>
		<OnInitialize>
			<CallFunction name="ScenarioAlerter.OnInitialize" />
		</OnInitialize>
		<OnUpdate>
    	</OnUpdate>
        <OnShutdown>
			<CallFunction name="ScenarioAlerter.ClearLogFile" />
        </OnShutdown>
	</UiMod>
</ModuleFile>