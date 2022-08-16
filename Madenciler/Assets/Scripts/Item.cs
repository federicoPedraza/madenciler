using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Interactable
{
    public abstract void OnGrab();
    public abstract void OnRelease();

    public override void Interact()
    {
        OnGrab();
    }
}
