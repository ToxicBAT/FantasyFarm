
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructPlacementTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private GameObject constructPrefab;

    [Header("Placement")]
    [SerializeField] private float targetDistance = 1f;
    [SerializeField] private float verticalOffset = 0.1f;

    [Header("Footprint")]
    [SerializeField] private int footprintWidth = 2;
    [SerializeField] private int footprintDepth = 2;
    [SerializeField] private int rotationDegrees = 0;

    private FarmTargeting farmTargeting;
    private ConstructPlacement constructPlacement;
    private ConstructFootprint footprint;

    private void Awake()
    {
        farmTargeting = new FarmTargeting(
            playerMovement,
            worldGrid,
            transform,
            targetDistance
        );

        constructPlacement = new ConstructPlacement(worldGrid);

    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null &&
            Keyboard.current.pKey.wasPressedThisFrame)
        {
            TryPlaceConstruct();
        }
#endif
    }

    private void TryPlaceConstruct()
    {
        if (worldGrid == null || constructPrefab == null)
        {
            Debug.LogError(
                "ConstructPlacementTester: Assign WorldGrid and Construct Prefab."
            );
            return;
        }

        if (!farmTargeting.TryGetTargetCell(
            out FarmCell targetCell,
            out int x,
            out int z))
        {
            Debug.Log("No valid target cell for placement.");
            return;
        }

        Vector2Int anchor = new(x, z);

        footprint = new ConstructFootprint(
            footprintWidth,
            footprintDepth
        );

        constructPlacement.TryPlace(
            constructPrefab,
            footprint,
            anchor,
            rotationDegrees,
            verticalOffset,
            out _
        );
    }
}
