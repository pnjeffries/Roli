using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to limit the maximum number of children of the object to which it is attached
/// by automatically culling the oldest
/// </summary>
public class LimitChildCount : MonoBehaviour
{
    [Tooltip("The maximum allowable number of children before culling begins")]
    public int MaxCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > MaxCount)
        {
            var child = transform.GetChild(0).gameObject;
            Destroy(child);
        }
    }
}
