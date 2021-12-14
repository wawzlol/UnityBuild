using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Build.Reporting;

public class GameBuilder : MonoBehaviour {

    [MenuItem("Build/Build Windows")]
    public static void BuildPC()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new [] { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Main.unity" };
        buildPlayerOptions.locationPathName = "build/Windows/Game.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report= BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build Succeed" + summary.totalSize + " bytes");
        }

        if(summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}