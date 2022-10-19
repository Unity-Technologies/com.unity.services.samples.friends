using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace UnityGamingServicesUsesCases.Relationships
{
    public static class UASUtils
    {
        public static async Task SwitchUser(string playerName)
        {
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.SwitchProfile(playerName);
            await LogIn(playerName);
        }

        public static async Task LogIn(string playerName)
        {
            var options = new InitializationOptions();
            var option = options.SetProfile(playerName);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}