using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    private TextMeshPro _Text;
    private Image _Panel;

    // Start is called before the first frame update
    void Start()
    {
        _Text = GetComponentInChildren<TextMeshPro>();
        _Panel = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null && hit.transform.GetComponent<Tooltip>() != null)
        {
            _Panel.gameObject.SetActive(true);
        }
        else
        {
            _Panel.gameObject.SetActive(false);
        }
    }
}
