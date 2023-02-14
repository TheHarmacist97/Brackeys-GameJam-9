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
    private Vector3 velocity;
    private float smoothTurnVelocity;
    private float acceleration = GameConfig.MAX_GAME_GRAVITY;
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
        //GET PLAYER INPUT
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0)
        {
            velocity = characterSpecs.maxMoveSpeed * Time.deltaTime * ((transform.right * x) + (transform.forward * z));
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camTransform.eulerAngles.y, ref smoothTurnVelocity, characterSpecs.rotateSpeed);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        velocity.y = acceleration * Time.deltaTime;
        acceleration -= Time.deltaTime * characterSpecs.mass;
        acceleration = Mathf.Max(acceleration, GameConfig.MAX_GAME_GRAVITY);
        if (Physics.CheckSphere(groundCheck.position, 0.5f, groundMask))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
            acceleration = Mathf.Max(acceleration, 0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                acceleration = characterSpecs.jumpForce;
            }
        }
        controller.Move(velocity);
    }
}
