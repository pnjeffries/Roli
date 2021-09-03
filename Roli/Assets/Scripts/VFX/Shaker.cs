using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Tooltip("The default duration of the shake effect")]
    public float Duration = 1f;

    [Tooltip("The default magnitude of the shake effect")]
    public float Magnitude = 1f;

    private Vector3 _OriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        _OriginalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShake()
    {
        StartCoroutine(Shake(Duration, Magnitude));
    }

    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = _OriginalPosition;
        float elapsed = 0f;
        float random = Random.Range(0, 1000f);


        while (elapsed < duration)
        {
            float x = (Mathf.PerlinNoise(elapsed * 20, random) * 2 - 1) * magnitude * (1 - elapsed/duration);
            float y = (Mathf.PerlinNoise(random, elapsed * 20) * 2 - 1) * magnitude * (1 - elapsed/duration);
            transform.localPosition = originalPosition + new Vector3(x, y);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Reset to original
        transform.localPosition = originalPosition;
    }
}
