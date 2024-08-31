using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public float base_speed = 5.0f;     // Base movement speed
    public float run_speed = 10.0f;     // Running speed
    public float jumpSpeed = 8.0f;      // Jumping speed
    public float gravity = 20.0f;       // Gravity force
    public float lookSpeed = 2.0f;      // Mouse sensitivity
    public float lookXLimit = 80.0f;    // Vertical look limit

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private Camera playerCamera;
    private Camera weaponCamera;
    public Shovel shovel;


    private float rotationX = 0;

    private AudioSource footsteps;
    private AudioSource jumpSound;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        weaponCamera = GameObject.Find("WeaponCamera").GetComponent<Camera>();
        footsteps = GameObject.Find("Footsteps").GetComponent<AudioSource>();
        //footsteps.Play();
        jumpSound = GameObject.Find("JumpSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameState.Instance.isPaused) return;

        // Handle rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        weaponCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        

        // Handle movement
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= Input.GetButton("Fire3") ? run_speed : base_speed;
            if (false) // Disabled, because we don't know why footsteps are being destroyed sometimes.
            {
                if (Input.GetButton("Fire3"))
                {
                    footsteps.pitch = 1.5f;
                    footsteps.volume = 1.0f;
                }
                else
                {
                    footsteps.pitch = 1.0f;
                    footsteps.volume = 0.5f;
                }
                if (moveDirection.magnitude < 0.01f)  // Needs to be before gravity and jump.
                    footsteps.Pause();
                else
                    footsteps.UnPause();
            }

            if (Input.GetButton("Jump"))
            {
                jumpSound.Play();
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
