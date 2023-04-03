
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Unity.Services.Samples
{
    /// <summary>
    /// The Samples Plugins implementation of the Unity Authentication Service
    /// Prevents erros when combining samples
    /// </summary>
    public class SampleAuthenticator : MonoBehaviour
    {
        public async Task<bool> InitServices(string profileName = null)
        {
            if (!UnInitialized)
                return false;

            if (profileName != null)
            {
                //ProfileNames can't contain non-alphanumeric characters
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                profileName = rgx.Replace(profileName, "");
                var authProfile = new InitializationOptions().SetProfile(profileName);

                //If you are using multiple unity services, make sure to initialize it only once before using your services.
                await UnityServices.InitializeAsync(authProfile);
            }
            else
                await UnityServices.InitializeAsync();

            return IsInitialized;

        }

        public async Task SignIn(string profileName = null)
        {
            if (!await InitServices(profileName))
                return;

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }

        bool IsInitialized => UnityServices.State == ServicesInitializationState.Initialized;
        bool UnInitialized => UnityServices.State == ServicesInitializationState.Uninitialized;

    }

}
