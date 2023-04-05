using UnityEditor;
using UnityEngine;

public class FriendsSDKPostProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
      
        foreach (var imported in importedAssets)
        {
            Debug.Log(imported);
            if (imported == "Packages/com.unity.services.friends/Runtime/SDK/FriendsApi.cs")
            {
                Debug.Log("Detected friends package import");
                var path = "Assets/FriendsSampleInstaller/Package/FriendsSample.unitypackage";
                AssetDatabase.ImportPackage(path, true);
            }
        }
    }
}