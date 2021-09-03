using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    [Tooltip("Each cycle duration")]
    public float CycleDuration = 0.5f;

    [Tooltip("The number of cycles")]
    public int Cycles = 3;

    [Tooltip("The colour to flash to")]
    public Color FlashColour = Color.red;

    [Tooltip("The text component to flash")]
    public TMPro.TMP_Text Text;

    private Color _Original;

    public bool PlayOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        _Original = Text.color;
        if (PlayOnStart) PlayFlash();
    }

    public void PlayFlash()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        int cycle = 0;
        float time = 0;
        while (cycle < Cycles)
        {
            time += Time.deltaTime;
            if (time > CycleDuration)
            {
                time -= CycleDuration;
                cycle++;
            }
            var colour = Color.Lerp(_Original, FlashColour, time / CycleDuration);
            Text.color = colour;
            yield return null;
        }
        Text.color = _Original;
    }
}
