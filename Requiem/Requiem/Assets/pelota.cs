using UnityEngine;

public class pelota : MonoBehaviour
{
    public float launchSpeed = 8f;
    public bool launched = false;
    public Transform pad;
    public Vector3 offset = new Vector3(0f, 0.8f, 0f); // Offset ajustado

    // --- Variables M2: El Limbo ---
    public float umbralLimbo = -5f;
    public GameObject ecoPrefab; // Asignaremos el prefab del Eco aquí

    [Header("Sonido")]
    public AudioClip clipRebote;
    private AudioSource audioSource;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // Añade esta línea
    }

    void Start()
    {
        if (!launched)
        {
            ResetBall();
        }
    }

    void Update()
    {
        if (!launched)
        {
            FollowPad();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }
        else
        {
            // Validar si cayó al Limbo (M2)
            if (transform.position.y < umbralLimbo)
            {
                ConvertirEnEco();
            }
        }
    }

    void FixedUpdate()
    {
        // Solo forzamos la velocidad si la bola ya fue lanzada
        if (launched && rb != null)
        {
            // 1. Obtenemos la dirección pura hacia donde se mueve (normalized la vuelve de longitud 1)
            Vector3 direccionActual = rb.linearVelocity.normalized;

            // 2. Multiplicamos esa dirección pura por nuestra velocidad estricta
            rb.linearVelocity = direccionActual * launchSpeed;
        }
    }

    public void Launch(Vector3? direccion = null)
    {
        launched = true;
        float anguloGrados = Random.Range(45f, 135f);
        float anguloRadianes = anguloGrados * Mathf.Deg2Rad;

        float dirX = Mathf.Cos(anguloRadianes);
        float dirY = Mathf.Sin(anguloRadianes);

        Vector3 dirFinal = new Vector3(dirX, dirY, 0f);
        rb.linearVelocity = dirFinal * launchSpeed;
    }

    public void ResetBall()
    {
        launched = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        FollowPad();
    }

    void FollowPad()
    {
        transform.position = pad.position + offset;
    }

    public void MultiplySpeed(float multi)
    {
        launchSpeed = launchSpeed * multi;
    }

    void ConvertirEnEco()
    {
        if (ecoPrefab != null)
        {
            Instantiate(ecoPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("¡Te falta asignar el ecoPrefab en el Inspector de la pelota!");
        }

        Destroy(gameObject); // La bola original muere
    }

    void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null && clipRebote != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.PlayOneShot(clipRebote);
        }
    }

}