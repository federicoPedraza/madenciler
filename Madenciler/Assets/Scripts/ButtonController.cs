using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : Interactable
{
    public int id = 0;
    public UnityEvent onClick = new UnityEvent();

    public override string GetLabel()
    {
        return "Button";
    }

    public override void Interact()
    {
        onClick.Invoke();
    }
}
