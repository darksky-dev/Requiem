using UnityEngine;
using UnityEngine.UI;

public class AlertaMarea : MonoBehaviour
{
    public Image imagenVelo;
    public float velocidadLatido = 5f;
    public float opacidadMaxima = 0.5f;
    public float velocidadDesvanecimiento = 3f;

    public bool mareaCritica = false;
    private float alphaActual = 0f;

    // --- NUEVO: Referencia al sonido ---
    private AudioSource latidoAudio;

    void Start()
    {
        // Atrapa el AudioSource que le acabamos de poner al objeto
        latidoAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (imagenVelo == null) return;

        if (mareaCritica)
        {
            float alphaObjetivo = (Mathf.Sin(Time.time * velocidadLatido) + 1f) / 2f * opacidadMaxima;
            alphaActual = Mathf.Lerp(alphaActual, alphaObjetivo, Time.deltaTime * 5f);

            // Si hay peligro y el audio NO está sonando, lo enciende
            if (latidoAudio != null && !latidoAudio.isPlaying)
            {
                latidoAudio.Play();
                Debug.Log("Audio latido");
            }
        }
        else
        {
            alphaActual = Mathf.Lerp(alphaActual, 0f, Time.deltaTime * velocidadDesvanecimiento);

            // Si ya no hay peligro y el audio SÍ está sonando, lo detiene
            if (latidoAudio != null && latidoAudio.isPlaying)
            {
                latidoAudio.Stop();
            }
        }

        Color c = imagenVelo.color;
        c.a = alphaActual;
        imagenVelo.color = c;
    }
}