using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AimOverrideControlTPS : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("How far can the bullet be shot for our hitscan")]
    public float shotDistance = 200.0f;
    
    [Tooltip("Used to control the rate at which layers in the animation change")]
    public float animationLayerTransitionRate = 10.0f;
    
    [Tooltip("Used to control how far it projects on a miss, useful for aiming")]
    public float missedRaycastDistance = 20.0f;

    [Header("Camera Data")]
    [SerializeField]
    [Tooltip("Camera used for aiming, set to control")]
    CinemachineVirtualCamera aimCam = null;

    [SerializeField]
    [Tooltip("Camera's standard sensitivity")]
    float camSensitivity;

    [SerializeField]
    [Tooltip("Camera's sensitivity whem aiming")]
    float aimCamSensitivity;

    [Header("Layer Masking")]
    [SerializeField]
    [Tooltip("Layers to ignore")]
    LayerMask aimLayerMask = new LayerMask();

    [SerializeField]
    [Tooltip("Provide a spawm position for a bullet")]
    Transform bulletSpawnPos;
    [SerializeField]
    [Tooltip("Provide a bullet to be spawned")]
    GameObject bulletPrefab;

    public Transform aimMarkerTransform;

    PlayerControlsTPS controller;
    InputManagerTPS input;
    Animator animator;
    Vector2 currentAnimationVec;
    Vector2 animationDirection;
    Camera mainCam;


    void Awake()
    {
        controller = GetComponent<PlayerControlsTPS>();
        input = GetComponent<InputManagerTPS>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector2 movement = input.move.normalized;
        currentAnimationVec = Vector2.SmoothDamp(currentAnimationVec, movement, ref animationDirection, 0.1f, 1.0f);

        animator.SetBool("IsStillADS", movement == Vector2.zero);   
        animator.SetBool("IsMotionADS", input.aim);   
        animator.SetFloat("ForwardMotion", currentAnimationVec.y);   
        animator.SetFloat("RightMotion", currentAnimationVec.x);

        Vector3 mouseWorldPos;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        Transform hitRay = null;

        if(Physics.Raycast(ray, out RaycastHit hit, shotDistance, aimLayerMask))
        {
            aimMarkerTransform.position = hit.point; 
            mouseWorldPos = hit.point;
            hitRay = hit.transform;
        }
        else
        {
            aimMarkerTransform.position = ray.GetPoint(missedRaycastDistance);
            mouseWorldPos = ray.GetPoint(missedRaycastDistance);
        }

        if (input.aim)
        {
            aimCam.gameObject.SetActive(true);
            controller.SetCamSensitivitty(aimCamSensitivity);
            controller.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * animationLayerTransitionRate));
            
            Vector3 aimTarget = mouseWorldPos;
            aimTarget.y = transform.position.y;
            Vector3 aimDir = (aimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20);
        }
        else
        {
            aimCam.gameObject.SetActive(false);
            controller.SetCamSensitivitty(camSensitivity); 
            controller.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * animationLayerTransitionRate));
        }
    }
}
