using System.Collections.Generic;
using UnityEngine;

namespace PotanoCloud.MiniGames.DotConnect
{
    /// <summary>
    /// Pure path-state rules: ownership grid, orthogonal extend, no overlapping colors.
    /// </summary>
    public class DotPathState
    {
        #region Constants
        public const int Empty = -1;
        #endregion

        #region State
        private readonly int gridSize;
        private readonly int pairCount;
        private readonly int[,] owners;
        private readonly int[,] endpoints;
        private readonly List<Vector2Int>[] paths;
        private int activeColor = Empty;
        #endregion

        #region Properties
        public int GridSize => gridSize;
        public int PairCount => pairCount;
        public int ActiveColor => activeColor;
        public bool IsDragging => activeColor != Empty;
        #endregion

        public DotPathState(int size, ColorDotPair[] pairs)
        {
            gridSize = size;
            pairCount = pairs.Length;
            owners = new int[size, size];
            endpoints = new int[size, size];
            paths = new List<Vector2Int>[pairCount];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    owners[x, y] = Empty;
                    endpoints[x, y] = Empty;
                }
            }

            for (int i = 0; i < pairCount; i++)
            {
                paths[i] = new List<Vector2Int>();
                PlaceEndpoint(i, pairs[i].StartCell);
                PlaceEndpoint(i, pairs[i].EndCell);
            }
        }

        #region Public Methods
        public int GetOwner(int x, int y) => owners[x, y];

        public int GetEndpoint(int x, int y) => endpoints[x, y];

        public bool BeginDrag(Vector2Int cell)
        {
            int color = endpoints[cell.x, cell.y];
            if (color == Empty)
            {
                color = owners[cell.x, cell.y];
            }

            if (color == Empty)
            {
                return false;
            }

            ClearPathKeepingEndpoints(color);
            activeColor = color;
            paths[color].Clear();
            paths[color].Add(cell);
            owners[cell.x, cell.y] = color;
            return true;
        }

        public void ExtendTo(Vector2Int next)
        {
            if (activeColor == Empty)
            {
                return;
            }

            List<Vector2Int> path = paths[activeColor];
            if (path.Count == 0)
            {
                return;
            }

            Vector2Int last = path[path.Count - 1];
            if (next == last)
            {
                return;
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                if (path[i] == next)
                {
                    TruncatePath(activeColor, i);
                    return;
                }
            }

            if (!IsOrthogonalNeighbor(last, next))
            {
                return;
            }

            int owner = owners[next.x, next.y];
            int endpoint = endpoints[next.x, next.y];
            if (owner != Empty && owner != activeColor)
            {
                return;
            }

            if (endpoint != Empty && endpoint != activeColor)
            {
                return;
            }

            path.Add(next);
            owners[next.x, next.y] = activeColor;
        }

        public void EndDrag()
        {
            activeColor = Empty;
        }

        /// <summary>
        /// Clears all drawn paths but keeps endpoint icons in place.
        /// </summary>
        public void ResetAllPaths()
        {
            activeColor = Empty;
            for (int c = 0; c < pairCount; c++)
            {
                ClearPathKeepingEndpoints(c);
            }
        }

        public bool AreAllPairsConnected()
        {
            for (int c = 0; c < pairCount; c++)
            {
                if (!PathTouchesBothEndpoints(c))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void PlaceEndpoint(int colorId, Vector2Int cell)
        {
            endpoints[cell.x, cell.y] = colorId;
            owners[cell.x, cell.y] = colorId;
        }

        private void TruncatePath(int colorId, int keepThroughIndex)
        {
            List<Vector2Int> path = paths[colorId];
            for (int i = path.Count - 1; i > keepThroughIndex; i--)
            {
                Vector2Int cell = path[i];
                if (endpoints[cell.x, cell.y] != colorId)
                {
                    owners[cell.x, cell.y] = Empty;
                }

                path.RemoveAt(i);
            }
        }

        private void ClearPathKeepingEndpoints(int colorId)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (owners[x, y] == colorId && endpoints[x, y] != colorId)
                    {
                        owners[x, y] = Empty;
                    }
                }
            }

            paths[colorId].Clear();
        }

        private bool PathTouchesBothEndpoints(int colorId)
        {
            int hits = 0;
            List<Vector2Int> path = paths[colorId];
            for (int i = 0; i < path.Count; i++)
            {
                if (endpoints[path[i].x, path[i].y] == colorId)
                {
                    hits++;
                }
            }

            return hits >= 2;
        }

        private static bool IsOrthogonalNeighbor(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
        }
        #endregion
    }
}
