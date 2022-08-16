using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun; 

public class PlayerSpelling : MonoBehaviour
{
    private PlayerCasting casting;
    private UnitBuffs unitBuff;
    public SpellDisplayController drawingTranslation;
    public List<SpellSettings> allSpells;
    public List<CastMove> moves = new List<CastMove>();

    private void Awake()
    {
        casting = GetComponent<PlayerCasting>();
        unitBuff = GetComponentInParent<UnitBuffs>();
    }

    public void StartSpell()
    {
        moves = new List<CastMove>();
    }

    public void EndSpell()
    {
        List<SpellSettings> spells = allSpells.Where(spell => spell.element == casting.drawing.element).ToList();
        foreach (SpellSettings spell in spells)
        {
            if (CheckForEqualSpell(spell.moves.ToList(), moves))
            {
                //Throw spell
                Debug.Log($"Matched: {spell.name}");
                unitBuff.ApplyBuff(spell.buff);
            }
        }    
        drawingTranslation.ClearText();
    }

    public void ProcessNode(Node node)
    {
        CastMove move = LastMoveToCastMove(node);
        SpellElements element = DrawingUtils.GetSpellElement(move);

        //In this case, the drawing is new since there's no node before this one
        if (element != SpellElements.NONE && casting.drawing.element == SpellElements.NONE)
        {
            casting.drawing.element = element;
            float[] color = DrawingUtils.GetElementColorRGB(element);
            casting.drawingEffect.PV.RPC("Paint", RpcTarget.AllBuffered, color);
            drawingTranslation.UpdateText(DrawingUtils.CastMoveToString(move), DrawingUtils.GetInTextElementColor(move));
            casting.drawing.GetLastNode().direction = move;
            casting.drawing.moves.Add(move);
            moves.Add(move);
            return;
        }

        casting.drawing.GetLastNode().direction = move;
        if (casting.drawing.GetLastNode().linkedNode.direction != move)
        {
            drawingTranslation.UpdateText(DrawingUtils.CastMoveToString(move), DrawingUtils.GetInTextElementColor(move));
            casting.drawing.moves.Add(move);
            moves.Add(move);
        }

    }


    private bool CheckForEqualSpell(List<CastMove> list1, List<CastMove> list2)
    {
        var areListsEqual = true;

        if (list1.Count != list2.Count)
            return false;

        list1.Sort();
        list2.Sort();

        for (var i = 0; i < list1.Count; i++)
        {
            if (list2[i] != list1[i])
            {
                areListsEqual = false;
            }
        }

        return areListsEqual;
    }

    public CastMove LastMoveToCastMove(Node node)
    {
        Node dirNode = node - casting.drawing.GetPreviousNode();
        DebuggingTool.Debug($"({dirNode.x},{dirNode.y})", "node_coordinates");

        CastMove move = CastMove.NONE;

        if (dirNode.x != 0 && move == CastMove.NONE)
            move = dirNode.x > 0 ? CastMove.RIGHT : CastMove.LEFT;

        if (dirNode.y != 0 && move == CastMove.NONE)
            move = dirNode.y > 0 ? CastMove.UP : CastMove.DOWN;

        return move;
    }
}
