using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class BikePart // A part can be made of multiple mesh filters (ex : to change both tires simultaneously), each using multiple material slots (ex : the frame is made of several parts)
{
    public string name;
    public Button selectorButton;
    public Material[] allPossibleMaterials;
    [HideInInspector]
    public Material currentMaterial;
    [HideInInspector]
    public Color currentColor;

    [FormerlySerializedAs("allElementsForPart")]
    [SerializeField]
    private BikePartElement[] _allElementsForPart;

    [FormerlySerializedAs("startColor")]
    [SerializeField]
    private Color _startColor = Color.grey;

    [FormerlySerializedAs("startMaterial")]
    [SerializeField]
    private Material _startMaterial;
    

    public void Init()
    {
        currentColor = _startColor;
        currentMaterial = _startMaterial;
        foreach (var element in _allElementsForPart)
        {
            var tempMatArray = element.meshRenderer.materials;

            foreach (var slot in element.materialSlots)
            {
                tempMatArray[slot] = currentMaterial;
                tempMatArray[slot].SetColor("_BaseColor", currentColor);
            }

            element.meshRenderer.materials = tempMatArray;
        }
    }


    public void SetNewMaterial(Material newMaterial)
    {
        currentMaterial = newMaterial;


        foreach (var element in _allElementsForPart)
        {

            var tempMatArray = element.meshRenderer.materials;

            foreach (var slot in element.materialSlots)
            {
                tempMatArray[slot] = currentMaterial;
                tempMatArray[slot].SetColor("_BaseColor", currentColor);
            }

            element.meshRenderer.materials = tempMatArray;
        }
    }

    public void SetNewColor(Color color)
    {
        currentColor = color;
        foreach (var element in _allElementsForPart)
        {
            foreach (var slot in element.materialSlots)
            {
                element.meshRenderer.materials[slot].SetColor("_BaseColor", currentColor);
            }
        }
    }

}

[Serializable]
public struct BikePartElement
{
    public MeshRenderer meshRenderer;
    public int[] materialSlots;
}
