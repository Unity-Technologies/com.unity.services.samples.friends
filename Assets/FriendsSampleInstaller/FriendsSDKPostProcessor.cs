using UnityEditor;

public class FriendsSDKPostProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var importedAsset in importedAssets)
        {
            if (importedAsset != "Packages/com.unity.services.friends") 
                continue;
            
            FriendsSampleInstaller.InstallFriendsSampleUnityPackage();
        }
    }
}