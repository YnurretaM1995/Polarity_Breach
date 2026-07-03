using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentPusher : MonoBehaviour
{
    private NavMeshAgent agent;
    private Coroutine pushRoutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void PushBack(Vector3 direction, float distance, float duration = 0.15f)
    {
        if (pushRoutine != null) StopCoroutine(pushRoutine);
        pushRoutine = StartCoroutine(DoPush(direction.normalized, distance, duration));
    }

    private IEnumerator DoPush(Vector3 direction, float distance, float duration)
    {
        float elapsed = 0f;
        float speed = distance / duration;

        while (elapsed < duration)
        {
            agent.Move(direction * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pushRoutine = null;
    }
}