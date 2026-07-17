using UnityEngine;

namespace PolarityBreach.Feedback
{
    public static class FeedbackHandler
    {
        public static void SpawnParticles(GameObject prefab, Vector3 position)
        {
            GameObject effect = Object.Instantiate(prefab, position, Quaternion.identity);
            Object.Destroy(effect, 2f);
        }
        
    }
}
