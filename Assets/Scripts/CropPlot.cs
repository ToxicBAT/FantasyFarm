
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CropPlot : MonoBehaviour
{
    [Header("Soil")]
    [SerializeField] private Renderer soilRenderer;
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
    public bool IsConfigured { get; private set; }

    private GameObject cropVisual;
    private float growthTimer;

    private void Awake()
    {
        if (soilRenderer == null)
            soilRenderer = GetComponent<Renderer>();

        ValidateConfiguration();
        RefreshSoilAppearance();
    }

    private void ValidateConfiguration()
    {
        IsConfigured =
            soilRenderer != null &&
            untilledMaterial != null &&
            tilledMaterial != null &&
            seedlingPrefab != null &&
            smallPlantPrefab != null &&
            matureCropPrefab != null &&
            stageDuration > 0f;

        if (!IsConfigured)
        {
            Debug.LogError(
                "CropPlot: Configuration is invalid. Check the Inspector.",
                this
            );
        }
    }

    public bool Till()
    {
        if (!IsConfigured)
            return false; 
        
        if (IsTilled)
            return false;

        IsTilled = true;
        RefreshSoilAppearance();
        return true;
    }

    public bool Plant()
    {
        if (!IsConfigured)
            return false; 
        
        if (!IsTilled || HasCrop)
            return false;

        if (seedlingPrefab == null ||
            smallPlantPrefab == null ||
            matureCropPrefab == null)
        {
            Debug.LogError(
                "CropPlot: Crop prefabs are not fully assigned.",
                this
            );
            return false;
        }

        HasCrop = true;
        GrowthStage = 0;
        growthTimer = 0f;
        ShowCropStage();

        return true;
    }

    public bool Harvest()
    {
        if (!IsConfigured)
            return false; 
        
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

        return true;
    }

    private void Update()
    {
        if (!IsConfigured)
            return; 
        
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
        if (soilRenderer != null)
        {
            soilRenderer.sharedMaterial =
                IsTilled ? tilledMaterial : untilledMaterial;
        }
    }
}
