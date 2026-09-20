using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    public float intensidadMinima = 1f;
    public float intensidadMaxima = 4f;
    public float velocidad = 2f;

    private Light luz;
    private float ruidoOffset;

    void Start()
    {
        luz = GetComponent<Light>();
        // Un valor aleatorio para que cada vela parpadee a su propio ritmo
        ruidoOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // PerlinNoise crea transiciones suaves entre 0 y 1
        float ruido = Mathf.PerlinNoise(Time.time * velocidad, ruidoOffset);
        luz.intensity = Mathf.Lerp(intensidadMinima, intensidadMaxima, ruido);
    }
}