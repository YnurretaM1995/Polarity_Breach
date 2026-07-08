using PolarityBreach.Enemy;
using PolarityBreach.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach
{
    public class CheatMenu : MonoBehaviour
    {
        [SerializeField] private PlayerStatsData playerStats;
        [SerializeField] private EnemyWaveSpawner waveSpawner;

        private bool showMenu;
        private Vector2 scrollPosition;

        private void Awake()
        {
            if (playerStats == null)
            {
                playerStats = GetComponent<PlayerStatsData>();
            }
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
            {
                showMenu = !showMenu;
            }
        }

        private void OnGUI()
        {
            if (!showMenu) return;
            if (playerStats == null) return;

            GUILayout.BeginArea(new Rect(20, 20, 280, 600), "Debug Menu", GUI.skin.window);
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            GUILayout.Label("Player Stats");
            playerStats.movementSpeed = Slider("Movement Speed", playerStats.movementSpeed, 0f, 20f);
            playerStats.maxHealth = Slider("Max Health", playerStats.maxHealth, 1f, 300f);
            playerStats.polaritySwitchCooldown = Slider("Polarity Cooldown", playerStats.polaritySwitchCooldown, 0f, 5f);

            GUILayout.Space(10);
            
            GUILayout.Label("Dash");
            playerStats.dashUnlocked = GUILayout.Toggle(playerStats.dashUnlocked, "Dash Unlocked");
            playerStats.dashSpeed = Slider("Dash Speed", playerStats.dashSpeed, 0f, 60f);
            playerStats.dashDuration = Slider("Dash Duration", playerStats.dashDuration, 0f, 2f);
            playerStats.dashCooldown = Slider("Dash Cooldown", playerStats.dashCooldown, 0f, 5f);

            GUILayout.Space(10);

            GUILayout.Label("Normal Shot");
            playerStats.attackSpeedDelay = Slider("Attack Speed Delay", playerStats.attackSpeedDelay, 0.01f, 3f);
            playerStats.attackDamage = Slider("Attack Damage", playerStats.attackDamage, 0f, 100f);
            playerStats.attackSpeed = Slider("Projectile Speed", playerStats.attackSpeed, 0f, 200f);
            playerStats.knockBackPower = Slider("Knockback", playerStats.knockBackPower, 0f, 100f);

            GUILayout.Space(10);

            GUILayout.Label("Charge Shot");
            playerStats.chargeShotUnlocked = GUILayout.Toggle(playerStats.chargeShotUnlocked, "Charge Shot Unlocked");
            playerStats.chargeShotDamage = Slider("Charge Damage", playerStats.chargeShotDamage, 0f, 300f);
            playerStats.chargeShotSpeed = Slider("Charge Speed", playerStats.chargeShotSpeed, 0f, 50f);
            playerStats.chargeShotKnockBackPower = Slider("Charge Knockback", playerStats.chargeShotKnockBackPower, 0f, 200f);
            playerStats.chargeTime = Slider("Charge Time", playerStats.chargeTime, 0f, 5f);

            GUILayout.Space(10);
            
            GUILayout.Label("Wave Debug");
            if (GUILayout.Button("Kill all enemies") && waveSpawner != null)
            {
                waveSpawner.DebugCompleteCurrentWave();
            }
            
            GUILayout.Space(10);
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        
        private float Slider(string label, float value, float min, float max)
        {
            GUILayout.Label(label + ": " + value.ToString("0.00"));
            return GUILayout.HorizontalSlider(value, min, max);
        }
    }
}