using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace GlassyCode
{
    [RequireComponent(typeof(ScrollRect))]
    public class GlassyScrollView : MonoBehaviour
    {
        [SerializeField] private GameObject _viewPrefab;
        [SerializeField] private RectTransform _contentTransform;
        [SerializeField, Min(0)] private int _modelsNumber;
        [SerializeField, Min(0)] private int _visibleViewsNumber;

        private readonly List<ItemModel> _models = new();
        private ScrollRect _scrollRect;
        private ItemViewManager _itemViewManager;
        private int _firstVisibleViewIndex;
        private int _lastVisibleViewIndex;

        public int CurrentModelsNumber => _models.Count;

        public event Action<int> OnModelsNumberUpdated;
        
        private void Awake()
        {
            _itemViewManager = new ItemViewManager(_viewPrefab, _contentTransform);
            _scrollRect = GetComponent<ScrollRect>();
            
            CreateModels();
        }

        private void Start()
        {
            CreateViews();
        }

        private void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        }

        private void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveAllListeners();
        }

        private void OnScrollValueChanged(Vector2 value)
        {
            UpdateViews();
        }

        private void CreateModels()
        {
            for (var i = 0; i < _modelsNumber; i++)
            {
                _models.Add(new ItemModel { Label = $"Item {i + 1}", IsChecked = false });
            }
        }

        private void CreateViews()
        {
            _firstVisibleViewIndex = 0;
            _lastVisibleViewIndex = Mathf.Min(_visibleViewsNumber, CurrentModelsNumber);

            for (var i = _firstVisibleViewIndex; i < _lastVisibleViewIndex; i++)
            {
                _itemViewManager.CreateView(_models[i]);
            }
        }
        
        private void UpdateViews()
        {
            UpdateViewsRange();

            var viewIndex = 0;

            for (var i = _firstVisibleViewIndex; i < _lastVisibleViewIndex; i++)
            {
                if (!_itemViewManager.IsViewCreated(viewIndex))
                {
                    _itemViewManager.CreateView(_models[i]);
                }
                
                _itemViewManager.UpdateView(_itemViewManager.Views[viewIndex], _models[i]);
                viewIndex++;
            }
        }

        private void UpdateViewsRange()
        {
            var scrollPos = _scrollRect.verticalNormalizedPosition;
            var remainingModels = CurrentModelsNumber - _visibleViewsNumber;

            _firstVisibleViewIndex = Mathf.Clamp((int)((1 - scrollPos) * remainingModels), 0, Mathf.Max(0, remainingModels));
            _lastVisibleViewIndex = Mathf.Min(_firstVisibleViewIndex + _visibleViewsNumber, CurrentModelsNumber);
        }

        public void RemoveCheckedModels()
        {
            var removedModelsNumber = _models.RemoveAll(itemData => itemData.IsChecked);

            if (removedModelsNumber <= 0) return;

            OnModelsNumberUpdated?.Invoke(CurrentModelsNumber);

            _itemViewManager.DestroyViews();
            CreateViews();
            UpdateViews();
        }

        public void AddRandomModel()
        {
            var randomCheck = RandomUtils.GetRandomBool;
            var randomLabel = $"Item {Random.Range(10000, 20000)}";

            _models.Add(new ItemModel { IsChecked = randomCheck, Label = randomLabel });
            OnModelsNumberUpdated?.Invoke(CurrentModelsNumber);

            UpdateViews();
        }
    }
}