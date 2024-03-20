using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWPlayerController : PlayerController
{
    public GameObject MyPawn; 

    protected override void Start()
    {
        base.Start();
        if (MyPawn)
        {
            ControlPawn(MyPawn); 
        }
        
    }


    protected override void ProcessInput()
    {
        if(!ControlledPawn)
        {
            // If we don't have a pawn, don't bother processing input. 
            return; 
        }

        if (InputCurrent.buttonNorth)
        {
            Fire1(InputCurrent.buttonNorth); 
        }

        Vertical(InputCurrent.leftStick.y);
        Horizontal(InputCurrent.leftStick.x); 
    }

    public override void Vertical(float value)
    {
        // Vroom Vroom, Go Fast. 
        //LOG("PWPC: Vertical");
        PWPawn PWP = ((PWPawn)ControlledPawn);
        if (PWP)
        {
            PWP.Vertical(value);
        }
    }
    public override void Horizontal(float value)
    {
        // Your spin me right around.... 
        //LOG("PC: Vertical");
        PWPawn PWP = ((PWPawn)ControlledPawn);
        if (PWP)
        {
            PWP.Horizontal(value);
        }

    }

    

    public override void Fire1(bool value)
    {
        //((PWPawn)PossesedPawn).Fire1(value);

        if (value)
        {
            LOG("PC: Fire1");
            PWPawn PWP = ((PWPawn)ControlledPawn); 
            if (PWP)
            {
                PWP.Fire1(value);
            }


        }
    }
}
