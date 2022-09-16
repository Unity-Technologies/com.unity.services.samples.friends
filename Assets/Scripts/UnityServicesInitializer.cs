using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace UnityGamingServicesUsesCases
{
    namespace Relationships
    {
        public class UnityServicesInitializer : MonoBehaviour
        {
            [SerializeField] private FriendsUiController m_FriendsUiController = null;

            async void Start()
            {
                await UnityServices.InitializeAsync();

                AuthenticationService.Instance.ClearSessionToken();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Authenticated as " + AuthenticationService.Instance.PlayerId);
                Debug.Log("Token " + AuthenticationService.Instance.AccessToken);

                m_FriendsUiController.Init();
            }
        }
    }
}