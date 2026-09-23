using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void IniciarJuego()
    {
        Time.timeScale = 1; // Por si venimos de un Game Over
        SceneManager.LoadScene("NOMBREDETUESCENA"); // Pon el nombre exacto de tu escena de juego
    }
}