using UnityEngine;

public class Marea : MonoBehaviour
{
    void Update()
    {
        if (GameManager.I != null)
        {
            float yPos = GameManager.I.mareaY + (transform.localScale.y / 2f);
            transform.position = new Vector3(0, yPos, -0.5f);

            // Esta línea nos dirá si el script realmente está intentando moverlo
            Debug.Log($"Moviendo Marea visualmente a Y: {yPos}");
        }
    }
}