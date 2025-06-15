using UnityEngine;
public class TesteUnitare : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spawnerPrefab;
    public GameObject conveyorPrefab;
    public GameObject cutterPrefab;
    public GameObject rotatorPrefab;
    public GameObject endpointPrefab;

    // Binary masks representing cutter configurations
    private readonly int[] _cutters = { 448, 56, 7, 292, 146, 73, 273, 84 };

    private const int NumberOfInstances = 15; 
    private const float StepSize = 2f;          
    private const int MatrixSize = 3;           

    private void Start()
    {
        for (int i = 0; i < NumberOfInstances; i++)
        {
            // Initial spawn position for each chain (staggered vertically)
            Vector3 spawnPosition = new Vector3(0.5f, i * StepSize + 0.5f, -1f);
            
            // Spawn spawner
            Instantiate(spawnerPrefab, spawnPosition, Quaternion.identity);

            // Place 5 conveyor belts to the right
            spawnPosition = SpawnConveyors(spawnPosition, 5);

            // Spawn cutter and apply cutter mask
            GameObject cutter = Instantiate(cutterPrefab, spawnPosition + Vector3.right, Quaternion.identity);
            ApplyCutterMatrix(cutter, _cutters[i % _cutters.Length]);
            spawnPosition += Vector3.right;

            // For the last 7 chains, also place a rotator after 2 more conveyors
            if (i >= 8)
            {
                spawnPosition = SpawnConveyors(spawnPosition, 2);
                spawnPosition += Vector3.right;
                Instantiate(rotatorPrefab, spawnPosition, Quaternion.identity);
            }

            // Place 2 more conveyors
            spawnPosition = SpawnConveyors(spawnPosition, 2);
            spawnPosition += Vector3.right;

            // Spawn endpoint and apply endpoint mask (optionally rotated)
            GameObject endpoint = Instantiate(endpointPrefab, spawnPosition, Quaternion.identity);
            ApplyEndpointMatrix(endpoint, _cutters[i % _cutters.Length], i > 7);
        }
    }
 
    private Vector3 SpawnConveyors(Vector3 startPos, int count)
    {
        for (int j = 0; j < count; j++)
        {
            startPos += Vector3.right;
            Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);  // Rotate to face forward
            Instantiate(conveyorPrefab, startPos, rotation);
        }
        return startPos;
    }

    private void ApplyCutterMatrix(GameObject cutter, int mask)
    {
        bool[,] matrix = ConvertMaskToMatrix(mask);
        Cutter cutterScript = cutter.GetComponent<Cutter>();
        if (cutterScript != null)
        {
            cutterScript.taieri = matrix;
        }
    }
    
    // Applies a (possibly rotated) 3x3 matrix to an endpoint based on the given mask.
    private void ApplyEndpointMatrix(GameObject endpoint, int mask, bool isRotated)
    {
        bool[,] original = ConvertMaskToMatrix(mask);
        bool[,] matrixToUse = isRotated ? RotateMatrix(original) : original;

        EndpointTest endpointScript = endpoint.GetComponent<EndpointTest>();
        if (endpointScript == null) return;

        for (int row = 0; row < MatrixSize; row++)
        {
            for (int col = 0; col < MatrixSize; col++)
            {
                endpointScript.matrix[row].values[col] = !matrixToUse[row, col];
            }
        }
    }
    
    // Converts a binary int mask into a 3x3 matrix (right-to-left, bottom-to-top).
    private bool[,] ConvertMaskToMatrix(int mask)
    {
        bool[,] matrix = new bool[MatrixSize, MatrixSize];
        for (int row = MatrixSize - 1; row >= 0; row--)
        {
            for (int col = MatrixSize - 1; col >= 0; col--)
            {
                matrix[row, col] = (mask & 1) == 1;
                mask >>= 1;
            }
        }
        return matrix;
    }
    
    // Rotates a 3x3 matrix 90 degrees clockwise.
    private bool[,] RotateMatrix(bool[,] original)
    {
        bool[,] rotated = new bool[MatrixSize, MatrixSize];
        for (int row = 0; row < MatrixSize; row++)
        {
            for (int col = 0; col < MatrixSize; col++)
            {
                rotated[col, MatrixSize - 1 - row] = original[row, col];
            }
        }
        return rotated;
    }
}
