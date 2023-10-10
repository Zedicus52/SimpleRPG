using System;
using UnityEngine;

namespace SimpleRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Cinematic", menuName = "RPG/Cinematic", order = 0)]
    public class CinematicSO : ScriptableObject
    {
        public string Id => _id;
        public bool IsPlayed => _isPlayed;
        
        private bool _isPlayed;
        [SerializeField] private string _id;


        [ContextMenu("Generate Id")]
        private void GenerateId()
        {
            if (String.IsNullOrEmpty(_id))
                _id = Guid.NewGuid().ToString();
        }

        public void SetIsPlayed() => _isPlayed = true;
    }
}