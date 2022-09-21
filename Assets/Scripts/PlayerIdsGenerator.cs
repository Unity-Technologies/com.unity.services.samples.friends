using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerIdsGenerator : MonoBehaviour
    {
        [SerializeField] private RelationshipsSceneManager m_RelationshipsSceneManager = null;
        [SerializeField] private int m_Amount = 5;
        [SerializeField] private PlayerIdsData m_PlayerIdsData = null;

        private const string PlayerNamePrefix ="Player_";
        
        private async void Start()
        {
            //Editor Player will be the last profile created.
            var playerName = $"{PlayerNamePrefix}{m_Amount - 1}";
            var hasGeneratedPlayerIds = PlayerPrefs.GetString(playerName) != "";
            if (hasGeneratedPlayerIds)
            {
                await LogIn(playerName);
            }
            else
            {
                await GeneratePlayerIds(m_Amount);
            }

            Debug.Log($"Authenticated <b>{playerName}</b> with Id: <b>{AuthenticationService.Instance.PlayerId}</b>");

            m_RelationshipsSceneManager.Init();
        }

        private async Task LogIn(string playerName)
        {
            var options = new InitializationOptions();
            var option = options.SetProfile(playerName);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private async Task GeneratePlayerIds(int amount)
        {
            //Need to initialize before doing anything.
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            m_PlayerIdsData.Clear();

            for (int i = 0; i < amount; i++)
            {
                await GenerateSinglePlayerId(i);
            }
        }

        private async Task GenerateSinglePlayerId(int i)
        {
            AuthenticationService.Instance.SignOut();
            var playerName = $"{PlayerNamePrefix}{i}";
            AuthenticationService.Instance.SwitchProfile(name);
            await LogIn(playerName);
            m_PlayerIdsData.Add(AuthenticationService.Instance.PlayerId);
        }
    }
}