using System;
using UnityEditor;
using UnityEngine;

namespace AlderaminUtils
{
    public  class GridMap<TGridObject>
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private TGridObject[,] _gridArray;
        private TextMesh[,] debugTextArray;
        private Vector3 _originPosition;

        public event EventHandler<GridChangeEventArgs> OnGridMapValueChangeEvent;
        
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

        public void TrigerGridMapValueChange(GridChangeEventArgs arg)
        {
            OnGridMapValueChangeEvent?.Invoke(this, arg);
        }


        public GridMap(int width, int height, float cellSize, Vector3 originPosition, bool showDebug)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            _gridArray = new TGridObject[_width, _height];
            debugTextArray = new TextMesh[width, height];
            if (showDebug)
            {
                for (var x = 0; x < _gridArray.GetLength(0); x++)
                {
                    for (var y = 0; y < _gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = Tool.CreateWorldText(_gridArray[x, y].ToString(), null,
                            GetWorldPosition(x, y) + .5f * new Vector3(cellSize, cellSize), 20,
                            Color.white,
                            TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }

                    Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), Color.white, 100f);
                }
            }
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y, 0) * _cellSize + _originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public void SetValue(int x, int y, TGridObject gridObject)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = gridObject;
                debugTextArray[x, y].text = _gridArray[x, y].ToString();
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

        public Vector2 GetXY(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            var y = Mathf.FloorToInt(worldPosition.y / _cellSize);
            return new Vector2(x, y);
        }
    }
}