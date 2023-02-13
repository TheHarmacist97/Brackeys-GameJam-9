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
    private void Initialise()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
        this.fpp = GetComponent<Character>().fpp;
        this.tpp = GetComponent<Character>().tpp;
        fppTrigger = new Trigger(()=>SetFPP(true), ()=>SetFPP(false));
        SetFPP(false);
    }
    private void Update()
    {
        if(Input.GetMouseButton(1))
            fppTrigger.Fire();
        else
            fppTrigger.Reset();

        if(fppTrigger.fire)
        {
            tpp.transform.rotation = Quaternion.Euler(0f, fpp.transform.eulerAngles.y, 0f);
        }
        else
        {
            fpp.transform.rotation = Quaternion.Euler(0f, tpp.transform.eulerAngles.y, 0f);
        }
    }
    private void SetFPP(bool isFPP)
    {
        fpp.SetActive(isFPP);
        tpp.SetActive(!isFPP);
    }
}
