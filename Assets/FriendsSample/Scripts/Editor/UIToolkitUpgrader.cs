#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Toolkits.Friends.UIToolkit.Editor
{
    /// <summary>
    /// Upgrades UITOOLKIT from 2020 LTS versions to 2021 versions
    /// </summary>
    [InitializeOnLoad]
    public class UIToolkitUpgrader : UnityEditor.Editor
    {
#if UNITY_2021_1_OR_NEWER
        const string k_TriedUpgrade = "UIToolkitUpgrader.TriedUpgrade";

        static UIToolkitUpgrader()
        {
            EditorApplication.delayCall += TryToolkitUpgrade;
        }

        static void TryToolkitUpgrade()
        {
            if (!SessionState.GetBool(k_TriedUpgrade, false))
            {
                Try2020To2021Upgrade();
                SessionState.SetBool(k_TriedUpgrade, true);
            }

            void Try2020To2021Upgrade()
            {
                var toolkitInstance = GetToolkitPrefabInstance();

                var uiDocumentComponent = toolkitInstance.GetComponent<UIDocument>();

                //If there is NO UIDocument on this prefab, we need to fix
                if (uiDocumentComponent == null)
                {
                    Debug.Log("RelationshipsUIToolkitController prefab missing its UIDocument component." +
                        "This is likely due to upgrading from Unity 2020 to 2021+, attempting to fix with Updated UIToolkit components.");

                    //If Not, Try to Remove Behaviours, and Add a new UIDocument

                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(toolkitInstance);

                    uiDocumentComponent = toolkitInstance.AddComponent<UIDocument>();

                    uiDocumentComponent.panelSettings = ReplacePanelSettingsPrefab();
                    uiDocumentComponent.visualTreeAsset = GetSocialUIPrefab();

                    var uiToolkitController = toolkitInstance.GetComponent<RelationshipsViewUIToolkit>();

                    uiToolkitController.SetUIDocument(uiDocumentComponent);
                    PrefabUtility.ApplyPrefabInstance(toolkitInstance, InteractionMode.AutomatedAction);
                    AssetDatabase.SaveAssets();
                    Debug.Log("Successfully Upgraded UIToolkit implementation to Unity 2021 + ");
                }

                DestroyImmediate(toolkitInstance);
            }

            PanelSettings ReplacePanelSettingsPrefab()
            {
                var oldPanelSettings = GetOldPanelSettings(out var panelSettingsPath);
                DestroyImmediate(oldPanelSettings);
                var newPanelSettings = CreateInstance<PanelSettings>();
                newPanelSettings.name = "SocialUIPanelSettings";
                newPanelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
                AssetDatabase.CreateAsset(newPanelSettings, panelSettingsPath);

                return newPanelSettings;
            }

            VisualTreeAsset GetSocialUIPrefab()
            {
                var socialUiid = AssetDatabase.FindAssets("SocialUI", new[] { "Assets/Prefabs/UIToolkit/Views" });
                var socialUIPath = AssetDatabase.GUIDToAssetPath(socialUiid[0]);

                return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(socialUIPath);
            }

            GameObject GetToolkitPrefabInstance()
            {
                var toolkitId = AssetDatabase.FindAssets("RelationshipUIToolkit");
                var toolkitUIPath = AssetDatabase.GUIDToAssetPath(toolkitId[0]);
                var toolkitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(toolkitUIPath);

                return PrefabUtility.InstantiatePrefab(toolkitPrefab) as GameObject;
            }

            ScriptableObject GetOldPanelSettings(out string panelSettingsPath)
            {
                var panelSetingsID = AssetDatabase.FindAssets("SocialUIPanelSettings");
                panelSettingsPath = AssetDatabase.GUIDToAssetPath(panelSetingsID[0]);
                return AssetDatabase.LoadAssetAtPath<ScriptableObject>(panelSettingsPath);
            }
        }
#endif
    }
}
#endif