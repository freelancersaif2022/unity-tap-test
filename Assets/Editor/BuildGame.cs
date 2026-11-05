using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace UnityTapTest {
public static class BuildGame {
    const string ScenePath = "Assets/Scenes/Main.unity";
    [MenuItem("Tap Test/Open Demo Scene")]
    public static void PrepareScene() {
        Directory.CreateDirectory("Assets/Scenes");
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camera = new GameObject("Main Camera").AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(.04f, .06f, .11f);
        camera.transform.position = new Vector3(0, 0, -10);
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Tap Test/Build Android APK")]
    public static void BuildAndroid() {
        PrepareScene();
        PlayerSettings.companyName = "Tap Test";
        PlayerSettings.productName = "Unity Tap Test";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.freelancersaif.unitytaptest");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.forceInternetPermission = false;
        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.Android.bundleVersionCode = 1;
        EditorUserBuildSettings.buildAppBundle = false;
        Directory.CreateDirectory("build/Android");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { ScenePath }, locationPathName = "build/Android/UnityTapTest.apk",
            target = BuildTarget.Android, options = BuildOptions.Development
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Android build failed: " + report.summary.result);
        Debug.Log("APK ready: build/Android/UnityTapTest.apk");
    }
}
}
