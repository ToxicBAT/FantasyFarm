
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingInteraction : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private float targetDistance = 1f;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Target Highlight")]
    [SerializeField]
    private Color targetHighlightColor =
        new Color(1f, 0.75f, 0.15f, 1f);

    [SerializeField] private float borderThickness = 0.05f;
    [SerializeField] private float borderHeight = 0.06f;
    [SerializeField] private float borderGap = 0.02f;

    private TargetCellHighlight targetHighlight;

    private void Awake()
    {
        targetHighlight = new TargetCellHighlight(
            targetHighlightColor,
            borderThickness,
            borderHeight,
            borderGap
        );
    }

    public void PerformAction()
    {
        if (playerMovement == null || worldGrid == null)
        {
            Debug.LogError(
                "Assign PlayerMovement and worldGrid in the Inspector."
            );
            return;
        }

        if (!TryGetTargetCell(out FarmCell cell, out _, out _))
            return;

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

    private bool TryGetTargetCell(
        out FarmCell cell,
        out int x,
        out int z)
    {
        cell = null;
        x = 0;
        z = 0;

        Vector3 direction = playerMovement.FacingDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        direction.Normalize();

        Vector3 targetPosition =
            transform.position + direction * targetDistance;

        if (!worldGrid.WorldToGrid(targetPosition, out x, out z))
            return false;

        cell = worldGrid.GetCell(x, z);

        return cell != null;
    }

    private void UpdateTargetHighlight()
    {
        if (playerMovement == null || worldGrid == null ||
            targetHighlight == null)
        {
            targetHighlight?.Hide();
            return;
        }

        if (TryGetTargetCell(out FarmCell cell, out int x, out int z))
        {
            targetHighlight.Show(cell, worldGrid, x, z);
        }
        else
        {
            targetHighlight.Hide();
        }
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

    private void OnDestroy()
    {
        targetHighlight?.Dispose();
    }
}
