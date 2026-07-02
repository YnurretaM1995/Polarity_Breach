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

        private void OnEnable() => _fireAction.Enable();
        private void OnDisable() => _fireAction.Disable();
        private void OnDestroy() => _fireAction.Dispose();

        private void Update()
        {
            if (_fireAction.IsPressed() && Time.time >= _lastShotTime + _fireRate)
                Shoot();
        }

        private void Shoot()
        {
            if (_projectilePrefab == null) return;

            Vector3 dir = AimDirection();
            GameObject p = Instantiate(_projectilePrefab, _muzzle.position, Quaternion.LookRotation(dir));

            var bulletPolarity = p.GetComponent<PolarityComponent>();
            if (bulletPolarity != null) bulletPolarity.SetPolarity(_polarity.CurrentPolarity);

            _lastShotTime = Time.time;
        }

        private Vector3 AimDirection()
        {
            if (_cam == null || Mouse.current == null) return transform.forward;

            Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane ground = new Plane(Vector3.up, _muzzle.position);
            if (ground.Raycast(ray, out float dist))
            {
                Vector3 hit = ray.GetPoint(dist);
                Vector3 dir = hit - _muzzle.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.001f) return dir.normalized;
            }
            return transform.forward;
        }
    }
}