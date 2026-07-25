using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// One Flow-style grid tile: muted empty cell, saturated path fill, or icon endpoint.
    /// </summary>
    public class DotCellView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
    {
        #region Serialized Fields
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        #endregion

        #region State
        private int x;
        private int y;
        private bool showArtBackground;
        #endregion

        #region Events
        public event Action<DotCellView> PointerDown;
        public event Action<DotCellView> PointerEnter;
        public event Action<DotCellView> PointerUp;
        #endregion

        #region Properties
        public int X => x;
        public int Y => y;
        public Vector2Int Cell => new Vector2Int(x, y);
        #endregion

        #region Public Methods
        public void Initialize(int cellX, int cellY, bool artBackground = false)
        {
            x = cellX;
            y = cellY;
            showArtBackground = artBackground;

            if (backgroundImage == null)
            {
                backgroundImage = GetComponent<Image>();
            }

            SetEmpty();
        }

        /// <summary>
        /// When true, empty cells stay clear so GridBackground.png shows through.
        /// </summary>
        public void SetArtBackgroundMode(bool enabled)
        {
            showArtBackground = enabled;
        }

        public void SetEmpty()
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = showArtBackground
                    ? DotConnectTheme.EmptyTileClear
                    : DotConnectTheme.EmptyTile;
            }

            if (iconImage != null)
            {
                iconImage.enabled = false;
            }
        }

        /// <summary>
        /// Fills the whole tile with a saturated path color (Flow reference look).
        /// </summary>
        public void SetPathColor(Color color)
        {
            if (backgroundImage != null)
            {
                Color fill = color;
                fill.a = showArtBackground ? 0.85f : 1f;
                backgroundImage.color = fill;
            }

            if (iconImage != null)
            {
                iconImage.enabled = false;
            }
        }

        /// <summary>
        /// Shows a pair icon (banana, gem, etc.) or a solid colored disc fallback.
        /// </summary>
        public void SetEndpoint(Color color, Sprite icon)
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = showArtBackground
                    ? DotConnectTheme.EmptyTileClear
                    : DotConnectTheme.EmptyTile;
            }

            if (iconImage == null)
            {
                return;
            }

            iconImage.enabled = true;
            if (icon != null)
            {
                iconImage.sprite = icon;
                iconImage.color = Color.white;
                iconImage.preserveAspect = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.color = color;
            }
        }

        public void SetDimmed(bool dimmed)
        {
            float alpha = dimmed ? 0.45f : 1f;
            if (backgroundImage != null)
            {
                Color c = backgroundImage.color;
                c.a = alpha;
                backgroundImage.color = c;
            }

            if (iconImage != null && iconImage.enabled)
            {
                Color c = iconImage.color;
                c.a = alpha;
                iconImage.color = c;
            }
        }
        #endregion

        #region Pointer Handlers
        public void OnPointerDown(PointerEventData eventData) => PointerDown?.Invoke(this);

        public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(this);

        public void OnPointerUp(PointerEventData eventData) => PointerUp?.Invoke(this);
        #endregion
    }
}
