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

        
        public void Initialize(Func<float> mainAction, Action updateText, float value)
        {
            _valueText.text = $"{value:f2}";
            _addButton.onClick.AddListener(() =>
            {
                float newValue = mainAction();
                _valueText.text = $"{newValue:f2}";
                updateText();
            });
        }

        private void OnDisable()
        {
            _addButton.onClick.RemoveAllListeners();
        }
    }
}