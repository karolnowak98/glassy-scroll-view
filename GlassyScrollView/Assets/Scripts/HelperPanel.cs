using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GlassyCode
{
    public class HelperPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemModelsNumberTmp;
        [SerializeField] private Button _addRandomModelBtn;
        [SerializeField] private Button _removeCheckedModelsBtn;
        [SerializeField] private GlassyScrollView _glassyScrollView;

        private void Start()
        {
            _itemModelsNumberTmp.text = $"Models number: {_glassyScrollView.CurrentModelsNumber}";
        }

        private void OnEnable()
        {
            _addRandomModelBtn.onClick.AddListener(_glassyScrollView.AddRandomModel);
            _removeCheckedModelsBtn.onClick.AddListener(_glassyScrollView.RemoveCheckedModels);
            
            _glassyScrollView.OnModelsNumberUpdated += UpdateModelsNumberTmp;
        }

        private void OnDisable()
        {
            _addRandomModelBtn.onClick.RemoveAllListeners();
            _removeCheckedModelsBtn.onClick.RemoveAllListeners();
            
            _glassyScrollView.OnModelsNumberUpdated -= UpdateModelsNumberTmp;
        }

        private void UpdateModelsNumberTmp(int modelsNumber) => _itemModelsNumberTmp.text = $"Models number: {modelsNumber}";
    }
}