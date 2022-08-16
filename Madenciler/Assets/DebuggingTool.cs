using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebuggingTool : MonoBehaviour
{
    public static DebuggingTool Instance { get; private set; }
    public KeyCode activationKey = KeyCode.F1;
    public GameObject canvas;
    public bool debugging;

    [Header("Constant information display")]
    public TextMeshProUGUI fpsCounter;
    private float fps;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
            debugging = !debugging;
        canvas.SetActive(debugging);

        if (!debugging) return;

        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = $"FPS: {fps}";
    }

    public static void Debug(string text, string target)
    {
        if (!Instance.debugging) return;
        TextMeshProUGUI t = GameObject.Find(target).GetComponent<TextMeshProUGUI>();
        if (!t) return;
        t.text = text;
    }
}
