
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FarmCell : MonoBehaviour
{
    [Header("Grid Cell")]
    [SerializeField] private Renderer cellRenderer;

    public Renderer CellRenderer => cellRenderer;

    // Generic construct occupancy
    public GameObject OccupyingConstruct { get; private set; }

    public bool IsOccupied => OccupyingConstruct != null;

    public CropPlot OccupyingCropPlot =>
        OccupyingConstruct != null
            ? OccupyingConstruct.GetComponent<CropPlot>()
            : null;

    private void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<Renderer>();

        if (cellRenderer == null)
        {
            Debug.LogError(
                "FarmCell: No Renderer found.",
                this
            );
        }
    }

    public bool TryPlaceConstruct(GameObject construct)
    {
        if (construct == null || IsOccupied)
            return false;

        OccupyingConstruct = construct;
        return true;
    }

    public void RemoveConstruct(GameObject construct)
    {
        if (OccupyingConstruct != construct)
            return;

        OccupyingConstruct = null;
    }
}
