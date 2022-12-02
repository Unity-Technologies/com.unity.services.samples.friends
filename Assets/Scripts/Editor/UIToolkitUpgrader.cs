#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Unity.Services.Toolkits.Editor
{
    /// <summary>
    /// Upgrades UITOOLKIT from 2020 LTS versions to 2021 versions
    /// </summary>
    [InitializeOnLoad]
    public class UIToolkitUpgrader : UnityEditor.Editor
    {
        bool m_StylesInitialized;

        const string k_CheckedUnityVersion = "ReadmeEditor.showedReadme";

        static UIToolkitUpgrader()
        {
            EditorApplication.delayCall += CheckForUnityVersionAndUpgrade;
        }

        static void CheckForUnityVersionAndUpgrade()
        {
            if (!SessionState.GetBool(k_CheckedUnityVersion, false))
            {
                Debug.Log($"Current Unity Version: {Application.unityVersion}");
                SessionState.SetBool(k_CheckedUnityVersion, true);
            }
        }
    }
}
#endif