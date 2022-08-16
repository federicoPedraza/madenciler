using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellDisplayController : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float timeBetweenFades = 2.25f;
    private float _timeBFTimer = 2.25f;
    public bool isCasting;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        _timeBFTimer = timeBetweenFades;
        textMesh.text = "";
    }

    private void Update()
    {
        if (isCasting)
        {
            _timeBFTimer = timeBetweenFades;
            return;
        }
        _timeBFTimer -= Time.deltaTime;
        if (_timeBFTimer < 0)
            StartCoroutine("FadeOut");
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        textMesh.text = "";
        yield break;
    }

    public void ClearText()
    {
        isCasting = false;
        _timeBFTimer = timeBetweenFades;
        textMesh.color = Color.white;
    }

    public void UpdateText(string str, string colorhex)
    {
        if (!isCasting)
            textMesh.text = "";

        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);
        textMesh.text += $"<color={colorhex}>{str}";
        _timeBFTimer = timeBetweenFades;
        isCasting = true;
    }
}
