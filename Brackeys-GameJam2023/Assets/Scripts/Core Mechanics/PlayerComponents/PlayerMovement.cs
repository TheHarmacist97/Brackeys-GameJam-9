using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private CharacterSO characterSpecs;
    private Transform groundCheck;
    private LayerMask groundMask;
    private Vector3 velocity;
    private float acceleration = GameConfig.Constants.MAX_GAME_GRAVITY;


    private void OnEnable() => EnableComponent();
    private void OnDisable() => DisableComponent();

    private void EnableComponent()
    {
        characterSpecs = GetComponent<Character>().CharacterSpecs;
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
        groundCheck = transform.GetChild(1).GetComponent<Transform>();
        groundMask = LayerMask.GetMask(GameConfig.Constants.GROUND_TAG);
    }

    private void DisableComponent()
    {
        velocity = Vector3.zero;
        controller.enabled = false;
    }

    void Update()
    {
        MovementLogic();
    }
    private void MovementLogic()
    {
        //GET PLAYER INPUT
        float x = Input.GetAxisRaw(GameConfig.Constants.INPUT_HORIZONTAL);
        float z = Input.GetAxis(GameConfig.Constants.INPUT_VERTICAL);
        if (z != 0)
        {
            velocity = characterSpecs.maxMoveSpeed * Time.deltaTime * (transform.forward * z);
        }
        if(x != 0)
        {
            Vector3 rot = transform.rotation.eulerAngles + Vector3.up * (x * characterSpecs.rotateSpeed* Time.deltaTime);
            transform.rotation = Quaternion.Euler(rot);
        }
        velocity.y = acceleration * Time.deltaTime;
        acceleration -= Time.deltaTime * characterSpecs.mass;
        acceleration = Mathf.Max(acceleration, GameConfig.Constants.MAX_GAME_GRAVITY);
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
    private void OnDestroy()
    {
        Destroy(controller);
    }
}
