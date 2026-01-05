using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MaterialDisplayer : MonoBehaviour
{
    public Action<Material> OnMaterialSelect;

    [SerializeField]
    private UIMeshCamera _uiMeshCameraPrefab;

    [SerializeField]
    private RectTransform _materialButtonsParent;
    [SerializeField]
    private Button _materialButtonPrefab;

    private const float _uiMeshCameraXOffset = 5f;
    private const int _renderTextureSize = 256;
    private List<UIMeshCamera> _activeUIMeshCameras;
    private List<RenderTexture> _activeRenderTextures;
    


    public void LoadMaterials(Material[] newMaterialToLoad)
    {

        CleanPreviousActives();

        foreach (Material mat in newMaterialToLoad)
        {

            var newButton = CreateMaterialButton(mat);
            var renderTexture = CreateNewRenderTexture(mat);

            newButton.GetComponent<RawImage>().texture = renderTexture;
        }
    }

    public void SetColor(Color color)
    {
        foreach(var meshCamera in _activeUIMeshCameras)
        {
            meshCamera.meshRenderer.material.SetColor("_BaseColor", color);
        }
    }


    private void CleanPreviousActives()
    {
        if(_activeUIMeshCameras != null && _activeUIMeshCameras.Count >= 1)
        {
            for(int i = _activeUIMeshCameras.Count - 1; i >= 0; i--)
            {
                _activeUIMeshCameras[i].uiCamera.targetTexture = null;
                Destroy(_activeUIMeshCameras[i].gameObject);
            }
        }
        _activeUIMeshCameras = new List<UIMeshCamera>();

        if (_activeRenderTextures != null && _activeRenderTextures.Count >= 1)
        {
            for (int i = _activeRenderTextures.Count - 1; i >= 0; i--)
            {
                _activeRenderTextures[i].Release();
                Destroy(_activeRenderTextures[i]);
            }
            
        }
        _activeRenderTextures = new List<RenderTexture>();

        for(int i = _materialButtonsParent.childCount - 1; i >= 0; --i)
        {
            Destroy(_materialButtonsParent.GetChild(i).gameObject);
        }

    }

    private Button CreateMaterialButton(Material material)
    {
        Button newButton = Instantiate(_materialButtonPrefab, _materialButtonsParent);
        var textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if(textComponent != null) textComponent.text = material.name;
        newButton.onClick.AddListener(() => OnMaterialSelect?.Invoke(material));
        return newButton;
    }

    private RenderTexture CreateNewRenderTexture(Material materialToDisplay)
    {
        Vector3 spawnPosition = new Vector3(_uiMeshCameraXOffset * _activeUIMeshCameras.Count, 0f, 0f);
        UIMeshCamera newUIMeshCamera = Instantiate(_uiMeshCameraPrefab, spawnPosition, Quaternion.identity);

        newUIMeshCamera.meshRenderer.material = materialToDisplay;

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor
        {
            width = _renderTextureSize,
            height = _renderTextureSize,
            volumeDepth = 1,
            dimension = TextureDimension.Tex2D,
            graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
            depthBufferBits = 8,
            msaaSamples = 1
            
        };

        RenderTexture newRenderTexture = new RenderTexture(descriptor);

        newRenderTexture.Create();

        newUIMeshCamera.uiCamera.forceIntoRenderTexture = true;
        newUIMeshCamera.uiCamera.targetTexture = newRenderTexture;
        _activeRenderTextures.Add(newRenderTexture);
        _activeUIMeshCameras.Add(newUIMeshCamera);
        return newRenderTexture;
    }


}
