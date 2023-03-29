using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Floating text preset that contains : name, color, font size.
/// </summary>
[System.Serializable]
public class FloatingTextPreset
{
    public string Name = "New color";
    public Color Color = Color.white;
    public int FontSize = 30;

    public FloatingTextPreset(string name, Color color, int fontSize)
    {
        Name = name;
        Color = color;
        FontSize = fontSize;
    }
}

/// <summary>
/// Attach this script to the main canvas gameobject.
/// You can create a floating text by accessign the static instance of this class and call the 'CreateFloatingText' method.
/// FloatingText.Instance.CreateFloatingText(worldPosition, text, fontMultiplier, color);
/// </summary>
[RequireComponent(typeof(Canvas))]
public class FloatingText : MonoBehaviour
{
    public static FloatingText Instance;

    [Header("Animation")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private AnimationCurve _sizeOverTime;
    [Header("Font")]
    [SerializeField] private FontStyles _fontStyle;
    [SerializeField] private float _outlineWidth = 0.15f;
    [SerializeField] private TMP_FontAsset _fontAsset;
    [SerializeField] private List<FloatingTextPreset> _presetsList = new List<FloatingTextPreset>();

    private Dictionary<string, FloatingTextPreset> _presets = new Dictionary<string, FloatingTextPreset>();
    private Transform _floatingTextContainer;
    private ObjectPool<TextMeshProUGUI> _floatinTextPool;

    private void Awake()
    {
        if (FloatingText.Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        ConvertPresetsListToDictionnary();
    }

    private void Start()
    {
        _floatingTextContainer = GetFloatingTextContainer();
        _floatinTextPool = new ObjectPool<TextMeshProUGUI>(CreateFloatingTextObject);
    }

    private Transform GetFloatingTextContainer()
    {
        if (_floatingTextContainer == null)
        {
            GameObject container = new GameObject("FloatingTextContainer");
            container.transform.parent = transform;
            container.transform.localPosition = Vector3.zero;
            return container.transform;
        }
        else
        {
            return _floatingTextContainer;
        }
    }

    public void CreateFloatingText(Vector3 worldPosition, string text, string presetName = null)
    {
        StartCoroutine(SpawnFloatingText(worldPosition, text, presetName));
    }

    private IEnumerator SpawnFloatingText(Vector3 worldPosition, string text, string presetName)
    {
        TextMeshProUGUI instance = _floatinTextPool.Get();
        FloatingTextPreset preset = GetPreset(presetName);

        instance.SetText(text);
        instance.fontSize = preset.FontSize;
        instance.color = preset.Color;
        instance.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        instance.gameObject.SetActive(true);

        yield return null;

        float time = 0f;
        while (time < _duration)
        {
            instance.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
            float scale = _sizeOverTime.Evaluate(time/_duration);
            instance.transform.localScale = new Vector3(scale, scale, scale);
            time += Time.deltaTime;

            yield return null;
        }

        yield return null;
        instance.gameObject.SetActive(false);
        _floatinTextPool.Release(instance);
    }

    private TextMeshProUGUI CreateFloatingTextObject()
    {
        GameObject instance = new GameObject("FloatingText");
        instance.transform.parent = _floatingTextContainer;
        TextMeshProUGUI textComponent = instance.AddComponent<TextMeshProUGUI>();
        if (_fontAsset != null) { textComponent.font = _fontAsset; }
        textComponent.outlineWidth = _outlineWidth;
        textComponent.fontStyle = _fontStyle;
        textComponent.alignment = TextAlignmentOptions.Center;
        return textComponent;
    }

    private FloatingTextPreset GetPreset(string presetName)
    {
        if (presetName == null || !_presets.ContainsKey(presetName))
        {
            return _presets["_default"];
        }
        else
        {
            return _presets[presetName];
        }
    }

    private void ConvertPresetsListToDictionnary()
    {
        for (int i = 0; i < _presetsList.Count; i++)
        {
            _presets.Add(_presetsList[i].Name, _presetsList[i]);
        }
        _presets.Add("_default", new FloatingTextPreset(name: "_default", color: Color.white, fontSize: 30));
    }
}
