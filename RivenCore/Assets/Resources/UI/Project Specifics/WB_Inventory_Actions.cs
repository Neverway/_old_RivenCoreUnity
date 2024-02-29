//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WB_Inventory_Actions : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private readonly Dictionary<int, Vector2> inventoryPositions = new Dictionary<int, Vector2>{
        { 0, new Vector2(242, 38) },
        { 1, new Vector2(242, -38) },
        { 2, new Vector2(242, -100.8f) },
        { 3, new Vector2(242, -163.6f) },
        { 4, new Vector2(242, -226.4f) },
        { 5, new Vector2(521, 38) },
        { 6, new Vector2(521, -38) },
        { 7, new Vector2(521, -100.8f) },
        { 8, new Vector2(521, -163.6f) },
        { 9, new Vector2(521, -226.4f) },
    };


    //=-----------------=
    // Private Variables
    //=-----------------=
    [Range(0,4)] public int currentActionIndex;
    public bool isAllowingNavigation;
    public TMP_Text[] textTargets;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private InputActions.TopDown2DActions topdown2DActions;
    public GameObject actionMenu;


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
        SetTextHighlighting(currentActionIndex);
    }

    private IEnumerator WaitForKeypressDelay()
    {
        yield return new WaitForEndOfFrame();
        isAllowingNavigation = true;
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
        if (!isAllowingNavigation) return;
        if (topdown2DActions.Move.WasPressedThisFrame())
        {
            var moveDirection = topdown2DActions.Move.ReadValue<Vector2>();
            if (moveDirection.y == 1) NavigateActions("Up");
            else if (moveDirection.y == -1) NavigateActions("Down");
        }
        if (topdown2DActions.Interact.WasPressedThisFrame())
        {
            switch (currentActionIndex)
            {
                case (0):
                    FindObjectOfType<WB_Inventory>().EquipItem();
                    break;
                case (3):
                    FindObjectOfType<WB_Inventory>().DiscardItem();
                    break;
            }
            CloseActionMenu();
            FindObjectOfType<WB_Inventory>().EnableNavigation();
        }
    }

    private void NavigateActions(string _direction)
    {
        switch (_direction)
        {
            case ("Up"):
                // Check range boundaries (Don't go up if there is nothing above)
                if (currentActionIndex != 0) currentActionIndex--;
                break;
            case ("Down"):
                // Check range boundaries  (Don't go down if there is nothing below)
                if (currentActionIndex != 4) currentActionIndex++;
                break;
        }
    }
    public void SetTextHighlighting(int _highlightedIndex)
    {
        foreach (var text in textTargets)
        {
            text.color = new Color(1, 1, 1, 0.5f);
            text.fontStyle &= ~FontStyles.Underline; // Disable text underline
        }

        textTargets[_highlightedIndex].color = new Color(1, 1, 1, 1);
        textTargets[_highlightedIndex].fontStyle |= FontStyles.Underline; // Enable text underline
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ShowActionMenu(int _itemSlotIndex)
    {
        if (inventoryPositions.TryGetValue(_itemSlotIndex, out var position))
        {
            StartCoroutine(WaitForKeypressDelay());
            actionMenu.SetActive(true);
            actionMenu.transform.localPosition = new Vector3(position.x, position.y, 0);
        }
        else
        {
            Debug.LogWarning($"Position for itemSlotIndex {_itemSlotIndex} not found.");
        }
    }
    public void CloseActionMenu()
    {
        currentActionIndex = 0;
        isAllowingNavigation = false;
        actionMenu.SetActive(false);
    }
}
