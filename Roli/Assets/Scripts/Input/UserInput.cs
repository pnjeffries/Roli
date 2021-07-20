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
    /// The mapping of axis names to functions
    /// </summary>
    private Dictionary<string, AxisFunctions> _AxisMapping =
        new Dictionary<string, AxisFunctions>();

    /// <summary>
    /// The amount of time the current set of keys has been held down
    /// </summary>
    private float _HeldTime = 0;

    /// <summary>
    /// The amount of time before the key press will be repeated
    /// </summary>
    public float RepeatTime = 0.3f;

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
        _KeyMapping.Add(KeyCode.Keypad8, InputFunction.Up);
        _KeyMapping.Add(KeyCode.Keypad2, InputFunction.Down);
        _KeyMapping.Add(KeyCode.Keypad4, InputFunction.Left);
        _KeyMapping.Add(KeyCode.Keypad6, InputFunction.Right);
        _KeyMapping.Add(KeyCode.Keypad5, InputFunction.Wait);
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

        // Default axis mapping:
        _AxisMapping.Add("Vertical", new AxisFunctions(InputFunction.Up, InputFunction.Down));
        _AxisMapping.Add("Horizontal", new AxisFunctions(InputFunction.Right, InputFunction.Left));
        _AxisMapping.Add("Jump", new AxisFunctions(InputFunction.Wait));
    }

    // Update is called once per frame
    void Update()
    {
        KeyDown();
        KeyUp();
        KeyHeld();
        AxisCheck();
    }

    private void AxisCheck()
    {
        float threshold = 0.5f;
        foreach (var kvp in _AxisMapping)
        {
            float value = Input.GetAxis(kvp.Key);
            if (Mathf.Abs(value) >= threshold)
            {
                if (Mathf.Abs(kvp.Value.LastValue) < threshold)
                {
                    // Press
                    if (value > 0) GameEngine.Instance.Input.InputPress(kvp.Value.Positive);
                    else GameEngine.Instance.Input.InputPress(kvp.Value.Negative);
                    _HeldTime = -RepeatTime;
                }
                else
                {
                    // Hold
                    _HeldTime += Time.deltaTime;
                    if (_HeldTime > RepeatTime)
                    {
                        if (kvp.Value.LastValue > 0) GameEngine.Instance.Input.InputRelease(kvp.Value.Positive);
                        else GameEngine.Instance.Input.InputRelease(kvp.Value.Negative);
                        _HeldTime -= RepeatTime;
                    }
                }
            }
            else if (Mathf.Abs(kvp.Value.LastValue) >= threshold)
            {
                // Release
                if (kvp.Value.LastValue > 0) GameEngine.Instance.Input.InputRelease(kvp.Value.Positive);
                else GameEngine.Instance.Input.InputRelease(kvp.Value.Negative);
            }
            kvp.Value.LastValue = value;
        }
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
                        _HeldTime -= RepeatTime;
                        return;
                    }
                }
            }
        }
    }

    public class AxisFunctions
    {
        public InputFunction Negative;
        public InputFunction Positive;
        public float LastValue = 0;

        public AxisFunctions(InputFunction positive, InputFunction negative)
        {
            Negative = negative;
            Positive = positive;
        }

        public AxisFunctions(InputFunction function)
        {
            Negative = function;
            Positive = function;
        }
    }
}
