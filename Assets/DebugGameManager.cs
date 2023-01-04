using Unity.Services.Toolkits.Friends;
using UnityEngine;

/// <summary>
/// Debug Game Manager, we are using this only for initialization.
/// You would replace this with your own.
/// </summary>
public class DebugGameManager : MonoBehaviour
{
    [SerializeField] RelationshipsManager m_RelationshipsManager;

    void Start()
    {
        InitServices();
    }

    /// <summary>
    /// The only service we are dependent on is one that connects playerNames to Unity Authentication ID's
    /// </summary>
    void InitServices()
    {
        var debugSocialProfileService = new DebugSocialProfileService();
        m_RelationshipsManager.Init(debugSocialProfileService);
    }
}
