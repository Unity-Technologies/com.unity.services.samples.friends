using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Unity.Services.Samples
{
    /// <summary>
    /// The Samples Plugins implementation of the Unity Authentication Service
    /// Using this ensures we don't log in multiple times when combining samples.
    /// </summary>

    [CreateAssetMenu(order = 0, fileName = "New PlayerAuth_SO", menuName = "UGS/Samples/PlayerAuth_SO")]
    public class PlayerAuthentication : ScriptableObject
    {
        [field: SerializeField] public PlayerProfile LocalPlayer { get; private set; }

        IPlayerProfileService m_PlayerProfileService;
        bool m_SigningIn = false;
        public async Task Init(string profileName = null)
        {
            if (IsInitialized)
                return;
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

            m_PlayerProfileService = new SamplePlayerProfileService();
        }

        public async Task SignIn(string profileName = null)
        {
            await Init(profileName);
            if (IsSignedIn || m_SigningIn)
                return;
            m_SigningIn = true;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var playerID = AuthenticationService.Instance.PlayerId;
            LocalPlayer = new PlayerProfile(m_PlayerProfileService.GetName(playerID),playerID);

            m_SigningIn = false;
            Debug.Log($"[Auth] Signed into Unity Services as {LocalPlayer}");
        }

        public bool IsInitialized => UnityServices.State == ServicesInitializationState.Initialized;
        public bool IsSignedIn => IsInitialized && AuthenticationService.Instance.IsSignedIn ;
    }
}
