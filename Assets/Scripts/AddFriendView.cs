using System;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendView : MonoBehaviour
{
    public Action<string> OnAddFriendRequested;

    [SerializeField] private Button m_Button = null;
    [SerializeField] private PlayerIdsData m_PlayerData = null;
    private void Awake()
    {
        m_Button.onClick.AddListener(()=>OnAddFriendRequested?.Invoke(m_PlayerData[0]));
    }

}
