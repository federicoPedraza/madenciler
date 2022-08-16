using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class DrawingEffect : MonoBehaviour, IPunObservable
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    public LineRenderer lineRenderer;
    public float destroyDelay;
    public bool isStopped = false;
    private bool hasBeenStopped;
    private Color particleColor;

    [HideInInspector] public PhotonView PV;

    private int nodeCount = 0;

    private void Awake()
    {
        isStopped = false;
        PV = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (!isStopped || hasBeenStopped) return;
        StartCoroutine("FadeLineOut");
    }

    [PunRPC]
    public void AddVisualLine(Vector3 position)
    {
        //Add particle every 3rd line
        if (nodeCount % 3 == 0)
        {
            GameObject node = PhotonNetwork.Instantiate("Core", position, Quaternion.identity);
            ParticleSystem particle = node.GetComponent<ParticleSystem>();
            particles.Add(particle);
            PaintParticle(particle, particleColor);
            node.transform.SetParent(transform);
        } 
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(nodeCount++, position);
    }

    [PunRPC]
    public void Paint(float[] colorRGB)
    {
        Color color = new Color(colorRGB[0], colorRGB[1], colorRGB[2]);
        particleColor = color;
        PaintLocally(color);
    }

    public void PaintLocally(Color color)
    {
        PaintParticle(particles.LastOrDefault(), color);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey (color, 0f), new GradientColorKey (color, 1f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0f), new GradientAlphaKey(1, 1f) }
        );

        lineRenderer.colorGradient = gradient;
    }

    public void PaintParticle(ParticleSystem particle, Color color)
    {
        if (!particle) return;
        particle.Stop();
        ParticleSystem.MainModule main = particle.main;
        main.startColor = color;
        particle.Play();
    }

    public IEnumerator FadeLineOut()
    {
        hasBeenStopped = true;

        float duration = destroyDelay;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alphaStart = Mathf.Lerp(1f, 0f, currentTime / (duration / 2));
            lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, alphaStart);

            float alphaEnd = Mathf.Lerp(1f, 0f, currentTime / duration);
            lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, alphaEnd);
            currentTime += Time.deltaTime;
            yield return null;
        }
        PhotonNetwork.Destroy(PV);
    }

    public void StopDrawing()
    {
        Debug.Log("Stopped drawing");
        isStopped = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
