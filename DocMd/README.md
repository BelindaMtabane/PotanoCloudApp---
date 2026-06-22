# DocMd — Documentation Index

Central index for Potano Cloud Unity planning and design documentation.

**Project:** Potano Cloud · Unity 6 2D study companion · Android + Windows PC  
**Repo root:** [../](../)

---

## Folder Contents

| File | Description |
|---|---|
| [README.md](README.md) | This index |
| [Potano-Unity-Product-Design-Plan.md](Potano-Unity-Product-Design-Plan.md) | Master product design, UX, architecture, and 10-day roadmap |
| [Project-Architecture-Analysis.md](Project-Architecture-Analysis.md) | Codebase audit: architecture, dependencies, gaps, and technical debt |

---

## Product Design Plan — Section Index

Full document: **[Potano-Unity-Product-Design-Plan.md](Potano-Unity-Product-Design-Plan.md)**

| # | Section | Link |
|---|---|---|
| 1 | Product Vision | [§1](Potano-Unity-Product-Design-Plan.md#1-product-vision) |
| 2 | Target Audience | [§2](Potano-Unity-Product-Design-Plan.md#2-target-audience) |
| 3 | User Personas | [§3](Potano-Unity-Product-Design-Plan.md#3-user-personas) |
| 4 | User Journey | [§4](Potano-Unity-Product-Design-Plan.md#4-user-journey) |
| 5 | Core Gameplay Loop | [§5](Potano-Unity-Product-Design-Plan.md#5-core-gameplay-loop) |
| 6 | Screen Hierarchy | [§6](Potano-Unity-Product-Design-Plan.md#6-screen-hierarchy) |
| 7 | User Flow | [§7](Potano-Unity-Product-Design-Plan.md#7-user-flow) |
| 8 | Information Architecture | [§8](Potano-Unity-Product-Design-Plan.md#8-information-architecture) |
| 9 | UI/UX Design Plan | [§9](Potano-Unity-Product-Design-Plan.md#9-uiux-design-plan) |
| 10 | Theme Design Specifications | [§10](Potano-Unity-Product-Design-Plan.md#10-theme-design-specifications) |
| 11 | Color Palette Recommendations | [§11](Potano-Unity-Product-Design-Plan.md#11-color-palette-recommendations) |
| 12 | Typography Recommendations | [§12](Potano-Unity-Product-Design-Plan.md#12-typography-recommendations) |
| 13 | Accessibility Considerations | [§13](Potano-Unity-Product-Design-Plan.md#13-accessibility-considerations) |
| 14 | Reward System Design | [§14](Potano-Unity-Product-Design-Plan.md#14-reward-system-design) |
| 15 | XP System Design | [§15](Potano-Unity-Product-Design-Plan.md#15-xp-system-design) |
| 16 | Music System Design | [§16](Potano-Unity-Product-Design-Plan.md#16-music-system-design) |
| 17 | Motivation System Design | [§17](Potano-Unity-Product-Design-Plan.md#17-motivation-system-design) |
| 18 | Data Architecture | [§18](Potano-Unity-Product-Design-Plan.md#18-data-architecture) |
| 19 | Scene Architecture | [§19](Potano-Unity-Product-Design-Plan.md#19-scene-architecture) |
| 20 | Folder Structure | [§20](Potano-Unity-Product-Design-Plan.md#20-folder-structure) |
| 21 | Development Roadmap | [§21](Potano-Unity-Product-Design-Plan.md#21-development-roadmap) |
| 22 | MVP Definition | [§22](Potano-Unity-Product-Design-Plan.md#22-mvp-definition) |
| 23 | Stretch Goals | [§23](Potano-Unity-Product-Design-Plan.md#23-stretch-goals) |
| 24 | Risks and Scope Control | [§24](Potano-Unity-Product-Design-Plan.md#24-risks-and-scope-control) |

### Appendices

| Appendix | Link |
|---|---|
| A — MVP Checklist | [Appendix A](Potano-Unity-Product-Design-Plan.md#appendix-a-mvp-checklist-printable) |
| B — Glossary | [Appendix B](Potano-Unity-Product-Design-Plan.md#appendix-b-glossary) |

---

## Implementation Status (vs. Design Plan)

| Feature | Design ref | Code / scene | Status |
|---|---|---|---|
| Background themes (4 slots) | [§10](Potano-Unity-Product-Design-Plan.md#10-theme-design-specifications) | `ThemeManager`, `ThemeSelector`, `ThemeBackgroundApplier` | Implemented |
| Theme save / load | [§18](Potano-Unity-Product-Design-Plan.md#18-data-architecture) | `PlayerPrefs` key `LastTheme` | Implemented |
| Theme picker UI | [§7 Flow F](Potano-Unity-Product-Design-Plan.md#flow-f-choose-background-theme-and-character) | `ThemeSetting.unity` | Scene exists — wire toggles in Editor |
| Mood gate | [§7](Potano-Unity-Product-Design-Plan.md#7-user-flow) | `StarterScreen.unity` | Planned |
| Name personalization | [§18](Potano-Unity-Product-Design-Plan.md#18-data-architecture) | `NameSystem.cs` | Partial |
| Focus timer | [§22](Potano-Unity-Product-Design-Plan.md#22-mvp-definition) | `StudySession.unity` | Planned |
| Music system | [§16](Potano-Unity-Product-Design-Plan.md#16-music-system-design) | `MusicScene.unity` | Planned |
| Events / planning | [§23](Potano-Unity-Product-Design-Plan.md#23-stretch-goals) | `EventsSystem.cs`, `TimeTable.unity` | Stretch |
| XP / streaks / coins | [§14–15](Potano-Unity-Product-Design-Plan.md#14-reward-system-design) | — | Planned |

---

## Related Codebase Index

### Scripts — [Assets/Scripts/StudyAppScripts/](../Assets/Scripts/StudyAppScripts/)

| Script | Role |
|---|---|
| `ThemeManager.cs` | Static theme index (0–3), PlayerPrefs save/load, change events |
| `ThemeSelector.cs` | Four toggle buttons; calls `ThemeManager.SetTheme` |
| `ThemeBackgroundApplier.cs` | Per-scene `Sprite[4]` arrays; swaps background by theme index |
| `NameSystem.cs` | Optional name save/load via PlayerPrefs |
| `NameDisplay.cs` | Name display on secondary screens |
| `EventsSystem.cs` | Event slot save/load (stretch: study goals) |
| `EventsDisplay.cs` | Session history display (stub) |
| `RealTimeInfor.cs` | Date/time dashboard widget |

### Scenes — [Assets/Scenes/](../Assets/Scenes/)

| Scene | Purpose |
|---|---|
| `StarterScreen.unity` | Onboarding / mood gate (planned) |
| `OpenPager.unity` | Main dashboard |
| `ThemeSetting.unity` | Background theme picker (4 toggles) |
| `StudySession.unity` | Focus timer session |
| `MusicScene.unity` | Music category settings |
| `TaskSession.unity` | Task session (stretch) |
| `TimeTable.unity` | Weekly calendar (stretch) |
| `Gameplay/GameScene.unity` | Gameplay / progress (evaluate) |

---

## Theme System Quick Reference

| Theme # | Index | Name (design doc) |
|---|---|---|
| 1 | 0 | Modern Classic |
| 2 | 1 | Artsy Skies |
| 3 | 2 | Bright Spectrum |
| 4 | 3 | Autumn Warmth |

**Setup:** Add `ThemeBackgroundApplier` to each scene’s background `Image`; assign four sprites per slot. In `ThemeSetting`, add `ThemeSelector` and wire toggles to `OnThemeToggle1`–`4`.

---

## Adding New Documents

Place new `.md` files in this folder and add a row to **Folder Contents** and any relevant cross-links above.
