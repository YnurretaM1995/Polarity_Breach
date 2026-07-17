using PolarityBreach.Audio;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitCooldown = 1f;
    [SerializeField] private string playerTag = "Player";

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float enemyPushback = 1.5f;
    
    [SerializeField] private AudioClip damageSound;

    private float cooldownTimer;
    private NavMeshAgentPusher pusher;

    private void Awake()
    {
        pusher = GetComponent<NavMeshAgentPusher>();
        if (pusher == null) pusher = gameObject.AddComponent<NavMeshAgentPusher>();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        TryHit(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHit(other);
    }

    private void TryHit(Collider other)
    {
        if (cooldownTimer > 0f) return;
        if (!other.CompareTag(playerTag)) return;

        Debug.Log($"Player hit for {damage} contact damage.");

        cooldownTimer = hitCooldown;

        Vector3 awayFromEnemy = (other.transform.position - transform.position);
        awayFromEnemy.y = 0f;
        awayFromEnemy.Normalize();

        Rigidbody playerRb = other.attachedRigidbody;
        if (playerRb != null)
        {
            playerRb.AddForce(awayFromEnemy * knockbackForce, ForceMode.Impulse);
        }
        AudioHandler.Play3DSound(damageSound, transform.position);
        pusher.PushBack(-awayFromEnemy, enemyPushback);
    }
}