using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private LayerMask hitLayers = ~0;

    private Rigidbody rb;
    private float speed;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Launch(Vector3 direction, float launchSpeed, int launchDamage)
    {
        speed = launchSpeed;
        damage = launchDamage;
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"Player hit for {damage} damage.");
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}