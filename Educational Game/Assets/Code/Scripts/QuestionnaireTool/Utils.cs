using UnityEngine;
using System.Collections;

namespace Utilities
{
    public static class Utils
    {
        public static IEnumerator DelayedPlayNarratorClip(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            NarratorAudioManager.instance.fishNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.fishNetClip, netRelated: true);
        }

        public static void PositionRelativeToPlayer(Transform target, Vector3 offset)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target.position = player.transform.position + offset;
            }
        }
    }
}
