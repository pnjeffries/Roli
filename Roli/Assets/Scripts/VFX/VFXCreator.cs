using Nucleus.Game;
using Nucleus.Rendering;
using Nucleus.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCreator : MonoBehaviour
{
    /// <summary>
    /// The templates used to define special effects
    /// </summary>
    public GameObject[] SFXTemplates;

    /// <summary>
    /// A dictionary mapping keywords to templates
    /// </summary>
    private Dictionary<string, GameObject> _SFXTemplateMap = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var template in SFXTemplates)
        {
            if (template != null)
            {
                _SFXTemplateMap.Add(template.name, template);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SFXSupervisor super = GameEngine.Instance.SFX; //Temp?
        while (super.Triggers.Count > 0)
        {
            // Get trigger
            int iLast = super.Triggers.Count - 1;
            var trigger = super.Triggers[iLast];
            super.Triggers.RemoveAt(iLast);

            // Resolve trigger
            if (_SFXTemplateMap.ContainsKey(trigger.KeyWord))
            {
                var template = _SFXTemplateMap[trigger.KeyWord];
                var representation = Instantiate(template, this.transform);
                if (trigger.Position.IsValid())
                {
                    representation.transform.position += ToUnity.Convert(trigger.Position);
                }
                if (trigger.Direction.IsValid())
                {
                    representation.transform.rotation = 
                        trigger.Direction.Angle.ToUnityQuaternion() * representation.transform.rotation;
                }
                //TODO: Track?
            }
        }
    }
}
