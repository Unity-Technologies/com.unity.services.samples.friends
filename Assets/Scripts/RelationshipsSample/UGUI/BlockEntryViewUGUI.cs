using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Relationships.UGUI
{
    public class BlockEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_NameText = null;

        public Button unblockButton = null;

        public void Init(string playerName)
        {
            m_NameText.text = playerName;
        }
    }
}