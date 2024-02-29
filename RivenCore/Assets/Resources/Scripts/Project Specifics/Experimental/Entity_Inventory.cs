//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Purpose: 
// Notes: THIS WAS DIRECTLY PORTED FROM THE OLD VERSION. PLEASE REFACTOR THIS!
// THANKS ~ Liz M.
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Entity_Inventory : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Header("0-Util, 1-Weapon, 2-Magic, 3-Disabled")]
    [Range(0,2)] public int currentAction = 1;
    public List<Item> items = new List<Item>(10);
    public List<Item> equippedItems = new List<Item>(4);
    public List<GameObject> drops;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool currentlyUsingItem;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public GameObject bladedWeaponPrefab;
    public GameObject throwableUtilityPrefab;


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator UseDelay(float time)
    {
        currentlyUsingItem = true;
        yield return new WaitForSeconds(time);
        currentlyUsingItem = false;
    }
    
    private void OnEnable()    { GetComponent<Entity>().OnEntityDeath += OnEntityDeath; }
    private void OnDisable() { GetComponent<Entity>().OnEntityDeath -= OnEntityDeath; }
    
    private void OnEntityDeath()
    {
        if (drops == null) return;
        foreach (var _drop in drops)
        {
            Instantiate(_drop, gameObject.transform.position, gameObject.transform.rotation, null);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public bool AddItem(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = _item;
                return true;
            }
        }

        return false;
    }

    public void CycleAction(int direction)
    {
        if ((currentAction + direction) > 2)
        {
            currentAction = 0;
        }
        else if ((currentAction + direction) < 0)
        {
            currentAction = 2;
        }
        else
        {
            currentAction += direction;
        }
    }

    public void EquipItem(int _inventorySlot)
    {
        Item previouslyEquippedItem = null;
        switch (items[_inventorySlot].category)
        {
            case (0):
                previouslyEquippedItem = equippedItems[0];
                equippedItems[0] = items[_inventorySlot];
                items[_inventorySlot] = previouslyEquippedItem;
                break;
            case (1):
                previouslyEquippedItem = equippedItems[1];
                equippedItems[1] = items[_inventorySlot];
                items[_inventorySlot] = previouslyEquippedItem;
                break;
            case (2):
                previouslyEquippedItem = equippedItems[2];
                equippedItems[2] = items[_inventorySlot];
                items[_inventorySlot] = previouslyEquippedItem;
                break;
            case (3):
                previouslyEquippedItem = equippedItems[3];
                equippedItems[3] = items[_inventorySlot];
                items[_inventorySlot] = previouslyEquippedItem;
                break;
        }
    }

    public void DiscardItem(int _inventorySlot)
    {
        if (items[_inventorySlot].isDiscardable)
        {
            items[_inventorySlot] = null;
        }
    }

    public void UseItem()
    {
        if (equippedItems[currentAction] == null || currentlyUsingItem) return;

        var entity = gameObject.GetComponent<Entity>();
        if (entity.isNearInteractable) return;

        // For a bladed weapon,
        if (equippedItems[currentAction] is Item_Weapon itemWeapon)
        {
            // spawn a generic bladed weapon,
            var liveItem = Instantiate(bladedWeaponPrefab, this.transform.position, entity.faceDirection, this.transform);
            // set the depth layer
            liveItem.GetComponent<Object_DepthAssigner>().depthLayer = GetComponent<Object_DepthAssigner>().depthLayer;
            // set the graphic,
            liveItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = equippedItems[currentAction].inventoryIcon;
            // set the damage team,
            liveItem.transform.GetChild(1).GetComponent<Trigger_Damage>().owningTeam = entity.currentStats.team;
            // set the base damage,
            liveItem.transform.GetChild(1).GetComponent<Trigger_Damage>().damageAmount = itemWeapon.baseDamage;
            // set the damage type,
            // Damage trigger does not accept damage type yet
            // set the knock back force,
            liveItem.transform.GetChild(2).GetComponent<Trigger_Force>().forceStrength = itemWeapon.forceStrength;
            // set the knock back delay,
            liveItem.transform.GetChild(2).GetComponent<Trigger_Force>().removeEffectDelay = itemWeapon.removeForceDelay;
            // set the object's life time
            Destroy(liveItem, 0.5f);
            StartCoroutine(UseDelay(0.75f));
        }
        
        // For a consumable utility,
        else if (equippedItems[currentAction] is Item_Utility_Consumable itemUtilityConsumable)
        {
            // Check the status effect
            switch (itemUtilityConsumable.effect)
            {
                case "health":
                    entity.ModifyHealth(itemUtilityConsumable.amount);
                    equippedItems[currentAction] = null;
                    break;
                default:
                    break;
            }
            StartCoroutine(UseDelay(0.75f));
        }
        
        // For a throwable utility,
        else if (equippedItems[currentAction] is Item_Utility_Throwable itemUtilityThrowable)
        {
            // spawn a generic bladed weapon,
            var liveItem = Instantiate(throwableUtilityPrefab, this.transform.position, entity.faceDirection);
            // set the depth layer
            liveItem.GetComponent<Object_DepthAssigner>().depthLayer = GetComponent<Object_DepthAssigner>().depthLayer;
            // set the graphic,
            liveItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = equippedItems[currentAction].inventoryIcon;
            // set the damage team,
            liveItem.transform.GetChild(1).GetComponent<Trigger_Damage>().owningTeam = entity.currentStats.team;
            // set the base damage,
            liveItem.transform.GetChild(1).GetComponent<Trigger_Damage>().damageAmount = itemUtilityThrowable.baseDamage;
            // set the damage type,
            // Damage trigger does not accept damage type yet
            // set the knock back force,
            liveItem.transform.GetChild(2).GetComponent<Trigger_Force>().forceStrength = itemUtilityThrowable.forceStrength;
            // set the knock back delay,
            liveItem.transform.GetChild(2).GetComponent<Trigger_Force>().removeEffectDelay = itemUtilityThrowable.removeForceDelay;
            // set the knock back team,
            liveItem.transform.GetChild(2).GetComponent<Trigger_Force>().owningTeam = entity.currentStats.team;
            // set the speed of the object
            liveItem.GetComponent<Rigidbody2D>().AddForce(liveItem.transform.up*itemUtilityThrowable.speed, ForceMode2D.Impulse);
            // set the drag of the object
            liveItem.GetComponent<Rigidbody2D>().drag = itemUtilityThrowable.drag;
            // set the object's life time
            liveItem.GetComponent<Object_LiveThrowable>().throwable = equippedItems[currentAction] as Item_Utility_Throwable;
            StartCoroutine(liveItem.GetComponent<Object_LiveThrowable>().ActivateSubAction(itemUtilityThrowable.lifetime));
            StartCoroutine(UseDelay(0.25f));
            equippedItems[currentAction] = null;
        }
    }
}

