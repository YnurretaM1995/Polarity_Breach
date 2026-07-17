using UnityEngine;
using PolarityBreach.Enemy;

namespace PolarityBreach
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyWaveSpawner waveSpawner;
        [SerializeField] private Transform bossRoomSpawnPoint;

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
            Debug.Log("Door opened!");

            isOpen = true;

            // Hide the door
            GetComponent<MeshRenderer>().enabled = false;

            // Allow player to walk through
            GetComponent<Collider>().enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isOpen)
                return;

            if (other.CompareTag("Player"))
            {
                Debug.Log("Entering boss room!");
                other.transform.position = bossRoomSpawnPoint.position;
            }
        }
    }
}