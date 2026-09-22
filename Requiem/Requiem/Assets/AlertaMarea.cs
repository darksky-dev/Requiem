using UnityEngine;
using UnityEngine.UI;

public class AlertaMarea : MonoBehaviour
{
    public Image imagenVelo;
    public float velocidadLatido = 5f;
    public float opacidadMaxima = 0.5f;
    public float velocidadDesvanecimiento = 3f; // Qué tan rápido se apaga al estar a salvo

    public bool mareaCritica = false;
    private float alphaActual = 0f;

    void Update()
    {
        if (imagenVelo == null) return;

        if (mareaCritica)
        {
            // Calcula el latido actual
            float alphaObjetivo = (Mathf.Sin(Time.time * velocidadLatido) + 1f) / 2f * opacidadMaxima;
            // Interpola suavemente hacia el latido
            alphaActual = Mathf.Lerp(alphaActual, alphaObjetivo, Time.deltaTime * 5f);
        }
        else
        {
            // Interpola suavemente hacia la transparencia total (0)
            alphaActual = Mathf.Lerp(alphaActual, 0f, Time.deltaTime * velocidadDesvanecimiento);
        }

        Color c = imagenVelo.color;
        c.a = alphaActual;
        imagenVelo.color = c;
    }
}