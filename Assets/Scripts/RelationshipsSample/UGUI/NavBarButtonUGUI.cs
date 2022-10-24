using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class NavBarButtonUGUI : MonoBehaviour
    {
        public Action onSelected;
        [SerializeField] UnityEvent m_OnSelected = null;
        [SerializeField] UnityEvent m_OnDeselected = null;
        [SerializeField] Button m_Button;

        public void Init()
        {
            m_Button.onClick.AddListener(Select);
            Deselect();
        }

        public void Select()
        {
            m_OnSelected?.Invoke();
            onSelected?.Invoke();
        }

        public void Deselect()
        {
            m_OnDeselected?.Invoke();
        }
    }
}