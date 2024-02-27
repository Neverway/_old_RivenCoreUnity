//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WB_Inventory : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Range(0,9)] public int currentInventoryIndex;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private InputActions.TopDown2DActions topdown2DActions;
    [SerializeField] private UI_Text_EntityItems entityItemsText;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        topdown2DActions = new InputActions().TopDown2D;
        topdown2DActions.Enable();
    }

    private void Update()
    {
        ProcessUserInput();
        
        // Update highlighting
        entityItemsText.SetTextHighlighting(currentInventoryIndex);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// <summary>
    /// Processes user input for various actions in the inventory.
    /// </summary>
    private void ProcessUserInput()
    {
        // Navigate inventory
        if (topdown2DActions.Move.WasPressedThisFrame())
        {
            var moveDirection = topdown2DActions.Move.ReadValue<Vector2>();
            if (moveDirection.y == 1) NavigateInventory("Up");
            else if (moveDirection.y == -1) NavigateInventory("Down");
            if (moveDirection.x == -1) NavigateInventory("Left");
            else if (moveDirection.x == 1) NavigateInventory("Right");
        }
    }

    private void NavigateInventory(string _direction)
    {
        switch (_direction)
        {
            case ("Up"):
                // Check range boundaries (Don't go up if there is nothing above)
                if (currentInventoryIndex != 0 && currentInventoryIndex != 5) currentInventoryIndex--;
                break;
            case ("Down"):
                // Check range boundaries  (Don't go down if there is nothing below)
                if (currentInventoryIndex != 4 && currentInventoryIndex != 9) currentInventoryIndex++;
                break;
            case ("Left"):
                // Check range boundaries (Don't shift left if we are already in the left column)
                if (currentInventoryIndex > 4) currentInventoryIndex -= 5;
                break;
            case ("Right"):
                // Check range boundaries (Don't shift right if we are already in the right column)
                if (currentInventoryIndex < 5) currentInventoryIndex += 5;
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}