using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEnemy : MonoBehaviour
{
    Animator characterController;
    float speed;
    Vector3 lastPosition;
    private void Awake()
    {
        characterController = GetComponent<Animator>();

    }
    void Start()
    {
        lastPosition = transform.position;
    }
    void Update()
    {
        speed = (transform.position - lastPosition).magnitude;
        characterController.SetFloat("Speed", -speed*11f);
        lastPosition = transform.position;
    }
}
