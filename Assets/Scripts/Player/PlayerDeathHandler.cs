using UnityEngine;
using UnityEngine.SceneManagement;

namespace PolarityBreach.Player
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        private PlayerHealth _health;

        private void Awake()
        {
            _health = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            _health.OnDied += HandleDeath;
        }

        private void OnDisable()
        {
            _health.OnDied -= HandleDeath;
        }

        private void HandleDeath()
        {
            Debug.Log("YOU DIE...");
            RestartScene();
        }

        private void RestartScene()
        {
            Time.timeScale = 1f;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
