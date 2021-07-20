using Binding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public DataBinding Binding;

    // Start is called before the first frame update
    void Start()
    {
        Binding.UpdateTargetValue();
    }

}
