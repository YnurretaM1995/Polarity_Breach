using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    [RequireComponent(typeof(PolarityComponent))]
    public class PolarityVisual : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField, ColorUsage(true, true)] private Color _blackColor = new Color(0.12f, 0.12f, 0.16f);
        [SerializeField, ColorUsage(true, true)] private Color _whiteColor = new Color(0.95f, 0.95f, 0.98f);

        private PolarityComponent _polarity;

        private void Awake()
        {
            GetComponents();
        }

        private void GetComponents()
        {
            _polarity = GetComponent<PolarityComponent>();
            if (_renderers == null || _renderers.Length == 0)
                _renderers = GetComponentsInChildren<Renderer>();
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
            if (_renderers == null) return;
            Color nuevo = polarity == Polarity.Black ? _blackColor : _whiteColor;

            foreach (Renderer rend in _renderers)
            {
                if (rend == null) continue;

                Material[] mats = rend.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    Material m = mats[i];

                    if (m.HasProperty("_TintColor")) m.SetColor("_TintColor", nuevo);
                    if (m.HasProperty("_TintAmount")) m.SetFloat("_TintAmount", 0.7f);
                    if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", nuevo);
                    if (m.HasProperty("_Color")) m.SetColor("_Color", nuevo);
                }
            }
        }
    }
}