
using UnityEngine;

public class TargetCellHighlight
{
    private readonly GameObject[] targetBorders;
    private readonly Material targetHighlightMaterial;

    private readonly float borderThickness;
    private readonly float borderHeight;
    private readonly float borderGap;

    private const int OverlayRenderQueue = 4000;

    public TargetCellHighlight(
        Color highlightColor,
        float thickness,
        float height,
        float gap)
    {
        borderThickness = thickness;
        borderHeight = height;
        borderGap = gap;

        Shader shader = Shader.Find(
            "Universal Render Pipeline/Unlit");

        if (shader == null)
        {
            Debug.LogError("URP Unlit shader not found.");
            return;
        }

        targetHighlightMaterial = new Material(shader);

        targetHighlightMaterial.SetColor(
            "_BaseColor", highlightColor);

        targetHighlightMaterial.renderQueue = OverlayRenderQueue;

        targetBorders = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            GameObject border = GameObject.CreatePrimitive(
                PrimitiveType.Cube);

            border.name = "TargetBorder_" + i;

            Collider borderCollider = border.GetComponent<Collider>();

            if (borderCollider != null)
            {
                Object.Destroy(borderCollider);
            }

            Renderer borderRenderer =
                border.GetComponent<Renderer>();

            borderRenderer.sharedMaterial =
                targetHighlightMaterial;

            borderRenderer.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;

            borderRenderer.receiveShadows = false;

            border.SetActive(false);

            targetBorders[i] = border;
        }
    }

    public void Show(
    FarmCell targetCell,
    WorldGrid worldGrid,
    int x,
    int z)
    {
        if (targetBorders == null ||
            targetCell == null ||
            worldGrid == null)
        {
            Hide();
            return;
        }

        Renderer cellRenderer = targetCell.CellRenderer;

        if (cellRenderer == null)
        {
            Hide();
            return;
        }

        float half = worldGrid.CellSize * 0.5f;

        float y = cellRenderer.bounds.max.y
            + borderGap + borderHeight * 0.5f;

        Vector3 center = worldGrid.GridToWorld(x, z);

        center.y = y;

        // North edge (+Z)
        PositionBorder(
            0,
            center + new Vector3(0f, 0f, half),
            new Vector3(
                worldGrid.CellSize,
                borderHeight,
                borderThickness));

        // South edge (-Z)
        PositionBorder(
            1,
            center + new Vector3(0f, 0f, -half),
            new Vector3(
                worldGrid.CellSize,
                borderHeight,
                borderThickness));

        float sideLength =
            worldGrid.CellSize - 2f * borderThickness;

        // East edge (+X)
        PositionBorder(
            2,
            center + new Vector3(half, 0f, 0f),
            new Vector3(
                borderThickness,
                borderHeight,
                sideLength));

        // West edge (-X)
        PositionBorder(
            3,
            center + new Vector3(-half, 0f, 0f),
            new Vector3(
                borderThickness,
                borderHeight,
                sideLength));
    }

    private void PositionBorder(
        int index,
        Vector3 position,
        Vector3 scale)
    {
        GameObject border = targetBorders[index];

        border.transform.SetPositionAndRotation(
            position,
            Quaternion.identity);

        border.transform.localScale = scale;

        if (!border.activeSelf)
            border.SetActive(true);
    }

    public void Hide()
    {
        if (targetBorders == null)
            return;

        foreach (GameObject border in targetBorders)
        {
            if (border != null)
                border.SetActive(false);
        }
    }

    public void Dispose()
    {
        if (targetBorders != null)
        {
            foreach (GameObject border in targetBorders)
            {
                if (border != null)
                    Object.Destroy(border);
            }
        }

        if (targetHighlightMaterial != null)
            Object.Destroy(targetHighlightMaterial);
    }
}
