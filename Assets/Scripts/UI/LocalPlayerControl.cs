using System;

using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class LocalPlayerControl : UIBaseControl
    {
        public Action<string> OnStatusChanged { get; set; }

        public override string ViewRootName => "local-player-entry";

        public PlayerEntryControl PlayerEntryControl { get; private set; }
        public LocalPlayerControl(VisualElement documentParent)
            : base(documentParent)
        {
        }

        protected override void SetVisualElements(){
            PlayerEntryControl = new PlayerEntryControl(m_ViewRoot);
            PlayerEntryControl.PlayerStatusDropDown.visible = false;
        }

        protected override void RegisterButtonCallbacks()
        {
            PlayerEntryControl.PlayerStatusDropDown.RegisterValueChangedCallback(value => { OnStatusChanged?.Invoke(value.newValue);});
        }
    }

}
