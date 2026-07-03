using PolarityBreach.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PolarityBreach.PolaritySystem
{
   
    [RequireComponent(typeof(PolarityComponent))]
    public class TestShooter : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _muzzle;       
        [SerializeField] private float _fireRate = 0.15f;  

        private PolarityComponent _polarity;
        private InputAction _fireAction;
        private float _lastShotTime = float.NegativeInfinity;
        private Camera _cam;

        private void Awake()
        {
            _polarity = GetComponent<PolarityComponent>();
            _cam = Camera.main;
            if (_muzzle == null) _muzzle = transform;

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
            if (_fireAction.IsPressed() && Time.time >= _lastShotTime + _fireRate)
                Shoot();
        }
        
        private void HandlePause(bool paused)
        {
            if (paused) _fireAction.Disable();
            else _fireAction.Enable();
        }

        private void Shoot()
        {
            if (_projectilePrefab == null) return;

            Vector3 dir = transform.forward;
            dir.y = 0f;
            dir.Normalize();
            GameObject p = Instantiate(_projectilePrefab, _muzzle.position, Quaternion.LookRotation(dir));

            var bulletPolarity = p.GetComponent<PolarityComponent>();
            if (bulletPolarity != null) bulletPolarity.SetPolarity(_polarity.CurrentPolarity);

            _lastShotTime = Time.time;
        }

    }
}