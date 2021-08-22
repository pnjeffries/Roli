using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePosition : MonoBehaviour
{
    public Vector2 Offset = new Vector2(0, 0);

    private Canvas _Canvas;

    // Start is called before the first frame update
    void Start()
    {
        _Canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3[] corners = new Vector3[4];
        _Canvas.GetComponent<RectTransform>().GetWorldCorners(corners);
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 localImagePos = (mousePos - (Vector2)corners[0]) / _Canvas.scaleFactor; //.localScale.x;
        transform.position = localImagePos;

        var screenPoint = Input.mousePosition;
        screenPoint.z = 0;
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), screenPoint, Camera.main, out result);
        transform.position = result;*/

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_Canvas.transform as RectTransform, Input.mousePosition, _Canvas.worldCamera, out pos);
        transform.position = _Canvas.transform.TransformPoint(pos + Offset);
    }
}
