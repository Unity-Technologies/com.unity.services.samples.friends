using TMPro;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text = null;
        [SerializeField] private TMP_InputField m_InputField = null;

        public void Refresh(string name, string id)
        {
            m_Text.text = name;
            m_InputField.text = id;
        }
    }
}