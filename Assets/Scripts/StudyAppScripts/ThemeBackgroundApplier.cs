using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to any scene object that should swap background art when the theme changes.
/// Assign four sprites in order: slot 0 = theme 1, slot 1 = theme 2, etc.
/// </summary>
[DisallowMultipleComponent]
public class ThemeBackgroundApplier : MonoBehaviour
{
    [SerializeField] Image targetImage;
    [SerializeField] SpriteRenderer targetSpriteRenderer;
    [SerializeField] Sprite[] themeBackgrounds = new Sprite[ThemeManager.ThemeCount];

    void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetSpriteRenderer == null)
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        ThemeManager.ThemeChanged += ApplyTheme;
        ApplyTheme(ThemeManager.SelectedIndex);
    }

    void OnDisable()
    {
        ThemeManager.ThemeChanged -= ApplyTheme;
    }

    void ApplyTheme(int themeIndex)
    {
        if (themeBackgrounds == null || themeBackgrounds.Length == 0)
            return;

        themeIndex = Mathf.Clamp(themeIndex, 0, themeBackgrounds.Length - 1);
        Sprite sprite = themeBackgrounds[themeIndex];
        if (sprite == null)
            return;

        if (targetImage != null)
            targetImage.sprite = sprite;

        if (targetSpriteRenderer != null)
            targetSpriteRenderer.sprite = sprite;
    }
}
