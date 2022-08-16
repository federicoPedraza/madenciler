using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement
{
    public List<CastMove> currentMovements = new List<CastMove>();
    public List<SpellSettings> spells = new List<SpellSettings>();

    public SpellMovement(List<SpellSettings> spells)
    {
        this.spells = spells;
    }
}

[System.Serializable]
public enum CastMove
{
    NONE,
    DOWN,
    UP,
    LEFT,
    RIGHT
}

