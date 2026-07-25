using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Soft cloud palette matching the Flow-style Dot Connect reference screen.
    /// </summary>
    public static class DotConnectTheme
    {
        #region Colors
        /// <summary>Muted grey-blue empty tile (used when no art background).</summary>
        public static readonly Color EmptyTile = new Color(0.72f, 0.78f, 0.86f, 1f);

        /// <summary>Transparent empty tile so the colorful grid art shows through.</summary>
        public static readonly Color EmptyTileClear = new Color(1f, 1f, 1f, 0f);

        /// <summary>Soft sky background suggestion for the canvas.</summary>
        public static readonly Color SkyBackground = new Color(0.78f, 0.88f, 0.96f, 1f);

        /// <summary>Footer / panel wash.</summary>
        public static readonly Color PanelWash = new Color(0.93f, 0.96f, 1f, 1f);

        /// <summary>Magenta footer button tint from the reference.</summary>
        public static readonly Color FooterAccent = new Color(0.86f, 0.28f, 0.55f, 1f);

        /// <summary>Header stats text (darker blue).</summary>
        public static readonly Color StatsText = new Color(0.20f, 0.35f, 0.55f, 1f);
        #endregion

        #region Pair Colors (saturated path colors)
        public static readonly Color BananaYellow = new Color(1f, 0.84f, 0.10f, 1f);
        public static readonly Color CherryRed = new Color(0.90f, 0.18f, 0.22f, 1f);
        public static readonly Color HeartPink = new Color(0.95f, 0.45f, 0.70f, 1f);
        public static readonly Color GemGreen = new Color(0.25f, 0.78f, 0.35f, 1f);
        public static readonly Color GemBlue = new Color(0.20f, 0.55f, 0.95f, 1f);
        public static readonly Color GemCyan = new Color(0.35f, 0.78f, 0.95f, 1f);
        public static readonly Color GemPurple = new Color(0.55f, 0.30f, 0.75f, 1f);
        public static readonly Color GemOrange = new Color(0.98f, 0.55f, 0.12f, 1f);
        public static readonly Color GemTeal = new Color(0.15f, 0.70f, 0.70f, 1f);
        #endregion
    }
}
