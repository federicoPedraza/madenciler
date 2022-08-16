using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private float forwardMovement = 0;
    private float sideMovement = 0;

    public float walkSpeed = 1f;
    public float sprintSpeed = 1.26f;
    public float speedBuff = 0;
    public float speedMultiplier = 1f;
    private float currentSpeed = 0;

    public Light personalLight;
    public float maxLightIntensity = 1.1f;
    public float minLightIntensity = 0f;
    private float currentLightIntensity = .7f;
    private const float INTENSITY_SENSITIVITY = 4f;

    private Rigidbody rb;

    [Header("Networking")]
    private PhotonView photonView;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();


        if (photonView.IsMine)
        {
            personalLight = Instantiate(personalLight, transform);
            personalLight.transform.localPosition = Vector3.up / 2;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        currentSpeed += speedBuff;
        currentSpeed *= speedMultiplier;

        forwardMovement = Input.GetAxisRaw("Vertical") * currentSpeed;
        sideMovement = Input.GetAxisRaw("Horizontal") * currentSpeed;

        currentLightIntensity += Input.mouseScrollDelta.y * Time.deltaTime * INTENSITY_SENSITIVITY;
        currentLightIntensity = Mathf.Clamp(currentLightIntensity, minLightIntensity, maxLightIntensity);
        personalLight.intensity = currentLightIntensity;
    }

    private void FixedUpdate()
    {
        rb.velocity = (transform.forward * forwardMovement) + (transform.right * sideMovement) + (transform.up * rb.velocity.y);
    }
}
