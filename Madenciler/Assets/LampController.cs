using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class LampController : Interactable, IPunObservable
{
    public bool canBeTurnOff = true;
    public bool isOn = true;
    public string label = "Lamp";
    public GameObject[] lights;
    public bool turnOffRenderer;

    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override string GetLabel()
    {
        return label + " (" + (isOn ? "On)" : "Off)");
    }

    public override void Interact()
    {
        PV.RPC("SwitchLights", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SwitchLights()
    {
        if (canBeTurnOff) isOn = !isOn;

        if (turnOffRenderer) GetComponent<MeshRenderer>().enabled = isOn;

        foreach (GameObject light in lights)
            light.SetActive(isOn);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
