using System;
using UnityEngine;

namespace SimpleRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Checkpoint", menuName = "RPG/Checkpoint", order = 0)]
    public class CheckpointSO : ScriptableObject
    {
        public string Id => _id;
        public bool IsReached => _isReached;
        
        private bool _isReached;
        [SerializeField] private string _id;


        [ContextMenu("Generate Id")]
        private void GenerateId()
        {
            if (String.IsNullOrEmpty(_id))
                _id = Guid.NewGuid().ToString();
        }

        public void SetIsReached() => _isReached = true;
    }
}