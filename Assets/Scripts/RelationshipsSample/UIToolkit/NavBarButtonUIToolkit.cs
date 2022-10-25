using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class NavBarButtonUIToolkit
    {
        public Action onSelected;

        Button m_Button;

        public NavBarButtonUIToolkit(VisualElement root, string parent)
        {
            var relationshipsBarView = root.Q(parent);
            m_Button = relationshipsBarView.Q<Button>(parent);
            m_Button.RegisterCallback<ClickEvent>((_) => Select());
            Deselect();
        }

        void Select()
        {
            m_Button.style.backgroundColor = Color.magenta;
            m_Button.style.unityBackgroundImageTintColor = Color.cyan;
            onSelected?.Invoke();
        }

        public void Deselect()
        {
            m_Button.style.backgroundColor = Color.blue;
            m_Button.style.unityBackgroundImageTintColor = Color.white;
        }
    }
}