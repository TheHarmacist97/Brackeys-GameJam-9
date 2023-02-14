using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraSystem : MonoBehaviour
{
    private GameObject fpp;
    private GameObject tpp;
    private Trigger fppTrigger;
    private void OnEnable() => Initialise();
    private void OnDisable()
    {

    }
    #region Enable Functions
    private void Initialise()
    {
        Character character = GetComponent<Character>();

        SetFollow();

        SetCursor(true);

        this.fpp = character.FirstPersonCamera;
        this.tpp = character.ThirdPersonCamera;

        fppTrigger = new Trigger(() => SetFPP(true), () => SetFPP(false));

        SetFPP(false);
    }
    
    private void SetCursor(bool cursorLocked)
    {
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLocked;
    }
    private void SetFollow()
    {
        fpp.GetComponent<CinemachineVirtualCamera>().Follow = transform;
        tpp.GetComponent<CinemachineVirtualCamera>().Follow = transform;
    }

    #endregion

    private void Update()
    {
        if(!GameConfig.fppToggle)
        {
            FPPHold();
        }
        else
        {
            FPPToggle();
        }

    }

    #region Update Functions

    /// <summary>
    /// Toggle between First person and third Person Mode
    /// </summary>
    /// <param name="isFPP">bool=> true: First-Person Mode :: false: Third-Person Mode</param>
    private void SetFPP(bool isFPP)
    {
        fpp.SetActive(isFPP);
        tpp.SetActive(!isFPP);
    }
    /// <summary>
    /// Logic for switching to FPP on Hold
    /// </summary>
    private void FPPHold()
    {
        if (Input.GetMouseButton(1))
            fppTrigger.Fire();
        else
            fppTrigger.Reset();
    }
    /// <summary>
    /// Logic for Switching to FPP on Toggle
    /// </summary>
    private void FPPToggle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (fppTrigger.fire)
                fppTrigger.Reset();
            else
                fppTrigger.Fire();
        }
    }
    #endregion

}
