using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private CharacterSpecs characterSpecs;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float mass = 10;
    [SerializeField] private float jumpForce = 10;
    private Vector3 velocity;
    private float smoothTurnTime = .1f;
    private float smoothTurnVelocity;
    private float acceleration = -9.81f;
    private Transform camTransform;
    private void OnEnable() => EnableComponent();
    private void OnDisable() => velocity = Vector3.zero;

    private void EnableComponent()
    {
        characterSpecs = GetComponent<Character>().CharacterSpecs;
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(1).GetComponent<Transform>();
        groundMask = LayerMask.GetMask("Ground");
        camTransform = Camera.main.transform;
    }
    void Update()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0)
        {
            velocity = characterSpecs.maxMoveSpeed * Time.deltaTime * ((transform.right * x) + (transform.forward * z));
            transform.rotation = Quaternion.Euler(0f, camTransform.eulerAngles.y, 0f);
        }
        acceleration -= Time.deltaTime * mass;
        acceleration = Mathf.Max(acceleration, -9.81f);
        velocity.y += acceleration * Time.deltaTime;
        if (Physics.CheckSphere(groundCheck.position, 0.5f, groundMask))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                acceleration = jumpForce;
            }
        }
        controller.Move(velocity);
    }
}
