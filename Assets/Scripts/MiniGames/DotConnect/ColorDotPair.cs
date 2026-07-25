using System;
using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// One matching icon pair on the grid (same color / sprite endpoints to connect).
    /// </summary>
    [Serializable]
    public class ColorDotPair
    {
        #region Serialized Fields
        [SerializeField] private Color displayColor = Color.red;
        [SerializeField] private Sprite iconSprite;
        [SerializeField] private Vector2Int startCell;
        [SerializeField] private Vector2Int endCell;
        #endregion

        #region Properties
        public Color DisplayColor => displayColor;
        public Sprite IconSprite => iconSprite;
        public Vector2Int StartCell => startCell;
        public Vector2Int EndCell => endCell;
        #endregion

        #region Constructors
        public ColorDotPair()
        {
        }

        public ColorDotPair(Color color, Vector2Int start, Vector2Int end, Sprite icon = null)
        {
            displayColor = color;
            startCell = start;
            endCell = end;
            iconSprite = icon;
        }
        #endregion
    }
}
