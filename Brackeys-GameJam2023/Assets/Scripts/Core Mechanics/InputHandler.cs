using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private InputAbility inputAbility;
    private PlayerCameraSystem playerMovement;
    private Transform targetTransform;
    private void OnEnable()
    {
        Initialise();
    }
    private void Initialise()
    {
        inputAbility= GetComponent<InputAbility>();
        playerMovement= GetComponent<PlayerCameraSystem>();
        targetTransform = GetComponent<Character>().data.target;
    }
    private void Update()
    {
        PrimaryButtonLogic();
        SecondaryButtonLogic();
        ReloadButtonLogic();
    }

    private void PrimaryButtonLogic()
    {
        if (Input.GetKeyDown(GameConfig.PRIMARY_FIRE_BUTTON))
        {
            inputAbility.Fire(targetTransform.position);
        }
        else if (Input.GetKey(GameConfig.PRIMARY_FIRE_BUTTON))
        {
            inputAbility.FireContinually(false, targetTransform.position);
        }
        else if (Input.GetKeyUp(GameConfig.PRIMARY_FIRE_BUTTON))
        {
            inputAbility.StopFiring();
        }
    }

    private void SecondaryButtonLogic()
    {
        if (Input.GetKeyDown(GameConfig.SECONDARY_FIRE_BUTTON) && GameConfig.fppToggle)
        {
            playerMovement.FPPToggle();
        }
        else if (Input.GetKey(GameConfig.SECONDARY_FIRE_BUTTON) && !GameConfig.fppToggle)
        {
            playerMovement.FPPHold(true);
        }
        else if (Input.GetKeyUp(GameConfig.SECONDARY_FIRE_BUTTON) && !GameConfig.fppToggle)
        {
            playerMovement.FPPHold(false);
        }
    }

    private void ReloadButtonLogic()
    {
        if (Input.GetKeyDown(GameConfig.RELOAD_BUTTON))
        {
            inputAbility.ReloadAll();
        }
    }
}
