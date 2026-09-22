using UnityEngine;

public class Marea : MonoBehaviour
{
    void Update()
    {
        if (GameManager.I != null)
        {
            float yPos = GameManager.I.mareaY + (transform.localScale.y / 2f);
            transform.position = new Vector3(0, yPos, -0.5f);
        }
    }
}