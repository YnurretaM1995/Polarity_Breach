using UnityEngine;

namespace PolarityBreach.Player
{
    public class PlayerStatsData : MonoBehaviour
    {
        [Header("Player Stats")] 
        public float movementSpeed;
        public float maxHealth;
        public float polaritySwitchDelay;

        [Header("Player Skills Unlocks")] 
        public bool dashUnlocked;
        
        [Header("Weapon Stats")] 
        public float attackSpeed;
        public float attackPower;
        public float knockBackPower;
        public float chargeTime;

        [Header("Weapon Skills Unlocks")] 
        public bool chargeShotUnlocked;
    }
}