using UnityEngine;
using PolarityBreach.Enemy;

namespace PolarityBreach
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private EnemyWaveSpawner waveSpawner;

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

            GetComponent<MeshRenderer>().enabled = false;

            // Disable blocking collision
            GetComponent<Collider>().enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isOpen)
                return;

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player passed through the door!");
                // load next room / level
            }
        }
    }
}
