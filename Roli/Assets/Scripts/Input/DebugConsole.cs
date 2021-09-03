using Nucleus.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    private TMPro.TMP_InputField _InputField;

    // Start is called before the first frame update
    void Start()
    {
        _InputField = GetComponent<TMPro.TMP_InputField>();
        _InputField.ActivateInputField();
    }

 
    /// <summary>
    /// Attempt to execute the currently entered text
    /// </summary>
    public void OnEnterCommand()
    {
        string command = _InputField.text;
        GameEngine.Instance.State.RunDebugCommand(command);
        _InputField.text = "";
        _InputField.ActivateInputField();
    }
}
