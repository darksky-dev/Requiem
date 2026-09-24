using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image llenadoRequiem;
    public Transform contenedorRacha;
    public GameObject llamaPrefab;
    private int rachaVisualActual = 0;
    public TextMeshProUGUI textoOleada;
    public TextMeshProUGUI textoPuntaje; 

    void Update()
    {
        if (GameManager.I == null) return;

        // 1. Actualizar el Réquiem
        float porcentaje = (float)GameManager.I.cargaRequiem / GameManager.I.requiemNecesario;
        llenadoRequiem.fillAmount = porcentaje;

        // 2. Actualizar la Oleada
        textoOleada.text = $"OLEADA {GameManager.I.oleadaActual} / {GameManager.I.oleadasTotales}";

        textoOleada.text = $"OLEADA {GameManager.I.oleadaActual} / {GameManager.I.oleadasTotales}";

        if (textoPuntaje != null)
        {
            textoPuntaje.text = $"PUNTOS: {GameManager.I.puntos.ToString("D4")}";
        }

        if (GameManager.I.racha != rachaVisualActual)
        {
            ActualizarLlamas(GameManager.I.racha);
        }
    }

    void ActualizarLlamas(int nuevaRacha)
    {
        // CORRECCIÓN: "in" en lugar de "en"
        foreach (Transform hijo in contenedorRacha)
        {
            Destroy(hijo.gameObject);
        }

        for (int i = 0; i < nuevaRacha; i++)
        {
            Instantiate(llamaPrefab, contenedorRacha);
        }

        rachaVisualActual = nuevaRacha;
    }
}