using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleRPG.UI
{
    public class StatUi : MonoBehaviour
    {
        [SerializeField] private Button _addButton;
        [SerializeField] private TMP_Text _valueText;

        
        public void Initialize(Func<float> action, float value)
        {
            _valueText.text = $"{value:f2}";
            _addButton.onClick.AddListener(() =>
            {
                float newValue = action();
                _valueText.text = $"{newValue:f2}";
            });
        }

        private void OnDisable()
        {
            _addButton.onClick.RemoveAllListeners();
        }
    }
}