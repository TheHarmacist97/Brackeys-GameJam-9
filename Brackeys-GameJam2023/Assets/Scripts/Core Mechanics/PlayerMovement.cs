using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private CharacterSpecs characterSpecs;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;

    private void OnEnable() => EnableComponent();
    private void OnDisable() => velocity = Vector3.zero;

    private void EnableComponent()
    {
        characterSpecs = GetComponent<Character>().CharacterSpecs;
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
        groundMask = LayerMask.GetMask("Ground");
    }
    void Update()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        velocity = characterSpecs.maxMoveSpeed * Time.deltaTime * ((transform.right * x) + (transform.forward * z));
        if(Physics.CheckSphere(groundCheck.position, 0.5f, groundMask))
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y +=  -9.81f * Time.deltaTime;
        }
        controller.Move(velocity);
    }
}
