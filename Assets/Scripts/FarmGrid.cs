
using UnityEngine;

public class FarmGrid : MonoBehaviour
{
    [SerializeField] private GameObject farmCellPrefab;
    [SerializeField] private int width = 8;
    [SerializeField] private int depth = 8;
    [SerializeField] private float cellSize = 1.05f;

    private FarmCell[,] cells;

    private void Start()
    {
        if (farmCellPrefab == null)
        {
            Debug.LogError("Farm cell prefab is not assigned.");
            return;
        }

        cells = new FarmCell[width, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 position = transform.position
                    + new Vector3(x * cellSize, 0f, z * cellSize);

                GameObject instance = Instantiate(
                    farmCellPrefab,
                    position,
                    Quaternion.identity,
                    transform
                );

                cells[x, z] = instance.GetComponent<FarmCell>();

                if (cells[x, z] == null)
                {
                    Debug.LogError(
                        "FarmCell script missing from the prefab."
                    );
                }
            }
        }
    }

    public FarmCell GetCell(int x, int z)
    {
        if (cells == null)
            return null;

        if (x < 0 || x >= width || z < 0 || z >= depth)
            return null;

        return cells[x, z];
    }

    public bool WorldToGrid(Vector3 worldPosition, out int x, out int z)
    {
        Vector3 local = worldPosition - transform.position;

        x = Mathf.RoundToInt(local.x / cellSize);
        z = Mathf.RoundToInt(local.z / cellSize);

        return x >= 0 && x < width && z >= 0 && z < depth;
    }
}
