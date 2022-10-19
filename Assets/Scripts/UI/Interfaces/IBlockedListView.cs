using System;
using System.Collections.Generic;


namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IBlockedListView
    {
        Action<string> onUnblock { get; set; }
        void BindList(List<PlayerProfile> playerProfiles);
        void Show();
        void Hide();
        void Refresh();
    }

}

