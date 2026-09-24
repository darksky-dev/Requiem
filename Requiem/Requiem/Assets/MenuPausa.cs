using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject panelPausa;
    private bool juegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f; 
        juegoPausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void RegresarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ReiniciarOleada()
    {
        Reanudar();

        GameObject[] bloques = GameObject.FindGameObjectsWithTag("Bloque");
        foreach (GameObject bloque in bloques) Destroy(bloque);

        GameObject[] ecos = GameObject.FindGameObjectsWithTag("Eco");
        foreach (GameObject eco in ecos) Destroy(eco);

        FindObjectOfType<BloqueSpawner>().GenerarOleada(GameManager.I.oleadaActual);

        // --- NUEVO: Usamos tu método existente ---
        pelota scriptPelota = FindObjectOfType<pelota>();
        if (scriptPelota != null)
        {
            // Ajusta el nombre si en tu script original dice "RestBall" o "ResetBall"
            scriptPelota.ResetBall();
        }
    }
}