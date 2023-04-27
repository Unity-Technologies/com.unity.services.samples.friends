#if UNITY_EDITOR

// NOTE:
// You can disable all editor analytics in Unity > Preferences.
// The EditorAnalytics API is for Unity to collect usage data for this samples project in
// order to improve our products. The EditorAnalytics API won't be useful in your own project
// because it only works in the Editor and the data is only sent to Unity. To see how you
// could implement analytics in your own project, have a look at
// the AB Test Level Difficulty sample or the Seasonal Events sample.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Unity.Services.Samples
{
    [InitializeOnLoad]
    public static class FriendSampleEditorAnalytics
    {
        [Serializable]
        struct RelationshipManagerStartedData
        {
            public string SceneName;
            public bool IsUITK;
        }

        [Serializable]
        struct SceneTotalSessionLengthData
        {
            public string sceneName;
            public int sessionLengthSeconds;
        }

        const int k_WaitForEnabledMilliseconds = 500;

        const int k_MaxEventsPerHour = 3600;
        const int k_MaxItems = 10;
        const string k_VendorKey = "unity.friendsSample";

        const string k_Prefix = "friendsSample";

        const string k_ImportedEvent = k_Prefix + "Imported";
        const int k_ImportedEventVersion = 1;

        const string k_PrefabStartedEvent = k_Prefix + "FriendsManagerStarted";
        const int k_PrefabStartedVersion = 1;

        static string m_CurrentSceneName;
        static DateTime m_CurrentSceneSessionCheckpoint;

        static FriendSampleEditorAnalytics()
        {
            m_CurrentSceneName = SceneManager.GetActiveScene().name;
            m_CurrentSceneSessionCheckpoint = DateTime.Now;

            EditorApplication.wantsToQuit += OnEditorWantsToQuit;

// disable the warning that we aren't awaiting this call
#pragma warning disable 4014

            RegisterEvents();

#pragma warning restore 4014
        }

        static async Task RegisterEvents()
        {
            var timeout = 0;

            while (!EditorAnalytics.enabled)
            {
                await Task.Delay(k_WaitForEnabledMilliseconds);
                timeout += k_WaitForEnabledMilliseconds;

                if (timeout >= 30000)
                {
                    // let's stop trying after 30 seconds because the editor user might have disabled EditorAnalytics
                    return;
                }
            }
        }

        public static void SendRelationshipManagerStarted(string sceneName, bool isUITK)
        {
            EditorAnalytics.SendEventWithLimit(
                k_PrefabStartedEvent,
                new RelationshipManagerStartedData { SceneName = sceneName, IsUITK = isUITK },
                k_PrefabStartedVersion);
        }

        static bool OnEditorWantsToQuit()
        {
            return true;
        }
    }
}

#endif