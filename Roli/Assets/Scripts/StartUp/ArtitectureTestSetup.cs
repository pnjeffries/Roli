using Nucleus.Game;
using Roli;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtitectureTestSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEngine.Instance.State = new GeneratorTestState();
        GameEngine.Instance.State.StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
