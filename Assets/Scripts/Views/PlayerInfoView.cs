using TMPro;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text = null;


        public void Refresh(string text)
        {
            _text.text = text;
        }
    }
}