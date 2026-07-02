using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach.PolaritySystem
{
    [RequireComponent(typeof(PolarityComponent))]
    public class PlayerPolarityController : MonoBehaviour
    {
        [Header("Cooldown")]
        [Min(0f)]
        [SerializeField] private float _switchCooldown = 0.25f;

        [Header("Input")]
        [SerializeField] private InputActionReference _switchActionRef;

        private PolarityComponent _polarity;
        private InputAction _switchAction;
        private bool _ownsAction;
        private float _lastSwitchTime = float.NegativeInfinity;
        
        public float SwitchCooldown
        {
            get => _switchCooldown;
            set => _switchCooldown = Mathf.Max(0f, value);
        }
        
        public float CooldownRemaining => Mathf.Max(0f, (_lastSwitchTime + _switchCooldown) - Time.time);
        public bool CanSwitch => Time.time >= _lastSwitchTime + _switchCooldown;

        private void Awake()
        {
            GetComponents();
            RefToSwitchAction();
        }

        private void RefToSwitchAction()
        {
            if (_switchActionRef != null)
            {
                _switchAction = _switchActionRef.action;
            }
            else
            {
                _switchAction = new InputAction("SwitchPolarity", InputActionType.Button);
                _switchAction.AddBinding("<Mouse>/rightButton");
                _switchAction.AddBinding("<Gamepad>/leftTrigger");
                _ownsAction = true;
            }
        }

        private void GetComponents()
        {
            _polarity = GetComponent<PolarityComponent>();
        }

        private void OnEnable()
        {
            _switchAction.performed += OnSwitchPerformed;
            _switchAction.Enable();
        }

        private void OnDisable()
        {
            _switchAction.performed -= OnSwitchPerformed;
            if (_ownsAction) _switchAction.Disable();
        }

        private void OnDestroy()
        {
            if (_ownsAction) _switchAction?.Dispose();
        }

        private void OnSwitchPerformed(InputAction.CallbackContext context) => TrySwitch();

        public bool TrySwitch()
        {
            if (!CanSwitch) return false;
            _polarity.Toggle();
            _lastSwitchTime = Time.time;
            return true;
        }
    }
} 