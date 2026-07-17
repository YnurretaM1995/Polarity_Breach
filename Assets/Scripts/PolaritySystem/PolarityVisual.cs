using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    [RequireComponent(typeof(PolarityComponent))]
    public class PolarityVisual : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Material _blackMaterial;
        [SerializeField] private Material _whiteMaterial;

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
            
            Material targetMaterial = polarity == Polarity.Black ? _blackMaterial : _whiteMaterial;

            foreach (Renderer rend in _renderers)
            {
                if (rend == null) continue;
                
                Material[] newMats = new Material[rend.sharedMaterials.Length];
                
                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = targetMaterial;
                }
                
                rend.materials = newMats;
            }
        }
    }
}
