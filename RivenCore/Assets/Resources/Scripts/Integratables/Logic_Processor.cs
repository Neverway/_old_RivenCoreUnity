//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic_Processor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


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


    //=-----------------=
    // External Functions
    //=-----------------=
    public void UpdateState(string _targetSignalChannel, bool _isPowered)
    {
        foreach (var interactable in FindObjectsOfType<Logic_Interactable>())
        {
            // Send activation state signal to all listeners on same channel
            if (interactable.signalChannel == _targetSignalChannel) interactable.isPowered = _isPowered;
        }

        foreach (var logicGate in FindObjectsOfType<LogicGate_And>())
        {
            // Send activation state signal to all listeners on same channel
            if (logicGate.inputSignalA == _targetSignalChannel) logicGate.isAPowered = _isPowered;
            if (logicGate.inputSignalB == _targetSignalChannel) logicGate.isBPowered = _isPowered;
        }

        foreach (var logicGate in FindObjectsOfType<LogicGate_Not>())
        {
            // Send activation state signal to all listeners on same channel
            if (logicGate.inputSignal == _targetSignalChannel) logicGate.isInputPowered = _isPowered;
        }

        foreach (var logicGate in FindObjectsOfType<LogicGate_Or>())
        {
            // Send activation state signal to all listeners on same channel
            if (logicGate.inputSignalA == _targetSignalChannel) logicGate.isAPowered = _isPowered;
            if (logicGate.inputSignalB == _targetSignalChannel) logicGate.isBPowered = _isPowered;
        }

        foreach (var trigger in FindObjectsOfType<Trigger_Interactable>())
        {
            // Send activation state signal to all listeners on same channel
            if (trigger.resetSignal == _targetSignalChannel) trigger.wasActivated = false;
        }

        foreach (var trigger in FindObjectsOfType<Trigger_Event>())
        {
            // Send activation state signal to all listeners on same channel
            if (trigger.resetSignal == _targetSignalChannel) trigger.wasActivated = false;
        }
    }
}
