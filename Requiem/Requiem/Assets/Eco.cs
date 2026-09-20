using UnityEngine;

public class Eco : MonoBehaviour
{
    public float tiempoVida = 3f;       // Ventana para decidir rescate/ofrenda[cite: 3]
    public float velocidadCaida = 1.5f;
    public float fondoLimbo = -8f;

    float t;

    // --- EL CERROJO ANTI-DUPLICADOS ---
    bool yaResuelto = false;

    void Start()
    {
        t = tiempoVida;
    }

    void Update()
    {
        // Si el Eco ya fue rescatado u ofrendado en este frame, ignoramos el Update
        if (yaResuelto) return;

        transform.position += Vector3.down * velocidadCaida * Time.deltaTime;
        t -= Time.deltaTime;

        // Si el tiempo se acaba o llega al fondo del Limbo[cite: 3]
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

        Destroy(gameObject);
    }
}