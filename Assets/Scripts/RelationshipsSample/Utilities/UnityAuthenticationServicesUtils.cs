using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Unity.Services.Toolkits.Friends
{
    public static class UnityAuthenticationServicesUtils
    {
        public static async Task SwitchUser(string playerName)
        {
            Debug.Log($"Switching Player Profile to PlayerName: '{playerName}'");
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.SwitchProfile(playerName);
            await LogIn(playerName);
        }

        static async Task LogIn(string playerName)
        {
            var options = new InitializationOptions();
            var option = options.SetProfile(playerName);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Authenticated <b>{playerName}</b> with Id: <b>{AuthenticationService.Instance.PlayerId}</b>");
        }
    }
}