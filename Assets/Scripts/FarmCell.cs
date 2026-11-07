
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class FarmCell : MonoBehaviour
{
    [Header("Soil")]
    [SerializeField] private Renderer cellRenderer;
    [SerializeField] private Material untilledMaterial;
    [SerializeField] private Material tilledMaterial;

    [Header("Crop Prefabs")]
    [SerializeField] private GameObject seedlingPrefab;
    [SerializeField] private GameObject smallPlantPrefab;
    [SerializeField] private GameObject matureCropPrefab;

    [Header("Growth Timing")]
    [SerializeField] private float stageDuration = 30f;

    public bool IsTilled { get; private set; }
    public bool HasCrop { get; private set; }
    public int GrowthStage { get; private set; }

    public Renderer CellRenderer => cellRenderer;

    private GameObject cropVisual;
    private float growthTimer;

    private void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<Renderer>();

        ValidateConfiguration();

        RefreshSoilAppearance();
    }

    private void ValidateConfiguration()
    {
        bool configurationValid = true;

        if (cellRenderer == null)
        {
            Debug.LogError(
                "FarmCell: No Renderer found.",
                this
            );

            configurationValid = false;
        }

        if (untilledMaterial == null)
        {
            Debug.LogError(
                "FarmCell: Until­led material is not assigned.",
                this
            );

            configurationValid = false;
        }

        if (tilledMaterial == null)
        {
            Debug.LogError(
                "FarmCell: Tilled material is not assigned.",
                this
            );

            configurationValid = false;
        }

        if (seedlingPrefab == null)
        {
            Debug.LogError(
                "FarmCell: Seedling prefab is not assigned.",
                this
            );

            configurationValid = false;
        }

        if (smallPlantPrefab == null)
        {
            Debug.LogError(
                "FarmCell: Small plant prefab is not assigned.",
                this
            );

            configurationValid = false;
        }

        if (matureCropPrefab == null)
        {
            Debug.LogError(
                "FarmCell: Mature crop prefab is not assigned.",
                this
            );

            configurationValid = false;
        }

        if (stageDuration <= 0f)
        {
            Debug.LogError(
                "FarmCell: Stage duration must be greater than zero.",
                this
            );

            configurationValid = false;
        }

        if (!configurationValid)
        {
            Debug.LogError(
                "FarmCell: Configuration is invalid. Check the Inspector.",
                this
            );
        }
    }

    public void Till()
    {
        if (IsTilled)
            return;

        IsTilled = true;
        RefreshSoilAppearance();
    }

    public void Plant()
    {
        if (!IsTilled || HasCrop)
            return;

        if (seedlingPrefab == null ||
            smallPlantPrefab == null ||
            matureCropPrefab == null)
        {
            Debug.LogError(
                "Assign all three crop prefabs to FarmCell."
            );
            return;
        }

        HasCrop = true;
        GrowthStage = 0;
        growthTimer = 0f;
        ShowCropStage();
    }

    public bool Harvest()
    {
        if (!HasCrop || GrowthStage < 2)
            return false;

        if (cropVisual != null)
            Destroy(cropVisual);

        cropVisual = null;
        HasCrop = false;
        GrowthStage = 0;
        growthTimer = 0f;

        IsTilled = false;
        RefreshSoilAppearance();

        Debug.Log("Crop harvested! Soil returned to untilled state.");
        return true;
    }

    private void Update()
    {
        if (!HasCrop || GrowthStage >= 2)
            return;

        growthTimer += Time.deltaTime;

        if (growthTimer >= stageDuration)
        {
            growthTimer -= stageDuration;
            GrowthStage++;
            ShowCropStage();
        }
    }

    private void ShowCropStage()
    {
        if (cropVisual != null)
            Destroy(cropVisual);

        GameObject prefab = GrowthStage switch
        {
            0 => seedlingPrefab,
            1 => smallPlantPrefab,
            _ => matureCropPrefab
        };

        Vector3 position = transform.position
            + Vector3.up * 0.15f;

        cropVisual = Instantiate(
            prefab,
            position,
            Quaternion.identity,
            transform
        );
    }

    private void RefreshSoilAppearance()
    {
        if (cellRenderer != null)
        {
            cellRenderer.sharedMaterial =
                IsTilled ? tilledMaterial : untilledMaterial;
        }
    }
}
