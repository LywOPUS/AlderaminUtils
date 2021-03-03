using System;
using UnityEngine;

namespace AlderaminUtils
{
    [Serializable]
    public class Grid2D<TGridObject>
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private TGridObject[,] _gridArray;
        private TextMesh[,] _debugTextArray;
        private Vector3 _originPosition;
        public int GetHeight() => _height;
        public int GetWidth() => _width;
        public Color color = Color.white;
        public event EventHandler<GridChangeEventArgs> OnGridMapValueChangeEvent;

        public void TriggerGridMapValueChangeEvent(int x, int y)
        {
            var arg = new GridChangeEventArgs(x, y);
            OnGridMapValueChangeEvent?.Invoke(this, arg);
        }

        public class GridChangeEventArgs : EventArgs
        {
            public int x;
            public int y;

            public GridChangeEventArgs(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }


        public Grid2D(int width, int height, float cellSize, Vector3 originPosition,
            Func<Grid2D<TGridObject>, int, int, TGridObject> creatGridObject, bool showDebug)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            _gridArray = new TGridObject[_width, _height];
            _debugTextArray = new TextMesh[width, height];
            for (var x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (var y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = creatGridObject(this, x, y);
                    if (showDebug)
                    {
                        _debugTextArray[x, y] = Tool.CreateWorldText(_gridArray[x, y].ToString(), null,
                            GetWorldPosition(x, y) + .5f * new Vector3(cellSize, cellSize), 20,
                            color,
                            TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), color, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), color, 100f);
                    }
                }

                if (showDebug)
                {
                    Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), color, 100f);
                    Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), color, 100f);

                    OnGridMapValueChangeEvent += (sender, eventArgs) =>
                    {
                        _debugTextArray[eventArgs.x, eventArgs.y].text =
                            _gridArray[eventArgs.x, eventArgs.y].ToString();
                    };
                }
            }
        }

        public Vector3 Cell2WorldPos(int x, int y)
        {
            var pos = GetWorldPosition(x, y);
            return pos + .5f * new Vector3(_cellSize, _cellSize);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y, 0) * _cellSize + _originPosition;
        }

        public void SetValue(int x, int y, TGridObject gridObject)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = gridObject;
                OnGridMapValueChangeEvent?.Invoke(this, new GridChangeEventArgs(x, y));
            }
            else
            {
                Debug.Log("out of grid map");
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject gridObject)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, gridObject);
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public Vector2 GetXY(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            var y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
            return new Vector2(x, y);
        }

        public TGridObject GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                Debug.LogWarning("out of range");
                return default;
            }
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }
    }
}