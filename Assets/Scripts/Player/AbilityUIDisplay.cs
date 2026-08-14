using UnityEngine;
using UnityEngine.UI;

namespace PolarityBreach.Player
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AbilityUIDisplay : MonoBehaviour
    {
        public enum AbilityType { Polarity, Dash }
        
        [SerializeField] private AbilityType trackedAbility;
        [SerializeField] private PlayerStatsData statsData;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private bool hideIconWhenLocked = true;

        private float currentCooldownTimer = 0f;
        private bool isOnCooldown = false;
        private CanvasGroup canvasGroup;

        void Start()
        {
            statsData = FindFirstObjectByType<PlayerStatsData>();
            canvasGroup = GetComponent<CanvasGroup>();
            cooldownOverlay.fillAmount = 0f;
        }

        void Update()
        {
            bool isUnlocked = CheckUnlockStatus();
            HandleVisibility(isUnlocked);
            HandleCooldownProcess(isUnlocked);
        }

        private bool CheckUnlockStatus()
        {
            return (trackedAbility == AbilityType.Polarity) || statsData.dashUnlocked;
        }

        private void HandleVisibility(bool isUnlocked)
        {
            if (hideIconWhenLocked && trackedAbility == AbilityType.Dash)
            {
                canvasGroup.alpha = isUnlocked ? 1f : 0f;
                canvasGroup.interactable = isUnlocked;
                canvasGroup.blocksRaycasts = isUnlocked;
            }
            else if (!hideIconWhenLocked)
            {
                canvasGroup.alpha = 1f;
            }
        }

        private void HandleCooldownProcess(bool isUnlocked)
        {
            if (!isUnlocked)
            {
                cooldownOverlay.fillAmount = 0f;
                return;
            }

            if (isOnCooldown)
            {
                TickCooldownTimer();
            }
        }

        private void TickCooldownTimer()
        {
            currentCooldownTimer -= Time.deltaTime;
            float maxDuration = GetMaxCooldownDuration();

            if (currentCooldownTimer <= 0f)
            {
                ResetCooldown();
            }
            else
            {
                cooldownOverlay.fillAmount = currentCooldownTimer / maxDuration;
            }
        }
        
        public void StartCooldownUI()
        {
            if (canvasGroup.alpha <= 0f) return;
            if (trackedAbility == AbilityType.Dash && !statsData.dashUnlocked) return;

            if (!isOnCooldown)
            {
                isOnCooldown = true;
                currentCooldownTimer = GetMaxCooldownDuration();
            }
        }

        private float GetMaxCooldownDuration()
        {
            switch (trackedAbility)
            {
                case AbilityType.Polarity:
                    return statsData.polaritySwitchCooldown;
                case AbilityType.Dash:
                    return statsData.dashCooldown;
                default:
                    return 0f;
            }
        }

        private void ResetCooldown()
        {
            currentCooldownTimer = 0f;
            isOnCooldown = false; 
            cooldownOverlay.fillAmount = 0f;
        }
    }
}
