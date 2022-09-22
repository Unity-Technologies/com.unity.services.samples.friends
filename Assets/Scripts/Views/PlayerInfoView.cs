using TMPro;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private TMP_InputField _inputField = null;


        public void Refresh(string name,string id)
        {
            _text.text = name;
            _inputField.text = id;
        }
    }
}