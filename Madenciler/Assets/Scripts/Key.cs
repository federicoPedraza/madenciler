using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public string keyName = "Key";
    public Collider placeholder;

    public override string GetLabel()
    {
        return keyName;
    }

    public override void OnGrab()
    {

    }

    public override void OnRelease()
    {

    }
}
