using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerIdsGenerator : MonoBehaviour
    {
        [SerializeField] private RelationshipsSceneManager _relationshipsSceneManager = null;
        [SerializeField] private int _amount = 5;
        [SerializeField] private PlayerIdsData _playerIdsData = null;

        private const string PlayerNamePrefix ="Player_";
        
        private async void Start()
        {
            //Editor Player will be the last profile created.
            var playerName = $"{PlayerNamePrefix}{_amount - 1}";
            var hasGeneratedPlayerIds = PlayerPrefs.GetString(playerName) != "";
            if (hasGeneratedPlayerIds)
            {
                await LogIn(playerName);
            }
            else
            {
                await GeneratePlayerIds(_amount);
            }

            Debug.Log($"Authenticated <b>{playerName}</b> with Id: <b>{AuthenticationService.Instance.PlayerId}</b>");

            _relationshipsSceneManager.Init();
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
            
            _playerIdsData.Clear();

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
            _playerIdsData.Add(AuthenticationService.Instance.PlayerId);
        }
    }
}