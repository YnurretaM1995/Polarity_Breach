using UnityEngine;

namespace PolarityBreach.Player

{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float speed = horizontalVel.magnitude;

            animator.SetFloat("Speed", speed);
        }
    }
}
    