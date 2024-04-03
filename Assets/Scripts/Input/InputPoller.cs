using Hiyazcool.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;

// Player Number
// #0 : Gamepad Input
// #1 : Keyboard input :: WSAD + QEZC
// #2 : Keyboard input :: IJKL + UON>


public class InputPoller : MonoBehaviourSingleton<InputPoller> 
{
    [SerializeField]
    public Action<Vector2> InputMovement;
    public InGameInput Input;
    protected override void Awake() {
        base.Awake();
        Input = new InGameInput();
        Input.Enable();
        Input.PlayerCharacacter.Enable();
        Input.PlayerCharacacter.Movement.Enable();
        Input.PlayerCharacacter.WeaponNext.Enable();
        Input.PlayerCharacacter.WeaponPrevious.Enable();
        Input.PlayerCharacacter.Attack.Enable();
        
    }
    public void Update() {
        //if (Input.PlayerCharacacter.Movement.triggered)
        //InputMovement?.Invoke(Input.PlayerCharacacter.Movement.)
        GetInputP1();
    }



    



    public InputData GetInput(int PlayerNumber)
    {
        if (PlayerNumber == 0) { return GetInputP0(); }
        if (PlayerNumber == 1) { return GetInputP1(); }
        if (PlayerNumber == 2) { return GetInputP2(); }

        // Defaut return 
        return InputData.CleanDataStruct();

    }

   

    // This is the First GamePad Connected Info
    public InputData GetInputP0()
    {
        InputData input = InputData.CleanDataStruct();

        Gamepad gpad = Gamepad.current;
        // verify we have gamepad data
        if (gpad != null)
        {
            input.leftStick = gpad.leftStick.ReadValue();
            input.rightStick = gpad.rightStick.ReadValue();
            input.dpad = gpad.dpad.ReadValue();
            input.leftStickButton = gpad.leftStickButton.wasPressedThisFrame;
            input.rightStickButton = gpad.rightStickButton.wasPressedThisFrame;
            input.leftTrigger = gpad.leftTrigger.ReadValue();
            input.rightTrigger = gpad.rightTrigger.ReadValue();
            input.leftShoulder = gpad.leftShoulder.wasPressedThisFrame;
            input.rightShoulder = gpad.rightShoulder.wasPressedThisFrame;
            input.buttonNorth = gpad.buttonNorth.wasPressedThisFrame;
            input.buttonSouth = gpad.buttonSouth.wasPressedThisFrame;
            input.buttonEast = gpad.buttonEast.wasPressedThisFrame;
            input.buttonWest = gpad.buttonWest.wasPressedThisFrame;
            input.startButton = gpad.startButton.wasPressedThisFrame;
            input.selectButton = gpad.selectButton.wasPressedThisFrame;
        }

        return input;
    }

    // this is the Keyboard and Mouse Info
    public InputData GetInputP1()
    {
        InputData input = InputData.CleanDataStruct();

        Keyboard kb = Keyboard.current;
        // Verifiy we have Keyboard data 
        if (kb != null)
        {
            if (kb.wKey.isPressed) { input.leftStick.y = 1; }
            if (kb.sKey.isPressed) { input.leftStick.y = -1; }
            if (kb.aKey.isPressed) { input.leftStick.x = -1; }
            if (kb.dKey.isPressed) { input.leftStick.x = 1; }

            input.buttonNorth = kb.qKey.wasPressedThisFrame;
            input.buttonSouth = kb.eKey.wasPressedThisFrame;
            input.buttonEast = kb.zKey.wasPressedThisFrame;
            input.buttonEast = kb.cKey.wasPressedThisFrame;

        }

        InputMovement?.Invoke(new Vector2 {
            x = input.leftStick.x,
            y = input.leftStick.y
        });
        //Debug.Log("Input Poller" + new Vector2 {
        //    x = input.leftStick.x,
        //    y = input.leftStick.y
        //});
        return input;
    }

    // this is the Keyboard and Mouse Info
    public InputData GetInputP2()
    {
        InputData input = InputData.CleanDataStruct();

        Keyboard kb = Keyboard.current;
        // Verifiy we have Keyboard data 
        if (kb != null)
        {
            if (kb.iKey.isPressed) { input.leftStick.y = 1; }
            if (kb.kKey.isPressed) { input.leftStick.y = -1; }
            if (kb.jKey.isPressed) { input.leftStick.x = -1; }
            if (kb.lKey.isPressed) { input.leftStick.x = 1; }

            input.buttonNorth = kb.uKey.wasPressedThisFrame;
            input.buttonSouth = kb.oKey.wasPressedThisFrame;
            input.buttonEast = kb.nKey.wasPressedThisFrame;
            input.buttonEast = kb.periodKey.wasPressedThisFrame;

     
        }


        return input;
    }

}
