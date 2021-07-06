using Binding;
using Nucleus.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDataContext : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataContext.SetDataContextOn(this.gameObject, GameEngine.Instance);
    }
}
