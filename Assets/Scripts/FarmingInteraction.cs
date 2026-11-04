
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingInteraction : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private float targetDistance = 1f;
    [SerializeField] private PlayerInventory playerInventory;

    private GameObject[] targetBorders;
    private Material targetHighlightMaterial;

    [SerializeField]
    private Color targetHighlightColor =
        new Color(1f, 0.75f, 0.15f, 1f);

    [SerializeField] private float borderThickness = 0.025f;
    [SerializeField] private float borderHeight = 0.025f;
    [SerializeField] private float borderGap = 0.005f;
    [SerializeField] private const int overlayRenderQueue = 4000;

    public void PerformAction()
    {
        if (playerMovement == null || worldGrid == null)
        {
            Debug.LogError(
                "Assign PlayerMovement and worldGrid in the Inspector."
            );
            return;
        }

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            Debug.LogWarning("Move first to choose a facing direction.");
            return;
        }

        direction.Normalize();

        Vector3 targetPosition =
            transform.position + direction * targetDistance;

        if (worldGrid.WorldToGrid(targetPosition, out int x, out int z))
        {
            FarmCell cell = worldGrid.GetCell(x, z);

            if (cell != null)
            {
                if (!cell.IsTilled)
                {
                    cell.Till();
                    Debug.Log("Soil tilled.");
                }
                else if (!cell.HasCrop)
                {
                    cell.Plant();
                    Debug.Log("Seed planted.");
                }
                else if (cell.GrowthStage >= 2)
                {
                    if (cell.Harvest())
                    {
                        if (playerInventory != null)
                        {
                            playerInventory.AddCrops(1);
                        }
                        else
                        {
                            Debug.LogError(
                                "PlayerInventory is not assigned in the Inspector."
                            );
                        }

                        Debug.Log("Harvest successful!");
                    }
                }
                else
                {
                    Debug.Log("Crop is still growing.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Target position is outside the farm grid.");
        }
    }

    private void Awake()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");

        if (shader == null)
        {
            Debug.LogError("URP Unlit shader not found.");
            return;
        }

        targetHighlightMaterial = new Material(shader);
        targetHighlightMaterial.SetColor(
            "_BaseColor", targetHighlightColor);

        targetBorders = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            GameObject border = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            border.name = "TargetBorder_" + i;

            // Remove the unnecessary physics collider.
            Collider borderCollider = border.GetComponent<Collider>();

            if (borderCollider != null)
                Destroy(borderCollider);

            Renderer borderRenderer = border.GetComponent<Renderer>();

            borderRenderer.material = targetHighlightMaterial;
            borderRenderer.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            borderRenderer.receiveShadows = false;

            border.SetActive(false);

            targetBorders[i] = border;
        }
    }

    private void UpdateTargetHighlight()
    {
        if (playerMovement == null || worldGrid == null ||
            targetBorders == null)
        {
            HideTargetHighlight();
            return;
        }

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            HideTargetHighlight();
            return;
        }

        direction.Normalize();

        Vector3 targetPosition =
            transform.position + direction * targetDistance;

        if (!worldGrid.WorldToGrid(targetPosition, out int x, out int z))
        {
            HideTargetHighlight();
            return;
        }

        FarmCell targetCell = worldGrid.GetCell(x, z);

        if (targetCell == null)
        {
            HideTargetHighlight();
            return;
        }

        Renderer cellRenderer =
            targetCell.GetComponentInChildren<Renderer>();

        if (cellRenderer == null)
        {
            HideTargetHighlight();
            return;
        }

        float half = worldGrid.CellSize * 0.5f;

        float y = cellRenderer.bounds.max.y
            + borderGap + borderHeight * 0.5f;

        Vector3 center = worldGrid.GridToWorld(x, z);
        center.y = y;

        // North edge (+Z)
        PositionBorder(0,
            center + new Vector3(0f, 0f, half),
            new Vector3(worldGrid.CellSize, borderHeight, borderThickness));

        // South edge (-Z)
        PositionBorder(1,
            center + new Vector3(0f, 0f, -half),
            new Vector3(worldGrid.CellSize, borderHeight, borderThickness));

        // East edge (+X)
        PositionBorder(2,
            center + new Vector3(half, 0f, 0f),
            new Vector3(borderThickness, borderHeight, worldGrid.CellSize));

        // West edge (-X)
        PositionBorder(3,
            center + new Vector3(-half, 0f, 0f),
            new Vector3(borderThickness, borderHeight, worldGrid.CellSize));
    }

    private void PositionBorder(
        int index,
        Vector3 position,
        Vector3 scale)
    {
        GameObject border = targetBorders[index];

        border.transform.SetPositionAndRotation(
            position,
            Quaternion.identity
        );

        border.transform.localScale = scale;

        if (!border.activeSelf)
            border.SetActive(true);
    }

    private void HideTargetHighlight()
    {
        if (targetBorders == null)
            return;

        foreach (GameObject border in targetBorders)
        {
            if (border != null)
                border.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (targetBorders != null)
        {
            foreach (GameObject border in targetBorders)
            {
                if (border != null)
                    Destroy(border);
            }
        }

        if (targetHighlightMaterial != null)
            Destroy(targetHighlightMaterial);
    }

    private void Update()
    {
        UpdateTargetHighlight();

#if UNITY_EDITOR
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PerformAction();
        }
#endif
    }
}
