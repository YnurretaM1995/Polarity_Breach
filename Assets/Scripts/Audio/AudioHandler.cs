using UnityEngine;

namespace PolarityBreach.Audio
{
    public static class AudioHandler
    {
        public static void Play3DSound(AudioClip clip, Vector3 position)
        {
            if (clip == null) return;

            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
