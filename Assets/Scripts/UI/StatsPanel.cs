using System;
using System.Collections.Generic;
using SimpleRPG.Core;
using SimpleRPG.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleRPG.UI
{
    public class StatsPanel : MonoBehaviour
    {
        public bool IsOpened => _isOpened;
        
        [SerializeField] private Transform _mainPanel;

        [SerializeField] private StatUi _healthStat;
        [SerializeField] private StatUi _strengthStat;
        [SerializeField] private StatUi _agilityStat;
        [SerializeField] private StatUi _speedStat;

        [SerializeField] private TMP_Text _availableSkillPoints;
        

        private InputAction _skillsInputAction;
        private bool _isOpened;
        private PlayerStats _stats;
        

        private void OnEnable()
        {
            _skillsInputAction = LevelContext.Instance.PlayerInput.PlayerUI.OpenSkillsTree;
            _skillsInputAction.Enable();
            _skillsInputAction.performed += OnSkillsInputAction;
        }

        private void OnSkillsInputAction(InputAction.CallbackContext obj)
        {
            if(_isOpened)
                ClosePanel();
            else
                OpenPanel();
        }

        private void OnDisable()
        {
            _skillsInputAction.performed -= OnSkillsInputAction;
        }

        private void ClosePanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _isOpened = false;
        }

        private void OpenPanel()
        {
            _mainPanel.gameObject.SetActive(true);
            _isOpened = true;
        }

        
        public void Initialize(ref PlayerStats stats)
        {
            _stats = stats;
            UpdateSkillPointsText();
            _healthStat.Initialize(stats.IncreaseHealth, UpdateSkillPointsText, stats.MaxHealth);
            _strengthStat.Initialize(stats.IncreaseStrength,UpdateSkillPointsText, stats.StrengthMultiplier);
            _agilityStat.Initialize(stats.IncreaseAgility,UpdateSkillPointsText, stats.AgilityMultiplier);
            _speedStat.Initialize(stats.IncreaseMovementSpeed,UpdateSkillPointsText, stats.MaxMovementSpeed);
        }

        private void UpdateSkillPointsText()
        {
            _availableSkillPoints.text = _stats.AvailableSkillPoints.ToString();
        }
        
    }
}