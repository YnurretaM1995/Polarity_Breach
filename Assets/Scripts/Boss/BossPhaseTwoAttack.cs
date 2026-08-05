using System.Collections;
using UnityEngine;

namespace PolarityBreach.Boss
{
    public class BossPhaseTwoAttack : MonoBehaviour
    {
        [SerializeField] private Transform beamPivot;
        [SerializeField] private GameObject[] beams;
        [SerializeField] private GameObject[] warningBeams;
        [SerializeField] private float warningDuration = 1f;
        [SerializeField] private float beamDuration = 4f;
        [SerializeField] private float timeBetweenBeamAttacks = 2f;
        [SerializeField] private float rotationSpeed = 20f;
        
        private bool isActive;
        private Coroutine phaseRoutine;
        private float rotationDirection = 1f;

        private void Awake()
        {
            SetWarningBeamsActive(false);
            SetBeamsActive(false);
        }

        public void StartPhase()
        {
            if (isActive || phaseRoutine != null) return;

            rotationDirection = 1f;
            phaseRoutine = StartCoroutine(PhaseTwoLoop());
        }

        public void StopPhase()
        {
            if (phaseRoutine != null)
            {
                StopCoroutine(phaseRoutine);
                phaseRoutine = null;
            }

            isActive = false;
            SetWarningBeamsActive(false);
            SetBeamsActive(false);
        }

        private IEnumerator PhaseTwoLoop()
        {
            while (true)
            {
                isActive = false;
                SetBeamsActive(false);
                SetWarningBeamsActive(true);

                yield return new WaitForSeconds(warningDuration);

                SetWarningBeamsActive(false);
                SetBeamsActive(true);
                isActive = true;

                yield return new WaitForSeconds(beamDuration);

                isActive = false;
                SetBeamsActive(false);
                rotationDirection *= -1f;

                yield return new WaitForSeconds(timeBetweenBeamAttacks);
            }
        }

        private void Update()
        {
            if (!isActive || beamPivot == null) return;
            
            beamPivot.Rotate(Vector3.up, rotationSpeed * rotationDirection * Time.deltaTime);
        }
        
        private void SetBeamsActive(bool active)
        {
            for (int i = 0; i < beams.Length; i++)
            {
                if (beams[i] != null)
                {
                    beams[i].SetActive(active);
                }
            }
        }

        private void SetWarningBeamsActive(bool active)
        {
            for (int i = 0; i < warningBeams.Length; i++)
            {
                if (warningBeams[i] != null)
                {
                    warningBeams[i].SetActive(active);
                }
            }
        }

    }
}
