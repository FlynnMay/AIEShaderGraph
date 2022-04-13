using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControlsTPS : MonoBehaviour
{
    [Header("Player Movement")]
    [Tooltip("Movement speed of the player")]
    public float movementSpeed = 4.0f;
    [Tooltip("Sprinting speed of the player")]
    public float runningSpeed = 6.0f;
    [Tooltip("Player turn rate")]
    [Range(0f, 0.4f)]
    public float rotationSmoothRate = 0.1f;
    [Tooltip("Player rate of speed change")]
    public float speedChangeRate = 10.0f;

    [Space(10)]
    [Header("Player Grounded")]
    [Tooltip("Check if the player is grounded")]
    public bool grounded = true;
    [Tooltip("Rough ground offset, useful for complicated terrains")]
    [Range(-1, 1)]
    public float groundedOffset = -0.15f;
    [Tooltip("Radius of the above check")]
    public float groundedRadius = 0.3f;
    [Tooltip("Layers approved to be the 'ground'")]
    public LayerMask groundLayers;

    [Space(10)]
    [Header("Player Cinemachine Camera Controls")]
    [Tooltip("Follow target set for the virtual camera that is active")]
    public GameObject cinemachineVirtualCamera;
    [Tooltip("In degrees, how high up it moves")]
    public float topClamp = 70.0f;
    [Tooltip("In degrees, how far down it moves")]
    public float bottomClamp = -30.0f;
    [Tooltip("Testing/overloading the currently accepted degrees")]
    public float cameraClampOverride;
    [Tooltip("Locking the camera on each axis")]
    public bool lockCameraPosition;
    [Tooltip("Camera Sensitivity Speed Adjustment")]
    public float cameraSensitivity;

    // Cinemachine Camera
    float cinemachineTargetYaw;
    float cinemachineTargetPitch;

    // Player Ground Settings
    float speed;
    float animationBlend;
    float rotation = 0.0f;
    float rotationVelocity;
    bool rotateOnMove = true;
    float forwardVelocity;
    float maxForwardVelocity = 55.0f;

    Animator animator;
    CharacterController characterController;
    InputManagerTPS input;
    GameObject mainCamera;

    bool hasAnimator;
    const float threshold = 0.01f;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Start()
    {
        hasAnimator = TryGetComponent<Animator>(out animator);
        characterController = GetComponent<CharacterController>();
        input = GetComponent<InputManagerTPS>();
    }

    void Update()
    {
        GroundCheck();
        Moving();
    }

    void LateUpdate()
    {
        CameraRotation();    
    }

    void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (hasAnimator)
            animator.SetBool("IsGrounded", grounded);
    }

    void CameraRotation()
    {
        if (input.look.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            cinemachineTargetYaw += input.look.x * Time.deltaTime * cameraSensitivity;
            cinemachineTargetPitch += input.look.y * Time.deltaTime * cameraSensitivity;
        }

        // make sure the camera is clamped
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        cinemachineVirtualCamera.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraClampOverride, cinemachineTargetYaw, 0.0f);
    }

    void Moving()
    {
        // change speed depending on if the sprint key is pressed or not
        float targetSpeed = input.run ? runningSpeed : movementSpeed;

        if (input.move == Vector2.zero)
            targetSpeed = 0f;

        // get the players current speeds
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMag = input.movement ? input.move.magnitude : 1.0f;

        // adjust to the target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMag, Time.deltaTime * speedChangeRate);
            speed = Mathf.Round(speed * 1000f) / 1000f; // this will bind speed to 3 decimal places
        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        // normalize the input direction
        Vector3 inputDir = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        if (input.move != Vector2.zero)
        {
            rotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotate = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, rotationSmoothRate);
            if (rotateOnMove)
                transform.rotation = Quaternion.Euler(0.0f, rotate, 0.0f);
        }

        Vector3 targetDir = Quaternion.Euler(0.0f, rotation, 0.0f) * Vector3.forward;

        characterController.Move(targetDir.normalized * (speed * Time.deltaTime) + new Vector3(0f, forwardVelocity, 0.0f) * Time.deltaTime);

        if (hasAnimator)
        {
            Vector2 move = new Vector2(forwardVelocity, currentHorizontalSpeed).normalized;
            animator.SetFloat("Speed", animationBlend);
            animator.SetFloat("SpeedMultiplier", inputMag);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360;
        if (angle > 360f)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    void OnDrawGizmosSelected()
    {
        Color transparentPurple = new Color(0.5f, 0.0f, 0.5f, 0.4f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.4f);

        Gizmos.color = grounded ? transparentPurple : transparentRed;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }

    public void SetCamSensitivitty(float camSensitivity)
    {
        cameraSensitivity = camSensitivity;
    }

    public void SetRotateOnMove(bool rotate)
    {
        rotateOnMove = rotate;
    }
}
