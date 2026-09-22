using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;   // Singleton para acceso global

    [Header("Refs")]
    public jugador paddle;
    public GameObject bolaPrefab;

    public BloqueSpawner spawner;
    public Transform contenedorBolas;

    [Header("Estado y Marea (M4)")]
    public int racha = 0;
    public int puntos = 0;
    public int valorBloque = 100;

    public float mareaY = 6f;
    public float velocidadMarea = 0.15f;   // Descenso base por segundo
    public float empujeBloque = 0.05f;     // Lo que sube al romper bloque
    public float costoRescate = 0.4f;      // Lo que baja al rescatar
    public float lineaPaddleY = -3.5f;     // Altura a la que te mata (el paddle está en -4)

    public int cargaRequiem = 0;
    public int requiemNecesario = 3;
    public int oleadaActual = 1;
    public int oleadasTotales = 5;

    public enum Estado { Jugando, Victoria, Derrota }
    public Estado estado = Estado.Jugando;

    // ---- Eventos ----
    public event Action<int> OnRacha;
    public event Action<float> OnMarea;
    public event Action<int, bool> OnRequiem;
    public event Action<int> OnOleada;
    public event Action<Estado> OnEstado;
    public GameObject auraRequiem;
    public UnityEngine.UI.Image destelloPantalla;

    public AlertaMarea alertaMareaUI;

    void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Generar la primera oleada al iniciar
        if (spawner != null) spawner.GenerarOleada(oleadaActual);
    }

    void Update()
    {
        if (estado != Estado.Jugando)
        {
            // --- REINICIAR PARTIDA AL GANAR/PERDER (M6) ---
            if (Input.GetKeyDown(KeyCode.Return)) // Tecla Enter
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        // --- LÓGICA DE LA MAREA (M4) ---
        mareaY -= velocidadMarea * Time.deltaTime;
        if (alertaMareaUI != null)
        {
            alertaMareaUI.mareaCritica = (mareaY <= -2f);
        }
        OnMarea?.Invoke(mareaY);

        if (mareaY <= lineaPaddleY)
        {
            Derrota();
        }

        // --- DISPARAR RÉQUIEM (M5) ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            DetonarRequiem();
        }
    }

    // ---- API de Réquiem ----
    public void RegistrarBloqueRoto()
    {
        racha++;
        puntos += valorBloque * racha;

        // La Racha amplifica cuánto sube la Marea al romper bloques[cite: 3]
        mareaY += empujeBloque * racha;

        OnRacha?.Invoke(racha);
        OnMarea?.Invoke(mareaY);

        if (spawner != null && spawner.BloquesVivos() <= 1)
        {
            AvanzarOleada();
        }

    }

    public void RegistrarRescate()
    {
        racha = 0;

        // Castigo: la marea baja de golpe[cite: 3]
        mareaY -= costoRescate;

        OnRacha?.Invoke(racha);
        OnMarea?.Invoke(mareaY);
    }

    public void RegistrarOfrenda()
    {
        racha = 0;
        cargaRequiem++;
        bool listo = cargaRequiem >= requiemNecesario;

        OnRacha?.Invoke(racha);
        OnRequiem?.Invoke(cargaRequiem, listo);
        Debug.Log($"Eco ofrendado. Carga Réquiem: {cargaRequiem}/{requiemNecesario}");
        if (listo)
        {
            auraRequiem.SetActive(true);
        }
    }

    public void DetonarRequiem()
    {
        // Solo funciona si la carga está llena (por defecto 3 ofrendas)
        if (cargaRequiem >= requiemNecesario)
        {
            auraRequiem.SetActive(false);
            AlertaMarea alerta = FindObjectOfType<AlertaMarea>();
            if (alerta != null) alerta.mareaCritica = false;

            // Disparamos una rutina para hacer el "fogonazo" blanco
            StartCoroutine(FogonazoRequiem());
            Debug.Log("¡RÉQUIEM DETONADO! La Marea retrocede violentamente.");

            // Efecto: Empuja la marea fuertemente hacia arriba (ej. +3 unidades)
            mareaY += 3f;

            // Vacía el medidor
            cargaRequiem = 0;

            OnMarea?.Invoke(mareaY);
            OnRequiem?.Invoke(cargaRequiem, false);
        }
        else
        {
            Debug.Log($"Aún no puedes detonar. Carga actual: {cargaRequiem}/{requiemNecesario}");
        }
    }

    public void AvanzarOleada()
    {
        oleadaActual++;
        if (oleadaActual > oleadasTotales)
        {
            estado = Estado.Victoria;
            OnEstado?.Invoke(estado);
            Debug.Log("¡VICTORIA! Sobreviviste a las 5 oleadas.");
            Time.timeScale = 0; // Congela el juego
        }
        else
        {
            Debug.Log($"Avanzando a la oleada {oleadaActual}");
            velocidadMarea += 0.05f; // Sube la dificultad de la marea
            spawner.GenerarOleada(oleadaActual);
            OnOleada?.Invoke(oleadaActual);
        }
    }

    public void Derrota()
    {
        estado = Estado.Derrota;
        OnEstado?.Invoke(estado);
        Debug.Log("¡LA MAREA TE ALCANZÓ! GAME OVER.");
        Time.timeScale = 0; // Congela el juego al perder[cite: 3]
    }

    // ---- Fabricación de bolas ----
    public void SpawnBolaPegada()
    {
        GameObject nuevaBola = Instantiate(bolaPrefab, paddle.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        // Nos aseguramos de que asigne el paddle
        nuevaBola.GetComponent<pelota>().pad = paddle.transform;
    }

    public void SpawnBolaLanzada(Vector3 pos)
    {
        GameObject nuevaBola = Instantiate(bolaPrefab, pos, Quaternion.identity);
        nuevaBola.GetComponent<pelota>().pad = paddle.transform;
        nuevaBola.GetComponent<pelota>().Launch();
    }

    public int BolasVivas()
    {
        // Sumamos pelotas normales y ecos
        int pelotas = FindObjectsByType<pelota>(FindObjectsSortMode.None).Length;
        int ecos = FindObjectsByType<Eco>(FindObjectsSortMode.None).Length;
        return pelotas + ecos;
    }

    private System.Collections.IEnumerator FogonazoRequiem()
    {
        // 1. Pantalla blanca al instante
        Color colorDestello = destelloPantalla.color;
        colorDestello.a = 1f;
        destelloPantalla.color = colorDestello;

        // 2. Se desvanece en 0.5 segundos
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            colorDestello.a = t;
            destelloPantalla.color = colorDestello;
            yield return null;
        }
    }
}