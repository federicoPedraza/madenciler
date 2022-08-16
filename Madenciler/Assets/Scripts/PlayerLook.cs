using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public bool isEnabled = true;
    public float sensitivity = 100f;
    public float trackingSpeed = 10f;
    public Transform playerBody;

    private float rotation = 0f;

    private float x;
    private float y;

    public bool isTracking;
    private Vector3 trackingPoint;

    [Header("Networking")]
    private PhotonView photonView;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        photonView = GetComponentInParent<PhotonView>();

        if (!photonView.IsMine)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        x = Input.GetAxisRaw("Mouse X");
        y = Input.GetAxisRaw("Mouse Y");

        isTracking = Input.GetMouseButton(1);

        if (Input.GetMouseButtonDown(1))
        {
            trackingPoint = GetPointForward();
            isTracking = true;
        }

        if (Input.GetMouseButtonUp(1))
            isTracking = false;


        if (!isEnabled || isTracking) return;

        rotation -= y;
        rotation = Mathf.Clamp(rotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotation, 0, 0);

        playerBody.Rotate(Vector3.up * x);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (isTracking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(trackingPoint - transform.position), Time.deltaTime * trackingSpeed);

            Vector3 trackingPointFlatX = new Vector3(trackingPoint.x, playerBody.position.y, trackingPoint.z);
            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, Quaternion.LookRotation(trackingPointFlatX - playerBody.position), Time.deltaTime * trackingSpeed);

            return;
        }

    }

    private Vector3 GetPointForward()
    {
        Vector3 point = transform.forward;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
        }

        return point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, trackingPoint);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, GetPointForward());
    }
}
