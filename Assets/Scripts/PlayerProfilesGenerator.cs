using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerProfilesGenerator : MonoBehaviour
    {
        [SerializeField] private RelationshipsSceneManager _relationshipsSceneManager = null;
        [SerializeField] private int _amount = 5;
        [SerializeField] private PlayerProfilesData m_PlayerProfilesData = null;

        private const string PlayerNamePrefix ="Player_";
        
        private async void Start()
        {
            //The default logged in Player will be the last profile created.
            var playerName = $"{PlayerNamePrefix}{_amount - 1}";
            var hasGeneratedPlayerIds = m_PlayerProfilesData.Any();
            if (hasGeneratedPlayerIds)
            {
                await UASUtils.LogIn(playerName);
            }
            else
            {
                await GeneratePlayerProfiles(_amount);
            }

            Debug.Log($"Authenticated <b>{playerName}</b> with Id: <b>{AuthenticationService.Instance.PlayerId}</b>");

            await _relationshipsSceneManager.Init(playerName);
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
            var playerName = $"{PlayerNamePrefix}{i}";
            await UASUtils.SwitchUser(playerName);
            m_PlayerProfilesData.Add(playerName, AuthenticationService.Instance.PlayerId);
        }
    }
}