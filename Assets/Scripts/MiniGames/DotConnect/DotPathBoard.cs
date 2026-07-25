using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Builds the Flow-style UI grid and routes drag input into <see cref="DotPathState"/>.
    /// </summary>
    public class DotPathBoard : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private RectTransform boardRoot;
        [SerializeField] private DotCellView cellPrefab;
        [SerializeField] private GridLayoutGroup boardGrid;
        [Tooltip("UI Image behind the cells — assign GridBackground.png here.")]
        [SerializeField] private Image gridBackgroundImage;
        [SerializeField] private Sprite gridBackgroundSprite;
        [SerializeField] private float cellSpacing = 4f;
        [SerializeField] private bool useTransparentCellsWithBackground = true;
        #endregion

        #region State
        private DotCellView[,] cells;
        private Color[] pairColors;
        private Sprite[] pairIcons;
        private DotPathState pathState;
        private readonly List<DotCellView> spawned = new List<DotCellView>();
        private bool inputEnabled;
        private bool boardDimmed;
        private bool artBackgroundActive;
        #endregion

        #region Events
        public event Action AllPairsConnected;
        public event Action PathDragStarted;
        #endregion

        #region Public Methods
        public void Build(DotConnectLevel level)
        {
            ClearBoard();
            ApplyGridBackground();

            pathState = new DotPathState(level.GridSize, level.ColorPairs);
            int pairCount = level.ColorPairs.Length;
            pairColors = new Color[pairCount];
            pairIcons = new Sprite[pairCount];

            for (int i = 0; i < pairCount; i++)
            {
                pairColors[i] = level.ColorPairs[i].DisplayColor;
                pairIcons[i] = level.ColorPairs[i].IconSprite;
            }

            if (boardGrid != null)
            {
                boardGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                boardGrid.constraintCount = level.GridSize;
                boardGrid.spacing = new Vector2(cellSpacing, cellSpacing);
                boardGrid.cellSize = CalculateCellSize(level.GridSize);
            }

            int size = level.GridSize;
            cells = new DotCellView[size, size];

            for (int y = size - 1; y >= 0; y--)
            {
                for (int x = 0; x < size; x++)
                {
                    DotCellView cell = Instantiate(cellPrefab, boardRoot);
                    cell.Initialize(x, y, artBackgroundActive);
                    cell.PointerDown += HandlePointerDown;
                    cell.PointerEnter += HandlePointerEnter;
                    cell.PointerUp += HandlePointerUp;
                    cells[x, y] = cell;
                    spawned.Add(cell);
                }
            }

            inputEnabled = false;
            boardDimmed = false;
            RefreshAllVisuals();
        }

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
            if (!enabled && pathState != null && pathState.IsDragging)
            {
                pathState.EndDrag();
            }
        }

        public void SetBoardDimmed(bool dimmed)
        {
            boardDimmed = dimmed;
            if (gridBackgroundImage != null)
            {
                Color c = gridBackgroundImage.color;
                c.a = dimmed ? 0.45f : 1f;
                gridBackgroundImage.color = c;
            }

            RefreshAllVisuals();
        }

        public void ResetPaths()
        {
            if (pathState == null)
            {
                return;
            }

            pathState.ResetAllPaths();
            RefreshAllVisuals();
        }

        public void ClearBoard()
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i] == null)
                {
                    continue;
                }

                spawned[i].PointerDown -= HandlePointerDown;
                spawned[i].PointerEnter -= HandlePointerEnter;
                spawned[i].PointerUp -= HandlePointerUp;
                Destroy(spawned[i].gameObject);
            }

            spawned.Clear();
            pathState = null;
        }
        #endregion

        #region Private Methods
        private void ApplyGridBackground()
        {
            artBackgroundActive = false;
            if (gridBackgroundImage == null)
            {
                return;
            }

            Sprite sprite = gridBackgroundSprite;
            if (sprite == null)
            {
                // Fallback if the sprite field was left empty but the Image already has one
                sprite = gridBackgroundImage.sprite;
            }

            if (sprite != null)
            {
                gridBackgroundImage.sprite = sprite;
                gridBackgroundImage.color = Color.white;
                gridBackgroundImage.preserveAspect = true;
                gridBackgroundImage.enabled = true;
                artBackgroundActive = useTransparentCellsWithBackground;
            }
        }

        private Vector2 CalculateCellSize(int gridSize)
        {
            if (boardRoot == null)
            {
                return new Vector2(64f, 64f);
            }

            float width = boardRoot.rect.width;
            float height = boardRoot.rect.height;
            float gap = cellSpacing * (gridSize - 1);
            float size = Mathf.Min((width - gap) / gridSize, (height - gap) / gridSize);
            return new Vector2(size, size);
        }

        private void HandlePointerDown(DotCellView cell)
        {
            if (!inputEnabled || pathState == null)
            {
                return;
            }

            if (pathState.BeginDrag(cell.Cell))
            {
                PathDragStarted?.Invoke();
                RefreshAllVisuals();
            }
        }

        private void HandlePointerEnter(DotCellView cell)
        {
            if (!inputEnabled || pathState == null || !pathState.IsDragging)
            {
                return;
            }

            pathState.ExtendTo(cell.Cell);
            RefreshAllVisuals();
        }

        private void HandlePointerUp(DotCellView cell)
        {
            if (!inputEnabled || pathState == null)
            {
                return;
            }

            pathState.EndDrag();
            RefreshAllVisuals();

            if (pathState.AreAllPairsConnected())
            {
                AllPairsConnected?.Invoke();
            }
        }

        private void RefreshAllVisuals()
        {
            if (pathState == null)
            {
                return;
            }

            int size = pathState.GridSize;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    RefreshCellVisual(x, y);
                }
            }
        }

        private void RefreshCellVisual(int x, int y)
        {
            DotCellView cell = cells[x, y];
            int endpoint = pathState.GetEndpoint(x, y);
            int owner = pathState.GetOwner(x, y);

            if (endpoint != DotPathState.Empty)
            {
                cell.SetEndpoint(pairColors[endpoint], pairIcons[endpoint]);
            }
            else if (owner != DotPathState.Empty)
            {
                cell.SetPathColor(pairColors[owner]);
            }
            else
            {
                cell.SetEmpty();
            }

            cell.SetDimmed(boardDimmed);
        }
        #endregion
    }
}
