using System;
using UnityEditor;
using UnityEngine;

public class PlayerMovementVFX : MonoBehaviour
{
    private CharacterData data;
    private Camera cameraMain;
    private Transform targetTransform;
    private Vector3 lerpPosition;
    private Ray ray;
    private RaycastHit raycastHit;
    private void Awake()
    {
        cameraMain = Camera.main;
        data = GetComponent<Character>().data;
        targetTransform = data.target;
    }

    // Update is called once per frame
    private void Update()
    {
        ray = cameraMain.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
    
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            lerpPosition = raycastHit.point;
        }
        else
        {
            lerpPosition = ray.GetPoint(100f);
        }
    }
    private void LateUpdate()
    {
        targetTransform.position = Vector3.Lerp(targetTransform.position, lerpPosition, 10f * Time.deltaTime) ;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(cameraMain.transform.position, raycastHit.point*20f);
        Gizmos.DrawSphere(targetTransform.position, 2f);
    }

}
