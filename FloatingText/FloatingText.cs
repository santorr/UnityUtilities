using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections.Generic;

public enum EFloatingTextColor { White, Orange, Green }

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
    [SerializeField] private int _fontSize = 30;
    [SerializeField] private FontStyles _fontStyle;
    [SerializeField] private float _outlineWidth = 0.15f;
    [SerializeField] private TMP_FontAsset _fontAsset;
    
    private Transform _floatingTextContainer;
    private ObjectPool<TextMeshProUGUI> _floatinTextPool;
    private Dictionary<EFloatingTextColor, Color> _floatingColor = new Dictionary<EFloatingTextColor, Color>()
    {
        { EFloatingTextColor.White, Color.white },
        { EFloatingTextColor.Orange, Color.yellow },
        { EFloatingTextColor.Green, Color.green }
    };

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

    public void CreateFloatingText(Vector3 worldPosition, string text, float fontMultiplier = 1f, EFloatingTextColor color = EFloatingTextColor.White)
    {
        StartCoroutine(SpawnFloatingText(worldPosition, text, fontMultiplier, color));
    }

    private IEnumerator SpawnFloatingText(Vector3 worldPosition, string text, float fontMultiplier, EFloatingTextColor color)
    {
        TextMeshProUGUI instance = _floatinTextPool.Get();

        instance.SetText(text);
        instance.fontSize = _fontSize * fontMultiplier;
        instance.color = _floatingColor[color];
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
        textComponent.fontSize = _fontSize;
        textComponent.fontStyle = _fontStyle;
        textComponent.alignment = TextAlignmentOptions.Center;
        return textComponent;
    }
}