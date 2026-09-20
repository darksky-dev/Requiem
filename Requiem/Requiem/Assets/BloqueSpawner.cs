using UnityEngine;

public class BloqueSpawner : MonoBehaviour
{
    public GameObject bloquePrefab;
    public int filasBase = 3;
    public int columnas = 8;
    public float espacioX = 2f;
    public float espacioY = 1f;

    // Coordenada donde empezará a dibujarse la esquina superior izquierda de los bloques
    public Vector3 origen = new Vector3(-7f, 4.5f, 0f);

    public void GenerarOleada(int numeroOleada)
    {
        // Añade una fila extra por cada oleada para aumentar la dificultad
        int filasActuales = filasBase + (numeroOleada - 1);

        for (int i = 0; i < filasActuales; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Vector3 pos = origen + new Vector3(j * espacioX, -i * espacioY, 0f);

                // Instancia el bloque como "hijo" de este spawner
                Instantiate(bloquePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    public int BloquesVivos()
    {
        // Cuenta cuántos hijos (bloques) le quedan
        return transform.childCount;
    }
}