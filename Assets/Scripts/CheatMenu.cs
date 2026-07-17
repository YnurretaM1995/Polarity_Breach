// using PolarityBreach.Enemy;
// using PolarityBreach.Player;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace PolarityBreach
// {
//     public class CheatMenu : MonoBehaviour
//     {
//         [SerializeField] private PlayerStatsData playerStats;
//         [SerializeField] private EnemyWaveSpawner waveSpawner;
//
//         private bool showMenu;
//         private Vector2 scrollPosition;
//         private GUIStyle boldStyle;
//
//         private void Awake()
//         {
//             if (playerStats == null)
//             {
//                 playerStats = GetComponent<PlayerStatsData>();
//             }
//         }
//
//         private void Update()
//         {
//             if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
//             {
//                 ToggleMenu();
//             }
//         }
//
//         private void OnGUI()
//         {
//             if (!showMenu) return;
//             if (playerStats == null) return;
//
//             if (boldStyle == null)
//             {
//                 boldStyle = new GUIStyle(GUI.skin.label);
//                 boldStyle.fontStyle = FontStyle.Bold;
//             }
//
//             GUILayout.BeginArea(new Rect(20, 20, 280, 600), "Debug Menu", GUI.skin.window);
//             
//             scrollPosition = GUILayout.BeginScrollView(scrollPosition);
//             
//             GUILayout.Label("Player Stats",  boldStyle);
//             playerStats.movementSpeed = Slider("Movement Speed", playerStats.movementSpeed, 0f, 20f);
//             playerStats.maxHealth = Slider("Max Health", playerStats.maxHealth, 1f, 300f);
//             playerStats.godMode = GUILayout.Toggle(playerStats.godMode, "God Mode");
//             playerStats.polaritySwitchCooldown = Slider("Polarity Cooldown", playerStats.polaritySwitchCooldown, 0f, 5f);
//
//             GUILayout.Space(20);
//             
//             GUILayout.Label("Dash", boldStyle);
//             playerStats.dashUnlocked = GUILayout.Toggle(playerStats.dashUnlocked, "Dash Unlocked");
//             playerStats.dashSpeed = Slider("Dash Speed", playerStats.dashSpeed, 0f, 60f);
//             playerStats.dashDuration = Slider("Dash Duration", playerStats.dashDuration, 0f, 2f);
//             playerStats.dashCooldown = Slider("Dash Cooldown", playerStats.dashCooldown, 0f, 5f);
//
//             GUILayout.Space(20);
//
//             GUILayout.Label("Normal Shot", boldStyle);
//             playerStats.attackSpeedDelay = Slider("Attack Speed Delay", playerStats.attackSpeedDelay, 0.01f, 3f);
//             playerStats.attackDamage = Slider("Attack Damage", playerStats.attackDamage, 0f, 100f);
//             playerStats.attackSpeed = Slider("Projectile Speed", playerStats.attackSpeed, 0f, 200f);
//             playerStats.knockBackPower = Slider("Knockback", playerStats.knockBackPower, 0f, 100f);
//
//             GUILayout.Space(20);
//
//             GUILayout.Label("Charge Shot", boldStyle);
//             playerStats.chargeShotUnlocked = GUILayout.Toggle(playerStats.chargeShotUnlocked, "Charge Shot Unlocked");
//             playerStats.chargeShotDamage = Slider("Charge Damage", playerStats.chargeShotDamage, 0f, 300f);
//             playerStats.chargeShotSpeed = Slider("Charge Speed", playerStats.chargeShotSpeed, 0f, 50f);
//             playerStats.chargeShotKnockBackPower = Slider("Charge Knockback", playerStats.chargeShotKnockBackPower, 0f, 200f);
//             playerStats.chargeTime = Slider("Charge Time", playerStats.chargeTime, 0f, 5f);
//
//             GUILayout.Space(20);
//             
//             GUILayout.Label("Wave Debug", boldStyle);
//             if (GUILayout.Button("Kill all enemies") && waveSpawner != null)
//             {
//                 waveSpawner.DebugCompleteCurrentWave();
//             }
//             
//             GUILayout.Space(10);
//             
//             GUILayout.EndScrollView();
//             GUILayout.EndArea();
//         }
//         public void ToggleMenu()
//         {
//             showMenu = !showMenu;
//         }
//
//         public void OpenMenu()
//         {
//             showMenu = true;
//         }
//         
//         public void CloseMenu()
//         {
//             showMenu = false;
//         }
//         
//         private float Slider(string label, float value, float min, float max)
//         {
//             GUILayout.Label(label + ": " + value.ToString("0.00"));
//             return GUILayout.HorizontalSlider(value, min, max);
//         }
//     }
// }
using PolarityBreach.Enemy;
using PolarityBreach.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach
{
    public class CheatMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStatsData playerStats;
        [SerializeField] private EnemyWaveSpawner waveSpawner;

        [Header("Boss Debug")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform bossRoomPoint;
        [SerializeField] private GameObject bossObject;

        [Header("Window")]
        [SerializeField] private float windowWidth = 300f;
        [SerializeField] private float windowHeight = 600f;

        [Header("Gamepad Navigation")]
        [Tooltip("Slider range is divided into this many steps. One D-Pad press = one step.")]
        [SerializeField] private int sliderSteps = 20;

        private bool showMenu;
        private Vector2 scrollPosition;

        private GUIStyle boldStyle;
        private GUIStyle selectedStyle;

        private int selectedIndex;
        private int itemCount;
        private int currentIndex;

        private Rect selectedRect;
        private bool selectedRectValid;
        private float scrollAreaHeight;

        // Slider input read in Update, consumed in OnGUI
        private int pendingSliderDir; // -1 left, +1 right, 0 none

        // Only auto-scroll when the D-Pad moved the selection, not the mouse
        private bool navigatedWithGamepad;

        private void Awake()
        {
            // This script no longer lives on the player, so everything is assigned in the inspector
            if (playerStats == null)
                Debug.LogWarning("CheatMenu: playerStats is not assigned.");

            // Boss stays off until the cheat is used
            if (bossObject != null)
                bossObject.SetActive(false);
        }

        private void Update()
        {
            bool keyboardToggle = Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame;
            bool gamepadToggle = Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame;

            if (keyboardToggle || gamepadToggle)
                ToggleMenu();

            if (showMenu)
                HandleNavigation();
        }

        // All gamepad input is read here, exactly once per frame
        private void HandleNavigation()
        {
            Gamepad pad = Gamepad.current;
            if (pad == null) return;

            if (pad.dpad.up.wasPressedThisFrame)
            {
                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                navigatedWithGamepad = true;
            }

            if (pad.dpad.down.wasPressedThisFrame)
            {
                selectedIndex = Mathf.Min(Mathf.Max(0, itemCount - 1), selectedIndex + 1);
                navigatedWithGamepad = true;
            }

            // One press = one step. Holding does nothing until released and pressed again.
            pendingSliderDir = 0;
            if (pad.dpad.right.wasPressedThisFrame) pendingSliderDir = 1;
            if (pad.dpad.left.wasPressedThisFrame) pendingSliderDir = -1;

            // A button handled here, so it fires exactly once per press
            if (pad.buttonSouth.wasPressedThisFrame)
                Confirm(selectedIndex);
        }

        // Fires once per A press. Indices must match the draw order in OnGUI.
        private void Confirm(int index)
        {
            switch (index)
            {
                case 2:  playerStats.godMode = !playerStats.godMode; break;
                case 4:  playerStats.dashUnlocked = !playerStats.dashUnlocked; break;
                case 12: playerStats.chargeShotUnlocked = !playerStats.chargeShotUnlocked; break;
                case 17: if (waveSpawner != null) waveSpawner.DebugCompleteCurrentWave(); break;
                case 18: GoToBossFight(); break;
            }
        }

        // ---- Boss teleport ----

        private void GoToBossFight()
        {
            if (player != null && bossRoomPoint != null)
                StartCoroutine(TeleportRoutine());
            else if (bossObject != null)
                bossObject.SetActive(true);
        }

        private System.Collections.IEnumerator TeleportRoutine()
        {
            GameObject playerObj = player.gameObject;

            // Safe to disable: this coroutine runs on this object, not on the player
            playerObj.SetActive(false);

            yield return new WaitForSecondsRealtime(0.1f);

            // Move while it's off, so no collider can interfere
            player.position = bossRoomPoint.position;

            Rigidbody rb = playerObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = bossRoomPoint.position;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            playerObj.SetActive(true);

            if (bossObject != null)
                bossObject.SetActive(true);
        }

        // ---- GUI ----

        private void OnGUI()
        {
            if (!showMenu || playerStats == null) return;

            BuildStyles();
            currentIndex = 0;

            GUILayout.BeginArea(new Rect(20, 20, windowWidth, windowHeight), "Debug Menu", GUI.skin.window);
            GUILayout.Space(20); // room for the window title

            scrollAreaHeight = windowHeight - 40f;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.Label("Player Stats", boldStyle);
            playerStats.movementSpeed = NavSlider("Movement Speed", playerStats.movementSpeed, 0f, 20f);        // 0
            playerStats.maxHealth = NavSlider("Max Health", playerStats.maxHealth, 1f, 300f);                   // 1
            playerStats.godMode = NavToggle(playerStats.godMode, "God Mode");                                   // 2
            playerStats.polaritySwitchCooldown = NavSlider("Polarity Cooldown", playerStats.polaritySwitchCooldown, 0f, 5f); // 3

            GUILayout.Space(20);

            GUILayout.Label("Dash", boldStyle);
            playerStats.dashUnlocked = NavToggle(playerStats.dashUnlocked, "Dash Unlocked");                    // 4
            playerStats.dashSpeed = NavSlider("Dash Speed", playerStats.dashSpeed, 0f, 60f);                    // 5
            playerStats.dashDuration = NavSlider("Dash Duration", playerStats.dashDuration, 0f, 2f);            // 6
            playerStats.dashCooldown = NavSlider("Dash Cooldown", playerStats.dashCooldown, 0f, 5f);            // 7

            GUILayout.Space(20);

            GUILayout.Label("Normal Shot", boldStyle);
            playerStats.attackSpeedDelay = NavSlider("Attack Speed Delay", playerStats.attackSpeedDelay, 0.01f, 3f); // 8
            playerStats.attackDamage = NavSlider("Attack Damage", playerStats.attackDamage, 0f, 100f);          // 9
            playerStats.attackSpeed = NavSlider("Projectile Speed", playerStats.attackSpeed, 0f, 200f);         // 10
            playerStats.knockBackPower = NavSlider("Knockback", playerStats.knockBackPower, 0f, 100f);          // 11

            GUILayout.Space(20);

            GUILayout.Label("Charge Shot", boldStyle);
            playerStats.chargeShotUnlocked = NavToggle(playerStats.chargeShotUnlocked, "Charge Shot Unlocked"); // 12
            playerStats.chargeShotDamage = NavSlider("Charge Damage", playerStats.chargeShotDamage, 0f, 300f);  // 13
            playerStats.chargeShotSpeed = NavSlider("Charge Speed", playerStats.chargeShotSpeed, 0f, 50f);      // 14
            playerStats.chargeShotKnockBackPower = NavSlider("Charge Knockback", playerStats.chargeShotKnockBackPower, 0f, 200f); // 15
            playerStats.chargeTime = NavSlider("Charge Time", playerStats.chargeTime, 0f, 5f);                  // 16

            GUILayout.Space(20);

            GUILayout.Label("Wave Debug", boldStyle);
            if (NavButton("Kill all enemies") && waveSpawner != null)                                           // 17
                waveSpawner.DebugCompleteCurrentWave();

            GUILayout.Space(10);

            GUILayout.Label("Boss Debug", boldStyle);
            if (NavButton("Go to Boss Fight"))                                                                  // 18
                GoToBossFight();

            GUILayout.Space(10);

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            itemCount = currentIndex;
            FollowSelectionWithScroll();

            if (Event.current.type == EventType.Repaint)
                pendingSliderDir = 0;
        }

        private void BuildStyles()
        {
            if (boldStyle != null) return;

            boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

            // Highlight box drawn around the selected item
            Texture2D highlight = new Texture2D(1, 1);
            highlight.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.22f));
            highlight.Apply();

            selectedStyle = new GUIStyle(GUI.skin.box);
            selectedStyle.normal.background = highlight;
        }

        // Keeps the selected item inside the visible scroll area.
        // Only runs when the D-Pad moved the selection, so it doesn't fight the mouse wheel.
        private void FollowSelectionWithScroll()
        {
            if (!navigatedWithGamepad) return;
            if (!selectedRectValid || Event.current.type != EventType.Repaint) return;

            float itemTop = selectedRect.y;
            float itemBottom = selectedRect.y + selectedRect.height;

            if (itemTop < scrollPosition.y)
                scrollPosition.y = itemTop - 10f;
            else if (itemBottom > scrollPosition.y + scrollAreaHeight)
                scrollPosition.y = itemBottom - scrollAreaHeight + 10f;

            scrollPosition.y = Mathf.Max(0f, scrollPosition.y);
            selectedRectValid = false;
            navigatedWithGamepad = false;
        }

        private bool IsSelected() => currentIndex == selectedIndex;

        private void CaptureSelectedRect(bool selected)
        {
            if (!selected || Event.current.type != EventType.Repaint) return;
            selectedRect = GUILayoutUtility.GetLastRect();
            selectedRectValid = true;
        }

        // If the mouse is hovering the item just drawn, select it
        private void CheckMouseHover(int myIndex)
        {
            if (Event.current.type != EventType.Repaint) return;

            Rect r = GUILayoutUtility.GetLastRect();
            if (r.Contains(Event.current.mousePosition))
                selectedIndex = myIndex;
        }

        // ---- Navigable controls (work with both mouse and gamepad) ----

        private float NavSlider(string label, float value, float min, float max)
        {
            int myIndex = currentIndex;
            bool selected = IsSelected();

            if (selected) GUILayout.BeginVertical(selectedStyle);

            GUILayout.Label((selected ? "> " : "   ") + label + ": " + value.ToString("0.00"));
            value = GUILayout.HorizontalSlider(value, min, max);

            if (selected && pendingSliderDir != 0)
            {
                float step = (max - min) / Mathf.Max(1, sliderSteps);
                value = Mathf.Clamp(value + step * pendingSliderDir, min, max);
            }

            if (selected) GUILayout.EndVertical();

            CheckMouseHover(myIndex);
            CaptureSelectedRect(selected);
            currentIndex++;
            return value;
        }

        private bool NavToggle(bool value, string label)
        {
            int myIndex = currentIndex;
            bool selected = IsSelected();

            if (selected) GUILayout.BeginVertical(selectedStyle);

            // Only draws. The A button is handled in Update via Confirm().
            value = GUILayout.Toggle(value, (selected ? "> " : "   ") + label);

            if (selected) GUILayout.EndVertical();

            CheckMouseHover(myIndex);
            CaptureSelectedRect(selected);
            currentIndex++;
            return value;
        }

        private bool NavButton(string label)
        {
            int myIndex = currentIndex;
            bool selected = IsSelected();
            bool pressed = false;

            if (selected) GUILayout.BeginVertical(selectedStyle);

            // Mouse click only. The A button is handled in Update via Confirm().
            if (GUILayout.Button((selected ? "> " : "   ") + label))
                pressed = true;

            if (selected) GUILayout.EndVertical();

            CheckMouseHover(myIndex);
            CaptureSelectedRect(selected);
            currentIndex++;
            return pressed;
        }

        public void ToggleMenu() => showMenu = !showMenu;
        public void OpenMenu() => showMenu = true;
        public void CloseMenu() => showMenu = false;
    }
}