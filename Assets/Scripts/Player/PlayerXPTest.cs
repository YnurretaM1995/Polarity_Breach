using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach.Player
{
    public class PlayerXPTest : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                PlayerXP.Instance.AddXP(25);
            }
        }
    }
}