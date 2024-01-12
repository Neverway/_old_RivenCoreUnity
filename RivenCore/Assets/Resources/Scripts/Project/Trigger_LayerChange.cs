//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Layer mask ids: 1-6, 2-7, 3-8
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger_LayerChange : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Which layer should the entity switch to when entering the trigger. P.S DON'T FORGET TO SWITCH THE TRIGGERS LAYER!!")]
    [Range(0, 2)] public int targetLayer;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.GetComponent<Entity>())
        {
            var entity = other.transform.parent.GetComponent<Entity>();
            var entityPos = entity.transform.position;
            switch (targetLayer)
            {
                case 0:
                    other.gameObject.layer = 6;
                    entity.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 1";
                    entity.transform.position = new Vector3(entityPos.x, entityPos.y, 0);
                    break;
                case 1:
                    other.gameObject.layer = 7;
                    entity.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 2";
                    entity.transform.position = new Vector3(entityPos.x, entityPos.y, -1);
                    break;
                case 2:
                    other.gameObject.layer = 8;
                    entity.GetComponent<SpriteRenderer>().sortingLayerName = "Depth Layer 3";
                    entity.transform.position = new Vector3(entityPos.x, entityPos.y, -2);
                    break;
            }
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
