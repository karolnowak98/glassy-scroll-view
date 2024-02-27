using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GlassyCode
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelTmp;
        [SerializeField] private Toggle _toggle;

        public TextMeshProUGUI LabelTmp => _labelTmp;
        public Toggle Toggle => _toggle;
    }
}