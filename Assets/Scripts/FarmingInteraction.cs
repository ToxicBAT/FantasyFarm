
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
    private FarmTargeting farmTargeting;
    private FarmingActionHandler actionHandler;

    private void Awake()
    {
        farmTargeting = new FarmTargeting(
            playerMovement,
            worldGrid,
            transform,
            targetDistance
        );

        targetHighlight = new TargetCellHighlight(
            targetHighlightColor,
            borderThickness,
            borderHeight,
            borderGap
        );

        actionHandler = new FarmingActionHandler(playerInventory);
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

        if (!farmTargeting.TryGetTargetCell(
            out FarmCell cell, out _, out _))
        {
            return;
        }

        actionHandler.PerformAction(cell);
    }

    private void UpdateTargetHighlight()
    {
        if (playerMovement == null || worldGrid == null ||
            targetHighlight == null)
        {
            targetHighlight?.Hide();
            return;
        }

        if (farmTargeting.TryGetTargetCell(
            out FarmCell cell, out int x, out int z))
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
