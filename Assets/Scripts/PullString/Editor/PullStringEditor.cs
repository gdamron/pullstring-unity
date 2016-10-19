using System.IO;
using UnityEditor;
using UnityEngine;

public static class PullStringEditor {
    private const string TestMenuPath = "Unity Test Tools/PullString Tests Enabled";
    private const string TestDefine = "UNIT_TEST";
    private static readonly BuildTargetGroup[] TargetGroups = {
        BuildTargetGroup.Android,
        BuildTargetGroup.iOS,
        BuildTargetGroup.Standalone
    };

    [InitializeOnLoadMethod]
    static void LookForUnityTestTools()
    {
        var enabled = CanEnableTests();

        if (!enabled) {
            Debug.LogWarning("You can't run the PullString SDK unit tests. Import Unity Tests Tools from the Asset Store and restart to do so.");
        }

        EditorApplication.delayCall += () =>
        {
            Menu.SetChecked(TestMenuPath, enabled);
        };

        foreach (var target in TargetGroups) {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            defines = defines.Replace(TestDefine + ";", string.Empty);
            defines = defines.Replace(TestDefine, string.Empty);

            if (enabled) {
                defines += TestDefine;
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
        }
    }

    [MenuItem(TestMenuPath)]
    static void EnableUnitTests()
    {
        LookForUnityTestTools();
    }

    [MenuItem(TestMenuPath, true)]
    static bool CanEnableTests()
    {
        var wd = Directory.GetCurrentDirectory();
        wd = Path.Combine(wd, "Assets");
        var toolsDir = Path.Combine(wd, "UnityTestTools");
        return Directory.Exists(toolsDir);
    }
}
