using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Builds Flow-style placeholder levels using the study-themed pixel icons
    /// (camera, palette, color disc, controller, books).
    /// </summary>
    public static class DotConnectLevelFactory
    {
        #region Public Methods
        public static DotConnectLevel CreatePlaceholder(MiniGameDifficulty difficulty)
        {
            DotConnectLevel level = ScriptableObject.CreateInstance<DotConnectLevel>();

            switch (difficulty)
            {
                case MiniGameDifficulty.Medium:
                    level.ConfigureRuntime(
                        "Level 2",
                        difficulty,
                        2,
                        7,
                        140f,
                        new[]
                        {
                            Pair(DotConnectTheme.CherryRed, new Vector2Int(0, 0), new Vector2Int(6, 0), DotConnectIconCatalog.Camera),
                            Pair(DotConnectTheme.GemBlue, new Vector2Int(0, 3), new Vector2Int(6, 3), DotConnectIconCatalog.Controller),
                            Pair(DotConnectTheme.GemGreen, new Vector2Int(0, 6), new Vector2Int(6, 6), DotConnectIconCatalog.Books),
                            Pair(DotConnectTheme.BananaYellow, new Vector2Int(2, 1), new Vector2Int(2, 5), DotConnectIconCatalog.Palette),
                            Pair(DotConnectTheme.GemPurple, new Vector2Int(4, 2), new Vector2Int(4, 4), DotConnectIconCatalog.ColorDisc)
                        });
                    break;

                case MiniGameDifficulty.Hard:
                    level.ConfigureRuntime(
                        "Level 1",
                        difficulty,
                        1,
                        9,
                        180f,
                        CreateNineByNinePairs());
                    break;

                default:
                    level.ConfigureRuntime(
                        "Level 1",
                        MiniGameDifficulty.Easy,
                        1,
                        5,
                        100f,
                        new[]
                        {
                            Pair(DotConnectTheme.CherryRed, new Vector2Int(0, 0), new Vector2Int(4, 0), DotConnectIconCatalog.Camera),
                            Pair(DotConnectTheme.GemBlue, new Vector2Int(0, 2), new Vector2Int(4, 2), DotConnectIconCatalog.Controller),
                            Pair(DotConnectTheme.GemGreen, new Vector2Int(0, 4), new Vector2Int(4, 4), DotConnectIconCatalog.Books),
                            Pair(DotConnectTheme.BananaYellow, new Vector2Int(2, 1), new Vector2Int(2, 3), DotConnectIconCatalog.Palette)
                        });
                    break;
            }

            return level;
        }
        #endregion

        #region Private Methods
        private static ColorDotPair[] CreateNineByNinePairs()
        {
            // One pair per icon on a 9×9 board.
            return new[]
            {
                Pair(DotConnectTheme.CherryRed, new Vector2Int(0, 0), new Vector2Int(8, 0), DotConnectIconCatalog.Camera),
                Pair(DotConnectTheme.GemBlue, new Vector2Int(0, 4), new Vector2Int(8, 4), DotConnectIconCatalog.Controller),
                Pair(DotConnectTheme.GemGreen, new Vector2Int(0, 8), new Vector2Int(8, 8), DotConnectIconCatalog.Books),
                Pair(DotConnectTheme.BananaYellow, new Vector2Int(2, 2), new Vector2Int(2, 6), DotConnectIconCatalog.Palette),
                Pair(DotConnectTheme.GemPurple, new Vector2Int(6, 2), new Vector2Int(6, 6), DotConnectIconCatalog.ColorDisc)
            };
        }

        private static ColorDotPair Pair(Color color, Vector2Int start, Vector2Int end, string iconName)
        {
            return new ColorDotPair(color, start, end, DotConnectIconCatalog.Get(iconName));
        }
        #endregion
    }
}
