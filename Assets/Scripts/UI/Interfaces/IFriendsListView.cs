using System;
using System.Collections.Generic;

namespace UnityGamingServicesUsesCases.Relationships
{

    public interface IFriendsListView
    {
        Action<string> onRemove { get; set; }
        Action<string> onBlock { get; set; }
        void BindList(List<FriendsEntryData> friendEntryDatas);
        void Show();
        void Hide();
        void Refresh();
    }

}

