@ECHO OFF
call "C:\Program Files\Adobe\Flex Builder 3\FlexFrameworkCmdPrompt.bat" "C:\Program Files\Adobe\Flex Builder 3\sdks\3.2.0" "C:\Program Files\Adobe\Flex Builder 3\jre"
asdoc -source-path src -doc-sources src -output asdoc -target-player 10.0.12 -main-title "danmaq Nineball Library" -window-title "danmaq Nineball Library"
PAUSE
