using UnityEngine;

namespace PolarityBreach.Player
{
    [RequireComponent(typeof(PlayerXP))]
    [RequireComponent(typeof(PlayerStatsData))]
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerLevelUpBonus : MonoBehaviour
    {
        [Header("Stat Increase Per Level")]
        [SerializeField] private float attackDamageIncrease = 2f;
        [SerializeField] private float attackSpeedIncrease = 0.05f;
        [SerializeField] private float maxHealthIncrease = 10f;
        [SerializeField] private float attackSpeedDelayReduction = 0.02f; // cooldown between attacks
        [SerializeField] private float polarityCooldownReduction = 0.1f;  // dash/polarity cooldown

        private PlayerXP playerXP;
        private PlayerStatsData stats;
        private PlayerHealth health;

        private void Awake()
        {
            playerXP = GetComponent<PlayerXP>();
            stats = GetComponent<PlayerStatsData>();
            health = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            playerXP.OnLevelUp += HandleLevelUp;
        }

        private void OnDisable()
        {
            playerXP.OnLevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp(int newLevel)
        {
            stats.attackDamage += attackDamageIncrease;
            stats.attackSpeed += attackSpeedIncrease;
            stats.maxHealth += maxHealthIncrease;

            // lower delay = faster attacks, so we subtract, with a safety floor
            stats.attackSpeedDelay = Mathf.Max(0.05f, stats.attackSpeedDelay - attackSpeedDelayReduction);
            stats.polaritySwitchCooldown = Mathf.Max(0.1f, stats.polaritySwitchCooldown - polarityCooldownReduction);

            // sync current health so max health increase doesn't leave player at old value
            health.IncreaseMaxHealth(maxHealthIncrease);

            Debug.Log($"Level {newLevel}! Dmg: {stats.attackDamage}, AtkSpd: {stats.attackSpeed}, " +
                      $"MaxHP: {stats.maxHealth}, AtkDelay: {stats.attackSpeedDelay}, PolarityCD: {stats.polaritySwitchCooldown}");
        }
    }
}