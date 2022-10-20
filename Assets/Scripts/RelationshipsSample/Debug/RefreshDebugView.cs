using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RefreshDebugView : MonoBehaviour
    {
        public Action OnRefresh;

        [SerializeField] private Button m_Button = null;

        public void Init()
        {
            m_Button.onClick.AddListener(() => OnRefresh?.Invoke());
        }
    }
}