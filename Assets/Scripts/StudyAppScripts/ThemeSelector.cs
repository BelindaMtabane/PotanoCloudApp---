using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Four toggle buttons pick the active background theme (1–4).
/// Selection is saved automatically and restored on next app launch.
/// Hook each Toggle's On Value Changed to OnThemeToggle1–4, or call SelectTheme from buttons.
/// </summary>
public class ThemeSelector : MonoBehaviour
{
    [SerializeField] GameObject themeButton1;
    [SerializeField] GameObject themeButton2;
    [SerializeField] GameObject themeButton3;
    [SerializeField] GameObject themeButton4;

    [SerializeField] Toggle toggle1;
    [SerializeField] Toggle toggle2;
    [SerializeField] Toggle toggle3;
    [SerializeField] Toggle toggle4;

    Toggle[] toggles;
    bool isApplyingSavedTheme;

    void Awake()
    {
        ResolveToggleReferences();
    }

    void Start()
    {
        ApplySavedThemeToToggles();
    }

    void ResolveToggleReferences()
    {
        if (toggle1 == null && themeButton1 != null)
            toggle1 = themeButton1.GetComponent<Toggle>();
        if (toggle2 == null && themeButton2 != null)
            toggle2 = themeButton2.GetComponent<Toggle>();
        if (toggle3 == null && themeButton3 != null)
            toggle3 = themeButton3.GetComponent<Toggle>();
        if (toggle4 == null && themeButton4 != null)
            toggle4 = themeButton4.GetComponent<Toggle>();

        toggles = new[] { toggle1, toggle2, toggle3, toggle4 };
    }

    void ApplySavedThemeToToggles()
    {
        isApplyingSavedTheme = true;
        int savedIndex = ThemeManager.SelectedIndex;

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null)
                toggles[i].SetIsOnWithoutNotify(i == savedIndex);
        }

        isApplyingSavedTheme = false;
        ThemeManager.RefreshTheme();
    }

    public void OnThemeToggle1(bool isOn) => HandleToggleChanged(0, isOn);
    public void OnThemeToggle2(bool isOn) => HandleToggleChanged(1, isOn);
    public void OnThemeToggle3(bool isOn) => HandleToggleChanged(2, isOn);
    public void OnThemeToggle4(bool isOn) => HandleToggleChanged(3, isOn);

    void HandleToggleChanged(int index, bool isOn)
    {
        if (isApplyingSavedTheme || !isOn)
            return;

        SelectTheme(index);
    }

    public void SelectTheme1() => SelectTheme(0);
    public void SelectTheme2() => SelectTheme(1);
    public void SelectTheme3() => SelectTheme(2);
    public void SelectTheme4() => SelectTheme(3);

    public void SelectTheme(int index)
    {
        index = Mathf.Clamp(index, 0, ThemeManager.ThemeCount - 1);

        isApplyingSavedTheme = true;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null)
                toggles[i].SetIsOnWithoutNotify(i == index);
        }
        isApplyingSavedTheme = false;

        ThemeManager.SetTheme(index);
    }

    public int GetSelectedThemeNumber()
    {
        return ThemeManager.SelectedThemeNumber;
    }
}
