using System;
using UnityEngine;
using UnityEngine.UI;


public class ColorPickerControl : MonoBehaviour
{
    public float currentHue, currentSat, currentVal;
    public Action<Color> OnNewColorSelected;

    [SerializeField]
    private SVImageControl _svImageControl;

    [SerializeField]
    private RawImage _hueImage, _satValImage, _outputImage;

    [SerializeField]
    private Slider _hueSlider;

    private Texture2D _hueTexture, _svTexture, _outputTexture;

    private void Awake()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();

        _svImageControl.OnValueChanged += (vector) => SetSV(vector);
        _hueSlider.onValueChanged.AddListener((value) => SetHue(value));
        SetColor(Color.red);
    }


    public void SetColor(Color color)
    {
        float hue, sat, val = 0f;
        Color.RGBToHSV(color, out hue, out sat, out val);

        _hueSlider.SetValueWithoutNotify(hue);
        SetHue(hue, false);
        SetSV(new Vector2(sat, val), false);
        _svImageControl.SetPickerPositionFromSV(sat, val);
    }

    private void CreateHueImage()
    {
        _hueTexture = new Texture2D(1, 16);
        _hueTexture.wrapMode = TextureWrapMode.Clamp;
        _hueTexture.name = "HueTexture";

        for (int i = 0; i < _hueTexture.height; i++)
        {
            _hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / _hueTexture.height, 1, 1));
        }

        _hueTexture.Apply();
        currentHue = 0;
        _hueImage.texture = _hueTexture;
    }

    private void CreateSVImage()
    {

        _svTexture = new Texture2D(16, 16);
        _svTexture.wrapMode = TextureWrapMode.Clamp;
        _svTexture.name = "SatValTexture";

        for(int y = 0; y < _svTexture.height; y++)
        {
            for(int x = 0; x < _svTexture.width; x++)
            {
                _svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue,
                    (float)x / _svTexture.width,
                    (float)y / _svTexture.height));
            }
        }

        _svTexture.Apply();
        currentSat = 0;
        currentVal = 0;

        _satValImage.texture = _svTexture;
    }

    private void CreateOutputImage()
    {
        _outputTexture = new Texture2D(16, 1);
        _outputTexture.wrapMode = TextureWrapMode.Clamp;
        _outputTexture.name = "OutputTexture";
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);
    
        for(int i = 0; i < _outputTexture.width; i++)
        {
            _outputTexture.SetPixel(i, 0, currentColor);
        }
    
        _outputTexture.Apply();
        _outputImage.texture = _outputTexture;
    }

    public void SetHue(float hue, bool notify = true)
    {
        currentHue = hue;
        UpdateSVImage();
        UpdateOutputImage(notify);
    }

    public void SetSV(Vector2 satValVector, bool notify = true)
    {
        currentSat = satValVector.x;
        currentVal = satValVector.y;

        UpdateOutputImage(notify);
    }

    public void UpdateSVImage()
    {
        for(int y = 0;  y < _svTexture.height; y++)
        {
            for(int x = 0;  x < _svTexture.width; x++)
            {
                _svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue,
                    (float)x / _svTexture.width,
                    (float)y / _svTexture.height));
            }
        }

        _svTexture.Apply();
    }


    private void UpdateOutputImage(bool notify = true)
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for(int i = 0; i < _outputTexture.width; i++)
        {
            _outputTexture.SetPixel(i, 0, currentColor);
        }

        _outputTexture.Apply();
        _outputImage.texture = _outputTexture;
        if (notify)
        {
            OnNewColorSelected?.Invoke(currentColor);
        }
    }
}
