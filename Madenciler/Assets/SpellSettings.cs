using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Invokable Spell")]
public class SpellSettings : ScriptableObject
{
    public SpellElements element;
    public SpellType type;
    public string spellCode = "";
    public CastMove[] moves;
    public bool requiresHolding;
    public int length => moves.Length + 1;

    [Header("Buff settings")]
    public bool isBuff;
    public SpellBuff buff;
}

public enum SpellElements
{
    NONE,
    FIRE,
    AIR,
    WATER,
    EARTH
}
public enum SpellType
{
    AREA,
    SELFTARGET,
    MULTITARGET,
    ALLYTARGET,
    ENEMYTARGET,
    PROYECTILE
}