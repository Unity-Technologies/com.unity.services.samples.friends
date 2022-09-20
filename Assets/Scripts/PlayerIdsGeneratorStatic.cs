using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityGamingServicesUsesCases.Relationships;

public static class PlayerIdsGeneratorStatic
{
    //[SerializeField] private PlayerIdsData m_PlayerIdsData = null;
    private const int PlayerAmount = 5;


#if UNITY_EDITOR
    [MenuItem("SampleTools/GeneratePlayerIds")]
    public static async void GeneratePlayerIds()
    {
        if (Application.isPlaying)
            return;

        // var rel = Object.FindObjectOfType<RelationshipsSceneManager>(true).gameObject;
        // rel.SetActive(false);
        UnityEditor.EditorApplication.isPlaying = true;
        //await Task.Delay(1000);
        await GenerateAsync();
        UnityEditor.EditorApplication.isPlaying = false;
       //rel.SetActive(true);
    }
#endif

    private static async Task GenerateAsync()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();
        PlayerPrefs.DeleteAll();
        
        for (int i = 0; i < PlayerAmount; i++)
        {
            AuthenticationService.Instance.SignOut();
            var name = $"Player_{i}";
            AuthenticationService.Instance.SwitchProfile(name);
            var options = new InitializationOptions();
            var option = options.SetProfile(name);
            await UnityServices.InitializeAsync(option);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //m_PlayerIdsData.Add(AuthenticationService.Instance.PlayerId);
            Debug.Log("Saved player : " + AuthenticationService.Instance.PlayerId);
            PlayerPrefs.SetString(name, AuthenticationService.Instance.PlayerId);
        }
    }


    [MenuItem("SampleTools/Print Player Ids")]
    public static void Print()
    {
        for (int i = 0; i < PlayerAmount; i++)
        {
            var name = $"Player_{i}";
            var id = PlayerPrefs.GetString(name);
            Debug.Log($"{name} has ID: {id}");
        }
    }
    
    [MenuItem("SampleTools/Delete Player Prefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}