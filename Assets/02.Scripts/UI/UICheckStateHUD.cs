using System.Collections;
using System.Collections.Generic;

using FSM;

using TMPro;

using UnityEngine;

public class UICheckStateHUD : UIHUDBase
{
    [SerializeField] private TextMeshProUGUI _stateText;

    public void SetStateText(IState state)
    {
        switch (state)
        {
            case IdleState:
                _stateText.text = "IdleState";
                break;
            case WalkState:
                _stateText.text = "WalkStte";
                break;
            case SprintState:
                _stateText.text = "SprintState";
                break;
            case JumpState:
                _stateText.text = "JumpState";
                break;
            case FallingState:
                _stateText.text = "FallingState";
                break;
            case LandingState:
                _stateText.text = "LandingState";
                break;
            case CrouchState:
                _stateText.text = "CrouchState";
                break;
            case AttackState:
                _stateText.text = "AttackState";
                break;
        }
    }
}
