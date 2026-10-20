
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [SerializeField] private GameObject farmCellPrefab;
    [SerializeField] private int width = 16;
    [SerializeField] private int depth = 16;
    [SerializeField] private float cellSize = 1f;

    private FarmCell[,] cells;
    private GridCellData[,] cellData;

    public int Width => width;
    public int Depth => depth;
    public float CellSize => cellSize;

    private void Start()
    {
        if (farmCellPrefab == null)
        {
            Debug.LogError("WorldGrid: Farm cell prefab is not assigned.", this);
            return;
        }

        if (width <= 0 || depth <= 0 || cellSize <= 0f)
        {
            Debug.LogError("WorldGrid: Dimensions and cell size must be positive.", this);
            return;
        }

        cells = new FarmCell[width, depth];
        cellData = new GridCellData[width, depth];

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

                FarmCell farmCell = instance.GetComponent<FarmCell>();

                if (farmCell == null)
                {
                    Debug.LogError(
                        $"WorldGrid: FarmCell missing on prefab at ({x}, {z}).",
                        instance
                    );

                    Destroy(instance);
                    continue;
                }

                cells[x, z] = farmCell;
                cellData[x, z] = new GridCellData(x, z);
            }
        }
    }

    public FarmCell GetCell(int x, int z)
    {
        if (!IsInsideBounds(x, z) || cells == null)
            return null;

        return cells[x, z];
    }

    public GridCellData GetCellData(int x, int z)
    {
        if (!IsInsideBounds(x, z) || cellData == null)
            return null;

        return cellData[x, z];
    }

    public bool IsInsideBounds(int x, int z)
    {
        return x >= 0 && x < width && z >= 0 && z < depth;
    }

    public bool WorldToGrid(Vector3 worldPosition, out int x, out int z)
    {
        Vector3 local = worldPosition - transform.position;

        x = Mathf.RoundToInt(local.x / cellSize);
        z = Mathf.RoundToInt(local.z / cellSize);

        return IsInsideBounds(x, z);
    }

    public Vector3 GridToWorld(int x, int z)
    {
        if (!IsInsideBounds(x, z))
        {
            Debug.LogWarning(
                $"WorldGrid: Coordinates ({x}, {z}) are outside the grid.",
                this
            );
        }

        return transform.position
            + new Vector3(x * cellSize, 0f, z * cellSize);
    }
}
