using System;
using UnityEngine;

namespace PolarityBreach.Player
{
    public class PlayerXP : MonoBehaviour
    {
        public static PlayerXP Instance { get; private set; }

        [Header("XP Settings")]
        [SerializeField] private int currentXP = 0;
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int xpToNextLevel = 100;
        [SerializeField] private float xpCurveMultiplier = 1.25f; // how much harder each level gets

        public int CurrentXP => currentXP;
        public int CurrentLevel => currentLevel;
        public int XPToNextLevel => xpToNextLevel;

        public event Action<int, int> OnXPChanged; // (currentXP, xpToNextLevel)
        public event Action<int> OnLevelUp;         // (newLevel)

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void AddXP(int amount)
        {
            if (amount <= 0) return;

            currentXP += amount;
            Debug.Log($"Gained {amount} XP. Total: {currentXP}/{xpToNextLevel}");

            while (currentXP >= xpToNextLevel)
            {
                LevelUp();
            }

            OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        }

        private void LevelUp()
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpCurveMultiplier);

            Debug.Log($"LEVEL UP! Now level {currentLevel}. Next threshold: {xpToNextLevel}");
            OnLevelUp?.Invoke(currentLevel);
        }
    }
}