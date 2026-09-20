using UnityEngine;

public class bloque : MonoBehaviour
{
    public GameObject chispasPrefab;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pelota"))
        {
            int posibility = Random.Range(0, 5);

            GameManager.I.RegistrarBloqueRoto();

            if (chispasPrefab != null)
            {
                // Instancia las chispas
                GameObject chispas = Instantiate(chispasPrefab, transform.position, Quaternion.identity);
                // Destruye el objeto de partículas después de 2 segundos para no llenar la memoria
                Destroy(chispas, 2f);
            }

            Destroy(this.gameObject);
        }
    }
}
