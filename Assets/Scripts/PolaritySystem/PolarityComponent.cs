using System;
using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    public class PolarityComponent : MonoBehaviour, IPolarizable
    {
        [SerializeField] private Polarity _polarity = Polarity.White;
        public event Action<Polarity> OnPolarityChanged;
        public Polarity CurrentPolarity => _polarity;
        
        public void Toggle() => SetPolarity(_polarity.Opposite());
        public void SetPolarity(Polarity newPolarity)
        {
            if (_polarity == newPolarity) return;
            _polarity = newPolarity;
            OnPolarityChanged?.Invoke(_polarity);
        }
        
       
    }
}