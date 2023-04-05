using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Unity.Services.Samples.Friends
{
    public static class FriendsSampleInstaller
    {
        public const string k_FriendsSDKName = "com.unity.services.friends";
        private const string k_FriendsSDKVersion = "0.2.0-preview.9";

        [MenuItem("Tools/FriendsSampleInstaller/Install")]
        public static async void Install()
        {
            var packages = Client.List();

            while (!packages.IsCompleted)
                await Task.Delay(100);

            if (IsPackageInstalled(packages, k_FriendsSDKName))
            {
                //Then we can install the friends sample package safely
                InstallFriendsSampleUnityPackage();
                return;
            }

            Client.Add($"{k_FriendsSDKName}@{k_FriendsSDKVersion}");
        }

        private static bool IsPackageInstalled(ListRequest packages, string packageName)
        {
            return packages.Result.Any(package => package.name.Equals(packageName));
        }

        public static void InstallFriendsSampleUnityPackage()
        {
            var friendsSamplePackagePath = "Assets/FriendsSampleInstaller/Package/FriendsSample.unitypackage";
            AssetDatabase.ImportPackage(friendsSamplePackagePath, true);
        }
    }
}