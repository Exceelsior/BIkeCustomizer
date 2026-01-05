using UnityEngine;

public class BikeCustomizer : MonoBehaviour
{
    [SerializeField]
    private BikePart[] _allBikeParts;
    [SerializeField]
    private MaterialDisplayer _materialDisplayer;
    [SerializeField]
    private ColorPickerControl _colorPickerControl;

    private BikePart _activeBikePart;


    private void Start()
    {
        if (_allBikeParts == null || _allBikeParts.Length <= 0) return;

        foreach (var part in _allBikeParts)
        {
            part.Init();
            part.selectorButton.onClick.AddListener(() => SelectNewPart(part));
        }

        _activeBikePart = _allBikeParts[0];
        SelectNewPart(_activeBikePart);
        _colorPickerControl.OnNewColorSelected += ChangePartColor;
        _materialDisplayer.OnMaterialSelect += ChangePartMaterial;
    }


    void SelectNewPart(BikePart newPart)
    {
        _activeBikePart = newPart;

        _colorPickerControl.SetColor(_activeBikePart.currentColor);
        _materialDisplayer.LoadMaterials(_activeBikePart.allPossibleMaterials);
        _materialDisplayer.SetColor(_activeBikePart.currentColor);
    }

    void ChangePartMaterial(Material newMaterial)
    {
        _activeBikePart.SetNewMaterial(newMaterial);
    }

    void ChangePartColor(Color newColor)
    {
        _activeBikePart.SetNewColor(newColor);
        _materialDisplayer.SetColor(_activeBikePart.currentColor);
    }

}