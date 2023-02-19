using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHAnimations : MonoBehaviour
{
    [SerializeField]private float rotateAfter;
    [SerializeField]private float maxSpeed;
    [SerializeField]private float minSpeed;
    private Transform sphere;
    private Vector3 rotationDirection;
    private float currentSpeed;
    private MeshRenderer meshRender;
    private Material material;
    private void Start()
    {
        meshRender= GetComponent<MeshRenderer>();
        Material[] mats = meshRender.materials;
        material = mats[1];
        rotationDirection = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        sphere = transform.GetChild(0);
        SetLerp(0);
        StartCoroutine(UpdateRotationDirection());
    }
    void Update()
    {
        sphere.rotation = Quaternion.Euler(sphere.eulerAngles + (rotationDirection * currentSpeed));
    }

    public void SetLerp(float value)
    {
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, value);
        material.SetFloat("_LerpValue", value);
    }

    private IEnumerator UpdateRotationDirection()
    {
        while(true)
        {
            rotationDirection = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            yield return new WaitForSeconds(rotateAfter);
        }
    }

}
