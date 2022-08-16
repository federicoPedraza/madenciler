using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell buff")]
public class SpellBuff : ScriptableObject
{
    public SpellSettings spellSettings;
    public BuffType type;
    public GameObject effect;
    [Header("Buff settings")]
    public float duration = 5f;

    public float movementModifier = 0;
    [Tooltip("Multiplies the movement instead of changing it")]
    public bool movementMultiplies;
}

public enum BuffType
{
    MOVEMENT,
    REGENERATION,
    DAMAGE,
}