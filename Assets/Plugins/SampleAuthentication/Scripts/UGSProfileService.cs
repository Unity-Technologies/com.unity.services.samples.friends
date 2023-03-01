using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Unity.Services.Samples
{

    /// <summary>
    /// The Samples Plugins implementation of the Unity Authentication Service
    /// Using this ensures we don't log in multiple times when combining samples.
    /// </summary>
    public class UGSAnonymousProfileService : IUGSAuthService
    {
        [field: SerializeField] public IUGSPlayer LocalPlayer { get; private set; }
        IProfileService m_SocialProfileService;

        public UGSAnonymousProfileService()
        {
            m_SocialProfileService = new SampleSocialProfileService();
        }
        public async Task TryAuthenticate(string profileName = "player")
        {
            if (IsSignedIn())
                return;
            var authProfile = new InitializationOptions().SetProfile(profileName);
            //If you are using multiple unity services, make sure to initialize it only once before using your services.
            await UnityServices.InitializeAsync(authProfile);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var playerID = AuthenticationService.Instance.PlayerId;
            var playerName = m_SocialProfileService.GetName(playerID);
            LocalPlayer = new UGSPlayer(playerName, playerID);
        }

        public bool IsSignedIn()
        {
            return UnityServices.State != ServicesInitializationState.Uninitialized && AuthenticationService.Instance.IsSignedIn;
        }
    }
}
