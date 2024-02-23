//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Grabbable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool isHeld;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Entity targetEntity;
    public Vector3 lastFaceDirection;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (!isHeld || !targetEntity)
        {
            GetComponent<Trigger_Interactable>().hideIndicator=false;
            return;
        }
        GetComponent<Trigger_Interactable>().hideIndicator=true;
        transform.parent.position = targetEntity.transform.position + GetTargetEntityOffset();
        transform.parent.GetComponent<Object_DepthAssigner>().depthLayer = targetEntity.GetComponent<Object_DepthAssigner>().depthLayer;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Vector3 GetTargetEntityOffset()
    {
        var facingDirection = GetFaceDirectionFromQuaternion(targetEntity.faceDirection, -90);
        print(facingDirection);
        switch (facingDirection.y)
        {
            case (1):
                lastFaceDirection = new Vector3(0, 0.3f, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(0, -0.3f, 0);
                return lastFaceDirection;
        }
        
        switch (facingDirection.x)
        {
            case (1):
                lastFaceDirection = new Vector3(0.5f, 0, 0);
                return lastFaceDirection;
            case (-1):
                lastFaceDirection = new Vector3(-0.5f, 0, 0);
                return lastFaceDirection;
        }
        return lastFaceDirection;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        var interaction = _other.GetComponent<Trigger_Interaction>();
        if (interaction)
        {
            targetEntity = interaction.targetEntity;
            ToggleHeld();
        }
    }
    
    public Vector2 GetFaceDirectionFromQuaternion(Quaternion rotation, int zRotationOffset)
    {
        // Extract the z-axis rotation from the Quaternion
        float angle = rotation.eulerAngles.z - zRotationOffset;

        // Convert the angle back to radians since Unity's trig functions expect radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the x and y components of the facing direction
        float x = Mathf.Cos(angleInRadians);
        float y = Mathf.Sin(angleInRadians);

        // Create and return the facing direction vector
        Vector2 facingDirection = new Vector2(x, y);

        return facingDirection;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    private void ToggleHeld()
    {
        isHeld = !isHeld;
    }
}
