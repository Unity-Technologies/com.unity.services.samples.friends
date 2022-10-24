using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class NavBarButtonUIToolkit
    {
        public Action onSelected;

        private Button m_Button;

        public NavBarButtonUIToolkit(VisualElement root, string parent)
        {
            var relationshipsBarView = root.Q(parent);
            m_Button = relationshipsBarView.Q<Button>(parent);
            m_Button.RegisterCallback<ClickEvent>((_) => { onSelected?.Invoke(); });
            Deselect();
        }

        public void Select()
        {
            m_Button.style.backgroundColor = Color.magenta;
            m_Button.style.unityBackgroundImageTintColor = Color.cyan;
            //enable selected visuals
            onSelected?.Invoke();
        }

        public void Deselect()
        {
            m_Button.style.backgroundColor = Color.blue;
            m_Button.style.unityBackgroundImageTintColor = Color.white;
            //disable selected visuals
        }
    }
}