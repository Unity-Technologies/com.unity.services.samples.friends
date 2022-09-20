using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityGamingServicesUsesCases.Relationships;

public class PlayerIdsGenerator : MonoBehaviour
{
    [SerializeField] private PlayerIdsData m_PlayerIdsData = null;
    [SerializeField] private int m_Amount = 5;

    [SerializeField] private RelationshipsSceneManager m_RelationshipsSceneManager = null;

    private async void Start()
    {
        var playerName =$"Player_{m_Amount-1}";
        var hasGeneratedPlayerIds = PlayerPrefs.GetString(playerName) != "";
        if (hasGeneratedPlayerIds)
        {
            var options = new InitializationOptions();
            var option = options.SetProfile(playerName);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        else
        {
            await Generate();
        }

        Debug.Log($"Authentified as {playerName} : " + AuthenticationService.Instance.PlayerId);

        m_RelationshipsSceneManager.Init();
    }

    public async Task Generate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        PlayerPrefs.DeleteAll();
        m_PlayerIdsData.PlayerIds.Clear();

        for (int i = 0; i < m_Amount; i++)
        {
            AuthenticationService.Instance.SignOut();
            var name = $"Player_{i}";
            AuthenticationService.Instance.SwitchProfile(name);
            var options = new InitializationOptions();
            var option = options.SetProfile(name);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            m_PlayerIdsData.Add(AuthenticationService.Instance.PlayerId);
            //Debug.Log("Saved player : " + AuthenticationService.Instance.PlayerId);
            PlayerPrefs.SetString(name, AuthenticationService.Instance.PlayerId);
        }
    }


    [ContextMenu("Print Player Prefs")]
    public void Print()
    {
        for (int i = 0; i < m_Amount; i++)
        {
            var name = $"Player_{i}";
            var id = PlayerPrefs.GetString(name);
            Debug.Log($"{name} has ID: {id}");
        }
    }
}