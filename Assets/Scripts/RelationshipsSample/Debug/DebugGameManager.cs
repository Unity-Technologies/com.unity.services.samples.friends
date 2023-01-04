using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Toolkits.Friends;
using UnityEngine;

/// <summary>
/// Debug Game Manager, we are using this only for initialization.
/// You would replace this with your own.
/// </summary>
public class DebugGameManager : MonoBehaviour
{
    [SerializeField] RelationshipsManager m_RelationshipsManager;

    async void Start()
    {
        await InitServices();
    }

    /// <summary>
    /// The only service we are dependent on is one that connects playerNames to Unity Authentication ID's
    /// </summary>
    async Task InitServices()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        var playerID = AuthenticationService.Instance.PlayerId;
        var debugSocialProfileService = new DebugSocialProfileService();
        await m_RelationshipsManager.Init(playerID, debugSocialProfileService);
    }
}
