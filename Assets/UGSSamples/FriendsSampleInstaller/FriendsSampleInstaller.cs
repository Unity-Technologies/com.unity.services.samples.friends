using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;


public static class FriendsSampleInstaller
{
    [MenuItem("Tools/FriendsSample")]
    public static async void Install()
    {
        var addAndRemoveRequest = Client.AddAndRemove(new[]
        {
            "com.unity.services.friends@0.2.0-preview.9"
        });

        while (!addAndRemoveRequest.IsCompleted)
            await Task.Delay(100);

        var path = "Assets/UGSSamples/FriendsSampleInstaller/Friends.unitypackage";
        AssetDatabase.ImportPackage(path, true);
    }
}