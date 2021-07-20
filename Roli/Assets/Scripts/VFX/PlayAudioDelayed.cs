using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to play an audio source after a short delay
/// </summary>
public class PlayAudioDelayed : MonoBehaviour
{
    [Tooltip("The audio source to be played")]
    public AudioSource Audio = null;

    [Tooltip("The delay in seconds")]
    public float Delay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (Audio != null) Audio.PlayDelayed(Delay);
    }

}
