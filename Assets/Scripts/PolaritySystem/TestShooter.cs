using PolarityBreach.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach.PolaritySystem
{
   
    [RequireComponent(typeof(PolarityComponent))]
    public class TestShooter : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _chargedProjectilePrefab;
        
        [Header("Shoot Settings")]
        [SerializeField] private float _fireRate = 0.15f;
        [SerializeField] private float _chargeTime;
        
        private bool _isCharging;
        private bool _chargeReady;
        private float _chargeStartTime;
        [SerializeField] private Transform _muzzle;
        private PolarityComponent _polarity;
        private InputAction _fireAction;
        private float _lastShotTime = float.NegativeInfinity;
        private Camera _cam;

        private void Awake()
        {
            _polarity = GetComponent<PolarityComponent>();
            _cam = Camera.main;
            if (_muzzle == null) 
                _muzzle = transform;

            _fireAction = new InputAction("Fire", InputActionType.Button);
            _fireAction.AddBinding("<Mouse>/leftButton");
            _fireAction.AddBinding("<Gamepad>/rightTrigger");
        }

        private void OnEnable()
        { 
            _fireAction.Enable();
            PauseMenu.OnPauseChanged += HandlePause;
        }
        private void OnDisable()
        {
            _fireAction.Disable();
            PauseMenu.OnPauseChanged -= HandlePause;
        }
        private void OnDestroy() => _fireAction.Dispose();

        private void Update()
        {
            if (_fireAction.WasPressedThisFrame())
            {
                StartCharging();
                return;
            }
            
            if (_fireAction.WasReleasedThisFrame())
            {
                ReleaseCharge();
            }

            if (_fireAction.IsPressed())
            {
                UpdateCharging();
                return;
            }
            
            AutoFire();
        }
        
        private void HandlePause(bool paused)
        {
            if (paused) _fireAction.Disable();
            else _fireAction.Enable();
        }
        
        private void StartCharging()
        {
            _isCharging = true;
            _chargeReady = false;
            _chargeStartTime = Time.time;
        }

        private void UpdateCharging()
        {
            float holdTime = Time.time - _chargeStartTime;

            if (holdTime >= _chargeTime && !_chargeReady)
            {
                _chargeReady = true;
                Debug.Log("Charge Shot READY!");
            }
        }

        private void ReleaseCharge()
        {
            if (_isCharging && _chargeReady)
            {
                ChargeShot();
            }
            else
            {
                Debug.Log("Charge Shot CANCELLED!");
            }
            
            _isCharging = false;
            _chargeReady = false;
        }
        
        private void Shoot()
        {
            FireProjectile(_projectilePrefab);
        }
        
        private void ChargeShot()
        {
            FireProjectile(_chargedProjectilePrefab);
        }

        private void FireProjectile(GameObject prefab)
        {
            if (prefab == null) return;

            Vector3 dir = transform.forward;
            dir.y = 0f;
            dir.Normalize();

            GameObject p = Instantiate(prefab, _muzzle.position, Quaternion.LookRotation(dir));

            var bulletPolarity = p.GetComponent<PolarityComponent>();
            if (bulletPolarity != null) bulletPolarity.SetPolarity(_polarity.CurrentPolarity);

            _lastShotTime = Time.time;
        }
        
        private void AutoFire()
        {
            if (Time.time >= _lastShotTime + _fireRate)
            {
                Shoot();
            }
        }
    }
}
