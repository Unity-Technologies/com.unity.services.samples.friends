using System;
using System.Collections.Generic;


namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IBlockedListView : IListView
    {
        Action<string> onUnblock { get; set; }
        void BindList(List<PlayerProfile> playerProfiles);
    }

}

