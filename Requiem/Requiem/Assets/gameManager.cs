using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;   // Singleton para acceso global

    [Header("Refs")]
    public jugador paddle;
    public GameObject bolaPrefab;
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

    void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (estado != Estado.Jugando) return;

        // --- LÓGICA DE LA MAREA (M4) ---
        mareaY -= velocidadMarea * Time.deltaTime; // Baja constantemente
        OnMarea?.Invoke(mareaY);

        if (mareaY <= lineaPaddleY) // Condición de derrota
        {
            Derrota();
        }

        // Teclas de prueba temporales para validar M1
        if (Input.GetKeyDown(KeyCode.Alpha1)) RegistrarBloqueRoto();
        if (Input.GetKeyDown(KeyCode.Alpha2)) RegistrarRescate();
        if (Input.GetKeyDown(KeyCode.Alpha3)) RegistrarOfrenda();
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
    }

    public void DetonarRequiem()
    {
        if (cargaRequiem >= requiemNecesario)
        {
            // TODO en M5: efecto grande de Réquiem
            Debug.Log("¡RÉQUIEM DETONADO!");
            cargaRequiem = 0;
            OnRequiem?.Invoke(cargaRequiem, false);
        }
    }

    public void AvanzarOleada()
    {
        oleadaActual++;
        if (oleadaActual > oleadasTotales)
        {
            estado = Estado.Victoria;
            OnEstado?.Invoke(estado);
        }
        else
        {
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
}