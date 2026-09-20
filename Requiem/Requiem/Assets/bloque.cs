using UnityEngine;

public class bloque : MonoBehaviour
{
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        rend.material.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
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

            // Llamamos a la API del nuevo GameManager de Réquiem (M3)[cite: 3]
            GameManager.I.RegistrarBloqueRoto();

            Destroy(this.gameObject);
        }
    }
}
