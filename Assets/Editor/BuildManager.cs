using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build.Reporting;
using UnityEditor;

public static class BuildManager
{
    public enum BuildType
    {
        DEVELOPMENT,
        RELEASE,
        DEBUGGING
    }

    public enum BuildPlatform
    {
        WINDOWS,
        WINDOWS64,
        LINUX,
        MACOSX
    }

    //[MenuItem("File/Build/Dev/Windows")]
    static void BuildWindowsDevelopment()
    {
        Build(BuildType.DEVELOPMENT, BuildPlatform.WINDOWS);
    }

    //[MenuItem("File/Build/Dev with Debugging/Windows")]
    static void BuildWindowsDebug()
    {
        Build(BuildType.DEBUGGING, BuildPlatform.WINDOWS);
    }

    //[MenuItem("File/Build/Release/Windows")]
    static void BuildWindowsRelease()
    {
        Build(BuildType.RELEASE, BuildPlatform.WINDOWS);
    }

    [MenuItem("File/Build/Dev/Windows64")]
    static void BuildWindows64Development()
    {
        Build(BuildType.DEVELOPMENT, BuildPlatform.WINDOWS64);
    }

    [MenuItem("File/Build/Dev with Debugging/Windows64")]
    static void BuildWindows64Debug()
    {
        Build(BuildType.DEBUGGING, BuildPlatform.WINDOWS64);
    }

    [MenuItem("File/Build/Release/Windows64")]
    static void BuildWindows64Release()
    {
        Build(BuildType.RELEASE, BuildPlatform.WINDOWS64);
    }

    [MenuItem("File/Build/Dev/Linux")]
    static void BuildLinuxDevelopment()
    {
        Build(BuildType.DEVELOPMENT, BuildPlatform.LINUX);
    }

    [MenuItem("File/Build/Dev with Debugging/Linux")]
    static void BuildLinuxDebug()
    {
        Build(BuildType.DEBUGGING, BuildPlatform.LINUX);
    }

    [MenuItem("File/Build/Release/Linux")]
    static void BuildLinuxRelease()
    {
        Build(BuildType.RELEASE, BuildPlatform.LINUX);
    }

    [MenuItem("File/Build/Dev/MacOSX")]
    static void BuildMacOSXDevelopment()
    {
        Build(BuildType.DEVELOPMENT, BuildPlatform.MACOSX);
    }

    [MenuItem("File/Build/Dev with Debugging/MacOSX")]
    static void BuildMacOSXDebug()
    {
        Build(BuildType.DEBUGGING, BuildPlatform.MACOSX);
    }

    [MenuItem("File/Build/Release/MacOSX")]
    static void BuildMacOSXRelease()
    {
        Build(BuildType.RELEASE, BuildPlatform.MACOSX);
    }

    static void Build(BuildType buildType, BuildPlatform buildPlatform)
    {
        BuildPipeline.BuildPlayer(GetScenes(), GetPath(buildType, buildPlatform), GetBuildTarget(buildPlatform), GetBuildOptions(buildType) | BuildOptions.AutoRunPlayer);
    }

    static EditorBuildSettingsScene[] GetScenes()
    {
        return EditorBuildSettings.scenes;
    }

    static string GetPath(BuildType buildType, BuildPlatform buildPlatform)
    {
        string buildPath = "./Builds/";

        switch (buildPlatform)
        {
            case BuildPlatform.WINDOWS:
                buildPath += "Windows/";
                break;

            case BuildPlatform.WINDOWS64:
                buildPath += "Windows64/";
                break;

            case BuildPlatform.LINUX:
                buildPath += "Linux/";
                break;

            case BuildPlatform.MACOSX:
                buildPath += "MacOSX/";
                break;
        }

        switch (buildType)
        {
            case BuildType.DEVELOPMENT:
                buildPath += "Development/Dev_";
                break;

            case BuildType.RELEASE:
                buildPath += "Release/";
                break;

            case BuildType.DEBUGGING:
                buildPath += "DevelopmentDebug/Dev_";
                break;

            default:
                throw new UnityException("Incorrect Build Type");
        }

        buildPath += "UnitedTwinlightSouls.exe";

        return buildPath;
    }

    static BuildTarget GetBuildTarget(BuildPlatform buildTarget)
    {
        switch (buildTarget)
        {
            case BuildPlatform.WINDOWS:
                return BuildTarget.StandaloneWindows;

            case BuildPlatform.WINDOWS64:
                return BuildTarget.StandaloneWindows64;

            case BuildPlatform.LINUX:
                return BuildTarget.StandaloneLinux64;

            case BuildPlatform.MACOSX:
                return BuildTarget.StandaloneOSX;

            default:
                throw new UnityException("Incorrect Build Target");
        }
    }

    static BuildOptions GetBuildOptions(BuildType buildType)
    {
        switch (buildType)
        {
            case BuildType.DEVELOPMENT:
                return BuildOptions.Development;

            case BuildType.RELEASE:
                return BuildOptions.None;

            case BuildType.DEBUGGING:
                return BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectToHost;

            default:
                throw new UnityException("Incorrect Build Tpe");
        }
    }
}
