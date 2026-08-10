using UnityEngine;
using PolarityBreach.Enemy;
using TMPro;

namespace PolarityBreach.Enemy
{
    public class EnemiesRemainingUI : MonoBehaviour
    {
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private TMP_Text enemyText;
        [SerializeField] private RectTransform panel;

        [Header("Slide")]
        [SerializeField] private Vector2 hiddenPosition = new Vector2(0f, 120f);
        [SerializeField] private Vector2 visiblePosition = new Vector2(0f, -40f);
        [SerializeField] private float slideSpeed = 8f;

        private void Awake()
        {
            if (panel == null)
            {
                panel = GetComponent<RectTransform>();
            }

            panel.anchoredPosition = hiddenPosition;
        }

        private void Update()
        {
            if (enemyPool == null || enemyText == null || panel == null) return;

            int count = enemyPool.ActiveEnemyCount;

            enemyText.text = enemyPool.ActiveEnemyCount.ToString();

            Vector2 targetPosition = count > 0 ? visiblePosition : hiddenPosition;
            panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, targetPosition, slideSpeed * Time.deltaTime);
        }
    }
}
