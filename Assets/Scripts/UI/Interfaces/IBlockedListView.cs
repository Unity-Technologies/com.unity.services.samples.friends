using System;
using System.Collections.Generic;


namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IBlockedListView
    {
        Action<string> onUnBlock { get; set; }
        void BindList(List<PlayerProfile> playerProfiles);
        void Show();
        void Hide();
        void Refresh();
    }

}

