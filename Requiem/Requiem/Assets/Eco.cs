using UnityEngine;

public class Eco : MonoBehaviour
{
    public float tiempoVida = 3f;       // Ventana para decidir rescate/ofrenda[cite: 3]
    public float velocidadCaida = 1.5f;
    public float fondoLimbo = -8f;
    public GameObject vfxRescate;
    public GameObject vfxOfrenda;

    float t;

    // --- EL CERROJO ANTI-DUPLICADOS ---
    bool yaResuelto = false;

    void Start()
    {
        t = tiempoVida;
    }

    void Update()
    {
        if (yaResuelto) return;

        transform.position += Vector3.down * velocidadCaida * Time.deltaTime;
        t -= Time.deltaTime;

        // --- EL ARTE DEL ECO: Se consume gradualmente ---
        // Calcula qué porcentaje de vida le queda (de 1.0 a 0.0)
        float porcentajeVida = t / tiempoVida;
        // Escala el objeto basándose en ese porcentaje (se hará diminuto antes de morir)
        transform.localScale = Vector3.one * porcentajeVida;

        if (t <= 0f || transform.position.y < fondoLimbo)
        {
            Ofrendar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si ya chocó con un collider, ignoramos los demás choques simultáneos
        if (yaResuelto) return;

        if (other.GetComponent<jugador>() != null)
        {
            Rescatar();
        }
    }

    void Rescatar()
    {
        // Cerramos el cerrojo inmediatamente
        yaResuelto = true;

        GameManager.I.RegistrarRescate();

        // Calculamos una posición segura encima del paddle para que nazca fuera del Limbo
        Vector3 posSegura = GameManager.I.paddle.transform.position + new Vector3(0, 0.8f, 0);

        GameManager.I.SpawnBolaLanzada(posSegura);

        Debug.LogWarning("Eco destruido");

        if (vfxRescate != null) Destroy(Instantiate(vfxRescate, transform.position, Quaternion.identity), 2f);

        Destroy(gameObject);
    }

    void Ofrendar()
    {
        // Cerramos el cerrojo inmediatamente
        yaResuelto = true;

        GameManager.I.RegistrarOfrenda();

        if (GameManager.I.BolasVivas() <= 1)
        {
            GameManager.I.SpawnBolaPegada();
        }

        if (vfxOfrenda != null) Destroy(Instantiate(vfxOfrenda, transform.position, Quaternion.identity), 2f);

        Destroy(gameObject);
    }
}