using UnityEngine;

namespace PolarityBreach.Audio
{
    public static class AudioHandler
    {
        public static void Play3DSound(AudioClip clip, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
