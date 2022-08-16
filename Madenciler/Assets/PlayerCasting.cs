using Photon.Pun;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerLook))]
public class PlayerCasting : MonoBehaviour, IPunObservable
{
    [Header("Networking")]
    private PhotonView photonView;

    public bool isCasting;
    [SerializeField]
    public Drawing drawing;

    [Header("Settings")]
    private PlayerSpelling playerSpelling;
    private PlayerLook playerLook;
    public float min_sensitivity = 4f;

    public float mouseX = 0;
    public float mouseY = 0;

    [Header("Visuals")]
    public DrawingEffect drawingEffect;
    public Vector3 drawingOffset;

    private void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
        playerSpelling = GetComponent<PlayerSpelling>();
        photonView = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        EnableDrawing();
    }   

    private void EnableDrawing()
    {
        if (Input.GetMouseButtonDown(0))
            SetupDrawing();

        if (Input.GetMouseButton(0))
            HandleDrawInput();
        else
            if (!drawing.isStopped) EndDrawing();

        if (Input.GetMouseButtonUp(0))
            EndDrawing();

        playerLook.isEnabled = !isCasting;
        Cursor.lockState = isCasting ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetupDrawing()
    {
        isCasting = true;

        //Drawing effect (cde = currentDrawingEffect)
        GameObject cde = PhotonNetwork.Instantiate("DrawingEffect", transform.position, Quaternion.identity);
        drawingEffect = cde.GetComponent<DrawingEffect>();

        //Positioning initial cde
        cde.transform.SetParent(transform);
        cde.transform.localPosition = GetCastingPosition();
        cde.transform.SetParent(null);

        drawing = new Drawing();
        playerSpelling.StartSpell();
        CreateNode(0, 0, true);
    }
    private void HandleDrawInput()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");

        mouseX = Mathf.Clamp(mouseX, -min_sensitivity, min_sensitivity);
        mouseY = Mathf.Clamp(mouseY, -min_sensitivity, min_sensitivity);
    }

    private Vector3 GetCastingPosition()
    {
        Vector3 dir = (transform.forward / 2) + drawingOffset;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            return hit.point;
        }

        return dir;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(mouseX) >= min_sensitivity || Mathf.Abs(mouseY) >= min_sensitivity)
            CreateNode(mouseX, mouseY);
    }

    private void CreateNode(float x, float y, bool onlyVisual = false)
    {
        Node node = Drawing.ProcessNode(x / min_sensitivity, y / min_sensitivity, drawing.nodes);
        node.linkedNode = drawing.GetLastNode();

        drawing.AddNode(node);
        playerSpelling.ProcessNode(node);

        Vector3 positionInLocal = Node.ToVector3(drawing.GetLastNode(), drawingOffset, 0.2f);
        Vector3 positionInWorld = transform.TransformPoint(positionInLocal);

        if (drawingEffect)
            drawingEffect.PV.RPC("AddVisualLine", RpcTarget.AllBuffered, positionInWorld);

        //Resets mouse movement to be more precise with the next node
        mouseX = 0;
        mouseY = 0;
    }

    private void EndDrawing()
    {
        if (drawing.isStopped || !drawingEffect) return;
        isCasting = false;
        drawing.Stop();
        drawingEffect.StopDrawing();
        drawingEffect = null;
        playerSpelling.EndSpell();
        Debug.Log("Ended canvas with " + drawing.length + " moves.");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}

[System.Serializable]
public class Drawing
{
    public List<Node> nodes = new List<Node>();
    public List<CastMove> moves = new List<CastMove>();
    public SpellElements element;

    private Node previousNode;
    public bool isStopped = false;

    public static float DELAY_BTW_NODEFADE = 0.1f;
    public static float DIST_BTW_NODES = 2.1f;

    public Drawing()
    {
        AddNode(Node.zero);
        element = SpellElements.NONE;
    }

    public void AddNode(Node node) {
        if (nodes.Count != 0)
        {
            previousNode = GetLastNode();
            node.index = previousNode.index + 1;
        }
        nodes.Add(node);
    }

    public static Node ProcessNode(float x, float y, List<Node> nodes)
    {
        Node standingNode = GetLastNode(nodes);
        Node input = Node.ToNode(x, y);
        return standingNode + input;
    }

    public void UndoNode()
    {
        if (nodes.Count == 1) return;
        nodes.RemoveAt(nodes.Count - 1);
    }
    public Node GetLastNode()
    {
        return nodes[nodes.Count - 1];
    }
    public static Node GetLastNode(List<Node> nodes)
    {
        return nodes.LastOrDefault();
    }
    public Node GetNode(int index)
    {
        if (index < 0 || index >= nodes.Count) return nodes[0];
        return nodes[index];
    }
    public int length => nodes.Count;
    public Node GetPreviousNode()
    {
        if (previousNode != null) return previousNode;
        if (nodes.Count == 1) return nodes[0];
        return nodes[nodes.Count - 2];
    }
    public Node GetPreviousNode(int index)
    {
        if (nodes.Count == 1) return nodes[0];
        return nodes[index - 1];
    }
    public void Stop()
    {
        isStopped = true;
    }
}

[System.Serializable]
public class Node
{
    public int x;
    public int y;
    public int index = 0;
    public CastMove direction;
    public Node linkedNode;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Node zero => new Node(0, 0);
    public static Node ToNode(float x, float y)
    {
        return new Node(x < 0 ? Mathf.CeilToInt(x) : Mathf.FloorToInt(x), y < 0 ? Mathf.CeilToInt(y) : Mathf.FloorToInt(y));
    }

    public static Vector3 ToVector3(Node node, Vector3 offset, float scale = 1)
    {
        return new Vector3(node.x + Drawing.DIST_BTW_NODES, node.y, 0) * scale + offset;
    }

    public static Node operator -(Node a, Node b) => new Node(a.x - b.x, a.y - b.y);
    public static Node operator +(Node a, Node b) => new Node(a.x + b.x, a.y + b.y);
}
