using System.Collections.Generic;
using UnityEngine;

namespace GlassyCode
{
    public class ItemViewManager
    {
        private readonly GameObject _viewPrefab;
        private readonly RectTransform _contentTransform;

        public List<GameObject> Views { get; } = new();

        public ItemViewManager(GameObject viewPrefab, RectTransform contentTransform)
        {
            _viewPrefab = viewPrefab;
            _contentTransform = contentTransform;
        }

        public void CreateView(ItemModel model)
        {
            var newItem = Object.Instantiate(_viewPrefab, _contentTransform);

            if (!newItem.TryGetComponent<ItemView>(out var itemView))
            {
                Debug.LogError("Failed to get ItemView component on the instantiated prefab.");
                Object.Destroy(newItem);
                return;
            }

            itemView.Toggle.isOn = model.IsChecked;
            itemView.LabelTmp.text = model.Label;
            itemView.Toggle.onValueChanged.AddListener(model.SetIsChecked);
            Views.Add(newItem);
        }

        public void UpdateView(GameObject viewGameObject, ItemModel model)
        {
            if (!viewGameObject.TryGetComponent<ItemView>(out var itemView))
            {
                Debug.LogError("Failed to get ItemView component on the instantiated prefab.");
                Object.Destroy(viewGameObject);
                return;
            }

            itemView.Toggle.onValueChanged.RemoveAllListeners();
            itemView.Toggle.isOn = model.IsChecked;
            itemView.LabelTmp.text = model.Label;
            itemView.Toggle.onValueChanged.AddListener(model.SetIsChecked);
        }
        
        public bool IsViewCreated(int viewIndex)
        {
            return Views.Count > viewIndex;
        }
        
        public void DestroyViews()
        {
            foreach (var viewGameObject in Views)
            {
                Object.Destroy(viewGameObject);
            }
            
            Views.Clear();
        }
    }
}