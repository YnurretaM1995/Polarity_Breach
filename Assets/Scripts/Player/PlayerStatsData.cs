using UnityEngine;

namespace PolarityBreach.Player
{
    public class PlayerStatsData : MonoBehaviour
    {
        [Header("Player Stats")] 
        public float movementSpeed;
        public float maxHealth;
        public float polaritySwitchCooldown;
        
        [Header("Weapon Stats")] 
        public float attackSpeedDelay;
        public float attackDamage;
        public float attackSpeed;
        public float knockBackPower;

        [Header("Unlock Dash Skill")]
        public bool dashUnlocked;
        [Header("Dash Stats")] 
        public float dashSpeed;
        public float dashDuration;
        public float dashCooldown;
        
        [Header("Unlock ChargeShot Skill")]
        public bool chargeShotUnlocked;
        [Header("ChargeShot Stats")] 
        public float chargeShotDamage;
        public float chargeShotSpeed;
        public float chargeShotKnockBackPower;
        public float chargeTime;

        [Header("Debug")] 
        public bool godMode;
    }
}