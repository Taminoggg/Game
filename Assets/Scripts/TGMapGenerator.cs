using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] mapParts; // Array of map prefabs
    public int numberOfParts = 10; // How many parts to generate

    private Vector3 nextPosition = Vector3.zero; // Keeps track of where to place the next part

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int i = 0; i < numberOfParts; i++)
        {
            GameObject selectedPart = mapParts[Random.Range(0, mapParts.Length)];

            GameObject partInstance = Instantiate(selectedPart, nextPosition, Quaternion.identity);

            Transform exitPoint = partInstance.transform.Find("ExitPoint");

            if (exitPoint != null)
            {
                nextPosition = exitPoint.position;
            }
            else
            {
                Debug.LogWarning("No ExitPoint found on " + selectedPart.name);
            }
        }
    }
}
