using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nucleus.Game;
using System;

public class StartUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        GameEngine.Instance.Resources = new ResourceLoader();
        Debug.Log("Loading module...");
        var module = new Roli.RoliModule();
        GameEngine.Instance.LoadModule(module);
        Debug.Log("Initialising Engine...");
        GameEngine.Instance.StartUp();
        Debug.Log("Initialised.");
    }

    private void Update()
    {
        GameEngine.Instance.Update();
    }
}
