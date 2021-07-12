using Nucleus.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager class for user input operations
/// </summary>
public class UserInput : MonoBehaviour
{
    /// <summary>
    /// The mapping of key codes to functions
    /// </summary>
    private Dictionary<KeyCode, InputFunction> _KeyMapping =
            new Dictionary<KeyCode, InputFunction>();

    /// <summary>
    /// The amount of time the current set of keys has been held down
    /// </summary>
    private float _HeldTime = 0;

    /// <summary>
    /// The amount of time before the key press will be repeated
    /// </summary>
    public float RepeatTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Make static?
        // Default keymapping:
        _KeyMapping.Add(KeyCode.UpArrow, InputFunction.Up);
        _KeyMapping.Add(KeyCode.DownArrow, InputFunction.Down);
        _KeyMapping.Add(KeyCode.LeftArrow, InputFunction.Left);
        _KeyMapping.Add(KeyCode.RightArrow, InputFunction.Right);
        _KeyMapping.Add(KeyCode.Space, InputFunction.Wait);
        _KeyMapping.Add(KeyCode.Alpha1, InputFunction.Ability_1);
        _KeyMapping.Add(KeyCode.Alpha2, InputFunction.Ability_2);
        _KeyMapping.Add(KeyCode.Alpha3, InputFunction.Ability_3);
        _KeyMapping.Add(KeyCode.Alpha4, InputFunction.Ability_4);
        _KeyMapping.Add(KeyCode.Alpha5, InputFunction.Ability_5);
        _KeyMapping.Add(KeyCode.Alpha6, InputFunction.Ability_6);
        _KeyMapping.Add(KeyCode.Alpha7, InputFunction.Ability_7);
        _KeyMapping.Add(KeyCode.Alpha8, InputFunction.Ability_8);
        _KeyMapping.Add(KeyCode.Alpha9, InputFunction.Ability_9);
        _KeyMapping.Add(KeyCode.Z, InputFunction.Ability_1);
        _KeyMapping.Add(KeyCode.X, InputFunction.Ability_2);
        _KeyMapping.Add(KeyCode.C, InputFunction.Ability_3);
        _KeyMapping.Add(KeyCode.V, InputFunction.Ability_4);
        _KeyMapping.Add(KeyCode.B, InputFunction.Ability_5);
        _KeyMapping.Add(KeyCode.N, InputFunction.Ability_6);
        _KeyMapping.Add(KeyCode.G, InputFunction.PickUp);
    }

    // Update is called once per frame
    void Update()
    {
        KeyDown();
        KeyUp();
        KeyHeld();
    }

    /// <summary>
    /// Deal with input keys being pressed
    /// </summary>
    private void KeyDown()
    {
        if (Input.anyKeyDown)
        {
            foreach (var kvp in _KeyMapping)
            {

                if (Input.GetKeyDown(kvp.Key))
                {
                    GameEngine.Instance.Input.InputPress(kvp.Value);
                }
            }
            _HeldTime = -RepeatTime;
        }
    }

    /// <summary>
    /// Deal with input keys being released
    /// </summary>
    private void KeyUp()
    {
        foreach (var kvp in _KeyMapping)
        {
            if (Input.GetKeyUp(kvp.Key))
                GameEngine.Instance.Input.InputRelease(kvp.Value);
        }
    }

    /// <summary>
    /// Deals with auto-repeating when a key is held down
    /// </summary>
    private void KeyHeld()
    {
        if (Input.anyKey)
        {
            _HeldTime += Time.deltaTime;
            if (_HeldTime > RepeatTime)
            {
                foreach (var kvp in _KeyMapping)
                {
                    if (Input.GetKey(kvp.Key))
                    {
                        GameEngine.Instance.Input.InputRelease(kvp.Value);
                        _HeldTime = 0;
                        return;
                    }
                }
            }
        }
        else _HeldTime = -RepeatTime;
    }
}
