using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold,
        Menu
    }

    private float holdTime = 0;
    public float minHoldTime = 1f;

    public InteractionType type;
    public abstract string GetLabel();
    public abstract void Interact();
    public void IncreaseHoldTime()
    {
        holdTime += Time.deltaTime;
        if (holdTime > minHoldTime)
            Interact();
            ResetHoldTime();
    }
    public void ResetHoldTime() => holdTime = 0;
    public float GetHoldTime() => holdTime;
}
