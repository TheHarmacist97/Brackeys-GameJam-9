using Cinemachine;
using UnityEngine;

public class PlayerCameraSystem : MonoBehaviour
{
    private GameObject fpp;
    private GameObject tpp;
    private Trigger fppTrigger;
    private CameraProfile camProfile;
    CinemachineVirtualCamera fppCam;
    CinemachineFreeLook tppCam;
    private void OnEnable() => Initialise();
    private void OnDisable()
    {

    }
    #region Enable Functions
    private void Initialise()
    {
        Character character = GetComponent<Character>();
        this.fpp = character.FirstPersonCamera;
        this.tpp = character.ThirdPersonCamera;

        camProfile = character.data.cameraProfile;
        fppTrigger = new Trigger(() => SetFPP(true), () => SetFPP(false));
        SetFollow();
        ChangeCamProfile(tpp.GetComponent<CinemachineFreeLook>());

        SetCursor(true);
        

        SetFPP(false);
    }

    private void SetCursor(bool cursorLocked)
    {
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLocked;
    }
    private void SetFollow()
    {
        fppCam = fpp.GetComponent<CinemachineVirtualCamera>();
        
        if(transform.childCount > 2)
        {
            if (transform.GetChild(2).CompareTag(GameConfig.Constants.FPPTAG))
            {
                fppCam.Follow = transform.GetChild(2);
            }
        }
        else
        {
            fppTrigger.Clear();
        }
        fppCam.m_Lens.FieldOfView = camProfile.verticalFOVFPP;
        tppCam = tpp.GetComponent<CinemachineFreeLook>();
        tppCam.Follow = transform;
        tppCam.LookAt = transform;
    }

    #endregion

    private void Update()
    {
        if (!GameConfig.fppToggle)
        {
            FPPHold();
        }
        else
        {
            FPPToggle();
        }
    }

    private void ChangeCamProfile(CinemachineFreeLook currentFreeLook)
    {
        CinemachineComposer composer;
        CinemachineOrbitalTransposer transposer;
        for (int i = 0; i < 3; i++)
        {
            currentFreeLook.m_Orbits[i].m_Radius = camProfile.rigs[i].rigRadius;
            currentFreeLook.m_Orbits[i].m_Height = camProfile.rigs[i].rigHeight;

            composer = currentFreeLook.GetRig(i).GetCinemachineComponent<CinemachineComposer>();
            transposer = currentFreeLook.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>();

            transposer.m_YDamping = camProfile.rigs[i].bodyYDamping;
            transposer.m_ZDamping = camProfile.rigs[i].bodyZDamping;

            composer.m_ScreenX = camProfile.rigs[i].aimScreenX;
            composer.m_ScreenY = camProfile.rigs[i].aimScreenY;
            composer.m_SoftZoneWidth = camProfile.rigs[i].softZoneWidth;
            composer.m_SoftZoneHeight = camProfile.rigs[i].softZoneHeight;
        }

    }

    #region Update Functions

    /// <summary>
    /// Toggle between First person and third Person Mode
    /// </summary>
    /// <param name="isFPP">bool=> true: First-Person Mode :: false: Third-Person Mode</param>
    private void SetFPP(bool isFPP)
    {
        fppCam.Priority = isFPP ? 100 : 0;
        tppCam.Priority = isFPP ? 0 : 100;
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
