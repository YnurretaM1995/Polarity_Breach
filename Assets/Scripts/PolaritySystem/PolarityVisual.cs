using UnityEngine;
using System.Collections;

namespace PolarityBreach.PolaritySystem
{
    [RequireComponent(typeof(PolarityComponent))]
    public class PolarityVisual : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Material _blackMaterial;
        [SerializeField] private Material _whiteMaterial;

        [Header("Flash Hit")] 
        [SerializeField] private Material flashMaterial;
        [SerializeField] private float flashDuration = 0.04f;

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
            SetMaterial(targetMaterial);

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
        
        private void SetMaterial(Material material)
        {
            if (material == null) return;

            foreach (Renderer rend in _renderers)
            {
                if (rend == null) continue;

                Material[] newMats = new Material[rend.sharedMaterials.Length];

                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = material;
                }

                rend.materials = newMats;
            }
        }

        public void FlashHit()
        {
            StartCoroutine(FlashHitRoutine());
        }

        private IEnumerator FlashHitRoutine()
        {
            SetMaterial(flashMaterial);
            
            yield return new WaitForSeconds(flashDuration);
            
            Apply(_polarity.CurrentPolarity);
        }
    }
}
