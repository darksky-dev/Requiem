using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image llenadoRequiem;
    public TextMeshProUGUI textoOleada;

    public Transform contenedorRacha;
    public GameObject llamaPrefab;
    private int rachaVisualActual = 0;

    void Update()
    {
        if (GameManager.I == null) return;

        // 1. Actualizar el Réquiem
        float porcentaje = (float)GameManager.I.cargaRequiem / GameManager.I.requiemNecesario;
        llenadoRequiem.fillAmount = porcentaje;

        // 2. Actualizar la Oleada
        textoOleada.text = $"OLEADA {GameManager.I.oleadaActual} / {GameManager.I.oleadasTotales}";

        // 3. Actualizar la Racha
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