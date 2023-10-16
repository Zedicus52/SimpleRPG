using System;
using SimpleRPG.Core;
using SimpleRPG.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SimpleRPG.UI
{
    public class StatsPanel : MonoBehaviour
    {
        public bool IsOpened => _isOpened;
        
        [SerializeField] private Transform _mainPanel;

        private InputAction _skillsInputAction;

        private bool _isOpened;
        

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

        public void Initialize(PlayerStats stats, int skillsCount)
        {
            Debug.Log(stats);
            Debug.Log($"Skills count: {skillsCount}");
        }
    }
}