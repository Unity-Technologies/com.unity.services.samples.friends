using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
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
            //Need to initialize before doing anything.
            await UnityServices.InitializeAsync();
            
            if (!m_PlayerProfilesData.Any())
            {
                await GeneratePlayerProfiles(m_Amount);
            }
            
            var playerName = m_PlayerProfilesData.First().Name;
            await m_RelationshipsManager.Init(playerName, this);
            DebugUISetup();
        }

        void DebugUISetup()
        {
            m_LogInDebugView.Init();
            m_LogInDebugView.OnLogIn += m_RelationshipsManager.LogIn;

            m_RefreshDebugView.Init();
            m_RefreshDebugView.OnRefresh += m_RelationshipsManager.RefreshAll;
            m_QuitButton.onClick.AddListener(Application.Quit);
        }


        async Task GeneratePlayerProfiles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                await GeneratePlayerProfile(i);
            }
        }
        async Task GeneratePlayerProfile(int i)
        {
            var playerName = $"{k_PlayerNamePrefix}{i}";
            await UASUtils.SwitchUser(playerName);
            var playerID = AuthenticationService.Instance.PlayerId;
            m_PlayerProfilesData.Add(playerName,playerID);
        }
    }
}
