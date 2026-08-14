using UnityEngine;
using PolarityBreach.Enemy;

namespace PolarityBreach
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyWaveSpawner waveSpawner;
        [SerializeField] private Transform bossRoomSpawnPoint;

        [Header("Colliders")]
        [SerializeField] private Collider blockingCollider; // solid, blocks player when door is closed
        [SerializeField] private Collider triggerCollider;   // Is Trigger = true, detects player passing through
        [SerializeField] private MeshRenderer doorMesh;

        private bool isOpen = false;

        private void OnEnable()
        {
            if (waveSpawner != null)
                waveSpawner.OnRoomCleared += OpenDoor;
        }

        private void OnDisable()
        {
            if (waveSpawner != null)
                waveSpawner.OnRoomCleared -= OpenDoor;
        }

        private void OpenDoor()
        {
            if (isOpen) return; // guard against double-firing

            Debug.Log("Door opened!");
            isOpen = true;

            if (doorMesh != null)
                doorMesh.enabled = false;

            if (blockingCollider != null)
                blockingCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger entered by: {other.name}, tag: {other.tag}, isOpen: {isOpen}");
            
            if (!isOpen)
                return;

            if (other.CompareTag("Player"))
            {
                if (bossRoomSpawnPoint == null)
                {
                    Debug.LogWarning("Door: bossRoomSpawnPoint not assigned.");
                    return;
                }

                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero; 
                    rb.position = bossRoomSpawnPoint.position;
                }
                else
                {
                    other.transform.position = bossRoomSpawnPoint.position;
                }

                Debug.Log("Entering boss room!");
            }
        }
    }
}