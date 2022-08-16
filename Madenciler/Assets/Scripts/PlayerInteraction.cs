using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject objectInFront = null;
    public float interactionDistance = 2.4f;
    public Camera playerCamera;

    [Header("User's preferences")]
    public GameObject interactionIndicator = null;
    public KeyCode interactKey = KeyCode.E;

    [Header("Networking")]
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (!photonView.IsMine) return;
        GetObjectInFront();
    }

    public void GetObjectInFront()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
                
            if (interactable != null)
            {
                interactionIndicator.SetActive(true);
                HandleInteraction(interactable);
                return;
            }
        }
        interactionIndicator.SetActive(false);
    }

    public void HandleInteraction(Interactable target)
    {
        switch (target.type)
        {
            case Interactable.InteractionType.Click: 
                if (Input.GetKeyDown(interactKey))
                    target.Interact();
                break;
            case Interactable.InteractionType.Hold: 
                if (Input.GetKey(interactKey))
                    target.IncreaseHoldTime();
                else
                    target.ResetHoldTime();
                break;
            case Interactable.InteractionType.Menu:
                //make menu appear
                break;
        }
    }
}
