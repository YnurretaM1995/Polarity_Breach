using UnityEngine;
using UnityEngine.AI;

namespace PolarityBreach.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private NavMeshAgent agent;

        private void Awake()
        {
            GetComponents();
        }

        private void Update()
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        public void PlayAttack()
        {
            animator.SetTrigger("Attack");
        }
        
        private void GetComponents()
        {
            agent = GetComponent<NavMeshAgent>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }
    }
}
