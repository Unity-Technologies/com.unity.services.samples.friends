using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Friends
{
    /// <summary>
    /// There is no Unity Player Profile service available at the time we created this Toolkit.
    /// So, to easily test the friends API, we made this simplified local social profile "Service".
    /// It logs in as various
    /// </summary>
    public class DebugSocialProfileService : MonoBehaviour, ISocialProfileService
    {
        [SerializeField] RefreshDebugView m_RefreshDebugView;
        [SerializeField] Button m_QuitButton;

        [SerializeField] RelationshipsManager m_RelationshipsManager = null;

        const string k_PlayerNamePrefix = "Player_";

        public string GetName(string id)
        {
            //This is where we would put in calls to social profile services to match our UAS ID to Profile ID's to get information outside the friends service.
            return $"{k_PlayerNamePrefix}{id}";
        }

        async void Start()
        {
            await m_RelationshipsManager.Init(this);
            DebugUISetup();
        }

        void DebugUISetup()
        {
            m_RefreshDebugView.Init();
            m_RefreshDebugView.OnRefresh += m_RelationshipsManager.RefreshAll;
            m_QuitButton.onClick.AddListener(Application.Quit);
        }


    }


    [System.Serializable]
    public class PlayerProfile
    {
        //Decorating with [field: SerializeField] is shorthand for something like:
        //  public string Name => m_Name;
        //  [SerializeField]
        //  string m_Name;
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string Id { get; private set; }


        public PlayerProfile(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name} , Id :{Id}";
        }
    }

}
