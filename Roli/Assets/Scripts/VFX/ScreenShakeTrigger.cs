using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to trigger a shake effect on the main camera
/// </summary>
public class ScreenShakeTrigger : MonoBehaviour
{
    [Tooltip("The default duration of the shake effect")]
    public float Duration = 1f;

    [Tooltip("The default magnitude of the shake effect")]
    public float Magnitude = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        var camera = Camera.main;
        var shaker = camera.GetComponent<Shaker>();
        if (shaker != null) shaker.StartShake(Duration, Magnitude);
    }

}
