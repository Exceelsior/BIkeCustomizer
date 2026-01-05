using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<Vector2> OnValueChanged;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Image _pickerImage;
    [SerializeField]
    private RawImage _svImage;

    private RectTransform _svRectTransform, _pickerTransform;

    private void Awake()
    {
        _svRectTransform = _svImage.rectTransform;

        _pickerTransform = _pickerImage.rectTransform;
        _pickerTransform.position = new Vector2(-(_svRectTransform.sizeDelta.x * .5f), -(_svRectTransform.sizeDelta.y * .5f));
    }

    void UpdateColor(PointerEventData eventData)
    {
        Vector3 pos = _svRectTransform.InverseTransformPoint(eventData.position);

        float deltaX = _svRectTransform.sizeDelta.x * .5f;
        float deltaY = _svRectTransform.sizeDelta.y * .5f;

        if (pos.x < -deltaX)
        {
            pos.x = - deltaX;
        }
        else if (pos.x > deltaX)
        {
            pos.x = deltaX;
        }

        if (pos.y < -deltaY)
        {
            pos.y = - deltaY;
        }
        else if (pos.y > deltaY) {
            pos.y = deltaY;
        }

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNorm = (float)x / _svRectTransform.sizeDelta.x;
        float yNorm = (float)y / _svRectTransform.sizeDelta.y;

        _pickerTransform.localPosition = pos;
        _pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);

        OnValueChanged?.Invoke(new Vector2(xNorm, yNorm));
    }

    public void SetPickerPositionFromSV(float saturation, float value)
    {
        var halfX = _svRectTransform.sizeDelta.x * 0.5f;
        var halfY = _svRectTransform.sizeDelta.y * 0.5f;

        float x = Mathf.Lerp(-halfX, halfX, saturation);
        float y = Mathf.Lerp(-halfY, halfY, value);

        Vector2 localPos = new(x, y);

        _pickerTransform.localPosition = localPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
