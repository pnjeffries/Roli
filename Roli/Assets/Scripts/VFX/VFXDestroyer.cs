using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDestroyer : MonoBehaviour
{
    public float Delay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, Delay);
    }
}
