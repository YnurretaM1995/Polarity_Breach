using System;
using UnityEngine;

namespace PolarityBreach.Player
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        public static bool IsPaused { get; private set; }
        public static event Action<bool> OnPauseChanged;

        private PlayerInputActions controls;

        private void Awake()
        {
            controls = new PlayerInputActions();
            controls.Player.Pause.performed += ctx => TogglePause();
        }

        private void OnEnable()
        {
            controls.Player.Pause.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Pause.Disable();
        }

        private void TogglePause()
        {
            if (IsPaused) Resume();
            else Pause();
        }

        private void Pause()
        {
            IsPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            OnPauseChanged?.Invoke(true);
        }

        private void Resume()
        {
            IsPaused = false;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            OnPauseChanged?.Invoke(false);
        }
    }
}