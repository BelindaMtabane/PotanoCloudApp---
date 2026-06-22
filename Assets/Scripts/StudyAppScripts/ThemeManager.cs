using System;
using UnityEngine;

/// <summary>
/// Stores the selected background theme index (0–3, displayed as themes 1–4).
/// Persists via PlayerPrefs and notifies listeners when the theme changes.
/// </summary>
public static class ThemeManager
{
    public const string ThemeIndexKey = "LastTheme";
    public const int ThemeCount = 4;

    public static int SelectedIndex { get; private set; }

    /// <summary>Theme number for display or logic (1–4).</summary>
    public static int SelectedThemeNumber => SelectedIndex + 1;

    public static event Action<int> ThemeChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadSavedTheme()
    {
        SelectedIndex = PlayerPrefs.GetInt(ThemeIndexKey, 0);
        SelectedIndex = Mathf.Clamp(SelectedIndex, 0, ThemeCount - 1);
    }

    public static void SetTheme(int index)
    {
        index = Mathf.Clamp(index, 0, ThemeCount - 1);
        SelectedIndex = index;

        PlayerPrefs.SetInt(ThemeIndexKey, index);
        PlayerPrefs.Save();

        ThemeChanged?.Invoke(index);
    }

    public static void SetThemeNumber(int themeNumber)
    {
        SetTheme(themeNumber - 1);
    }

    public static void RefreshTheme()
    {
        ThemeChanged?.Invoke(SelectedIndex);
    }
}
