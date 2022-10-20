using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace UnityGamingServicesUsesCases.Relationships
{

    /// <summary>
    /// There is no Unity Player Profile service available at the time we created this Toolkit.
    /// So, to easily test the friends API, we made this simplified local social profile "Service".
    /// It logs in as various
    /// </summary>
    public class DebugSocialProfileService : MonoBehaviour, ISocialProfileService
    {

        [Header("Debug Profiles")]
        [SerializeField]
        PlayerProfilesData m_PlayerProfilesData;

        [Header("Debug UI")]
        [SerializeField]
        LogInDebugView m_LogInDebugView;

        [SerializeField]
        RefreshDebugView m_RefreshDebugView;
        [SerializeField]
        Button m_QuitButton;

        [FormerlySerializedAs("m_RelationshipManager")]
        [FormerlySerializedAs("m_RelationshipsSceneManager")]
        [SerializeField]
        RelationshipsManager m_RelationshipsManager = null;
        [SerializeField]
        int m_Amount = 5;

        const string k_PlayerNamePrefix = "Player_";

        public string GetName(string playerID)
        {
            return m_PlayerProfilesData.GetName(playerID);
        }

        async void Start()
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

            await m_RelationshipsManager.Init(playerName, this);
            DebugUISetup();
        }

        void DebugUISetup()
        {
            m_LogInDebugView.Init();
            m_LogInDebugView.OnLogIn += m_RelationshipsManager.LogIn;

            m_RefreshDebugView.Init();
            m_RefreshDebugView.OnRefresh += m_RelationshipsManager.RefreshAll;
            m_QuitButton.onClick.AddListener(QuitAsync);
        }

        async Task GeneratePlayerProfiles(int amount)
        {
            //Need to initialize before doing anything.
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            for (int i = 0; i < amount; i++)
            {
                await GeneratePlayerProfile(i);
            }
        }

        async Task GeneratePlayerProfile(int i)
        {
            var playerName = $"{k_PlayerNamePrefix}{i}";
            await UASUtils.SwitchUser(playerName);
            m_PlayerProfilesData.Add(playerName, AuthenticationService.Instance.PlayerId);
        }


        //Bug - Workaround for an API issue, should be removed with next API update
        async void QuitAsync()
        {
            Friends.Instance.Dispose();
            await Task.Delay(1000);
            Application.Quit();
        }
    }
}
