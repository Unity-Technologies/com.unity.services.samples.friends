using System;
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
            m_Button.style.backgroundColor = ColorUtils.SelectedNavBarTabColor;
            m_Button.style.unityBackgroundImageTintColor = ColorUtils.SelectedNavBarIconColor;
            onSelected?.Invoke();
        }

        public void Deselect()
        {
            m_Button.style.backgroundColor = ColorUtils.DefaultNavBarTabColor;
            m_Button.style.unityBackgroundImageTintColor = ColorUtils.DefaultNavBarIconColor;
        }
    }
}