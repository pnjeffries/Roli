using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaPulse : MonoBehaviour
{
    [Tooltip("Each cycle duration")]
    public float CycleDuration = 0.5f;

    [Tooltip("The changed alpha during the cycle cycle")]
    public float AlphaChanged = 0.5f;

    [Tooltip("The normal alpha")]
    public float AlphaBase = 1.0f;

    [Tooltip("The target Text component")]
    public TMPro.TextMeshPro Target = null;

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
            float alpha = AlphaBase + Mathf.Sin(t * Mathf.PI) * (AlphaChanged - AlphaBase);
            Target.alpha = alpha;
        }
    }
}
