using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerProfilesGenerator : MonoBehaviour
    {
        [SerializeField] private RelationshipsSceneManager m_RelationshipsSceneManager = null;
        [SerializeField] private PlayerProfilesData m_PlayerProfilesData = null;
        [SerializeField] private int m_Amount = 5;

        private const string k_PlayerNamePrefix = "Player_";

        private async void Start()
        {
            //The default logged in Player will be the last profile created.
            var playerName = $"{k_PlayerNamePrefix}{m_Amount - 1}";
            var hasGeneratedPlayerIds = m_PlayerProfilesData.Any();
            if (hasGeneratedPlayerIds)
            {
                await UASUtils.LogIn(playerName);
            }
            else
            {
                await GeneratePlayerProfiles(m_Amount);
            }

            Debug.Log($"Authenticated <b>{playerName}</b> with Id: <b>{AuthenticationService.Instance.PlayerId}</b>");

            await m_RelationshipsSceneManager.Init(playerName);
        }

        private async Task GeneratePlayerProfiles(int amount)
        {
            //Need to initialize before doing anything.
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            for (int i = 0; i < amount; i++)
            {
                await GeneratePlayerProfile(i);
            }
        }

        private async Task GeneratePlayerProfile(int i)
        {
            var playerName = $"{k_PlayerNamePrefix}{i}";
            await UASUtils.SwitchUser(playerName);
            m_PlayerProfilesData.Add(playerName, AuthenticationService.Instance.PlayerId);
        }
    }
}