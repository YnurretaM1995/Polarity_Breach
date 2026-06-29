using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    [RequireComponent(typeof(PolarityComponent))]
    public class PolarityVisual : MonoBehaviour
    {
        [SerializeField] private Material _renderer;
        [SerializeField] private Color _blackColor = new Color(0.12f, 0.12f, 0.16f);
        [SerializeField] private Color _whiteColor = new Color(0.95f, 0.95f, 0.98f);

        private PolarityComponent _polarity;

        private void Awake()
        {
            GetComponents();
        }

        private void GetComponents()
        {
            _polarity = GetComponent<PolarityComponent>();
            if (_renderer == null) _renderer = GetComponentInChildren<Material>();
        }

        private void OnEnable()
        {
            _polarity.OnPolarityChanged += Apply;
            Apply(_polarity.CurrentPolarity); 
        }

        private void OnDisable()
        {
            _polarity.OnPolarityChanged -= Apply;
        }

        private void Apply(Polarity polarity)
        {
            if (_renderer == null) return;
            _renderer.color = polarity == Polarity.Black ? _blackColor : _whiteColor;
        }
    }
}