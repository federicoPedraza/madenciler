using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitBuffs : MonoBehaviour
{
    public List<AppliedBuff> buffs = new List<AppliedBuff>();
    [Header("Player Settings")]
    public PlayerMovement playerMovement;
    private PhotonView PV;
    public Vector3 effectOffset;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (buffs.Count == 0) return;
        Tick();
    }


    public void Tick()
    {
        buffs.RemoveAll(buff => buff.duration <= 0);

        foreach (AppliedBuff buff in buffs)
        {
            buff.duration -= Time.deltaTime;
            if (buff.duration <= 0)
            {
                //Remove buff (TODO: Improve this)
                switch (buff.spellBuff.type)
                {
                    case BuffType.MOVEMENT:
                        if (buff.spellBuff.movementMultiplies)
                            playerMovement.speedMultiplier -= buff.spellBuff.movementModifier;
                        else
                            playerMovement.speedBuff -= buff.spellBuff.movementModifier;
                        break;
                    case BuffType.DAMAGE:
                        //Deal damage
                        break;
                }

                //Remove visual buff
                PhotonNetwork.Destroy(buff.effect);
            }
        }
    }



    public void ApplyBuff(SpellBuff buff)
    {
        AppliedBuff newBuff = new AppliedBuff(buff);
        buffs.Add(newBuff);

        //Visual effect
        GameObject visualEffect = PhotonNetwork.Instantiate(buff.effect.name, transform.position, transform.rotation);
        visualEffect.transform.SetParent(transform);
        visualEffect.transform.localPosition = Vector3.zero + effectOffset;

        newBuff.effect = visualEffect;        

        switch (buff.type)
        {
            case BuffType.MOVEMENT:
                if (buff.movementMultiplies)
                    playerMovement.speedMultiplier += buff.movementModifier;
                else
                    playerMovement.speedBuff += buff.movementModifier;
                break;
            case BuffType.DAMAGE:
                //Deal damage
                break;
        }
    }

    public void RemoveBuff(AppliedBuff buff)
    {
        buffs.Remove(buff);
    }
}

public class AppliedBuff
{
    public SpellBuff spellBuff;
    public float duration;
    public bool removable = false;
    public GameObject effect;

    public AppliedBuff(SpellBuff buff)
    {
        this.spellBuff = buff;
        this.duration = buff.duration;
    }
}
