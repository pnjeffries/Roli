using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    [Tooltip("Each cycle duration")]
    public float CycleDuration = 0.5f;

    [Tooltip("The maximum scale during the cycle")]
    public float ScaleMax = 1.2f;

    [Tooltip("The minimum scale during the cycle")]
    public float ScaleMin = 1.0f;

    public bool Play = false;

    private float _CycleTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Play || _CycleTime > 0)
        {
            _CycleTime += Time.deltaTime;
            if (_CycleTime > CycleDuration)
            {
                if (Play) _CycleTime -= CycleDuration;
                else _CycleTime = 0;
            }
            float t = _CycleTime / CycleDuration;
            float scale = ScaleMin + Mathf.Sin(t * Mathf.PI) * (ScaleMax - ScaleMin);
            var transform = this.transform;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
