using System;
using System.Collections.Generic;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRequestListView
    {
        Action<string> onAccept { get; set; }
        Action<string> onDecline { get; set; }
        Action<string> onBlock { get; set; }
        void BindList(List<PlayerProfile> playerProfiles);
        void Show();
        void Hide();
        void Refresh();
    }
}
