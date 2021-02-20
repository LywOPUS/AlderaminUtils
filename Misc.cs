using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlderaminUtils
{
    public static class Tool
    {
        /// <summary>
        /// 在世界空间创建文字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parent"></param>
        /// <param name="localPosition"></param>
        /// <param name="fontsize"></param>
        /// <param name="color"></param>
        /// <param name="textAnchor"></param>
        /// <param name="sortingOrder"></param>
        /// <returns></returns>
        public static TextMesh CreateWorldText(string text, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontsize = 40,
            Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter
            , int sortingOrder = 5000
        )
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontsize, color, textAnchor, sortingOrder);
        }

        /// <summary>
        /// 在世界空间创建文字
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="text"></param>
        /// <param name="localPosition"></param>
        /// <param name="fontsize"></param>
        /// <param name="color"></param>
        /// <param name="textAnchor"></param>
        /// <param name="sortingOrder"></param>
        /// <returns></returns>
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontsize,
            Color color, TextAnchor textAnchor
            , int sortingOrder
        )
        {
            var gameObject = new GameObject("world_text", typeof(TextMesh));
            var transform = gameObject.transform;
            var textMesh = gameObject.GetComponent<TextMesh>();
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            textMesh.anchor = textAnchor;
            textMesh.text = text;
            textMesh.fontSize = fontsize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        /// <summary>
        /// 获取鼠标在世界中的位置
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            var mousePos = Input.mousePosition;
            if (Camera.main is null) return default;
            mousePos.z = Camera.main.nearClipPlane;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        /// <summary>
        /// 获取鼠标在世界空间的位置
        /// </summary>
        /// <param name="distance">从摄像机出发的射线的预期距离</param>
        /// <returns>鼠标击中物体的位置</returns>
        public static RaycastHit GetMouseHitInfoInWorld(float distance, bool showDebugLine)
        {
            if (Camera.main is null) return default;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, distance)) return hit;
            if (showDebugLine) Debug.DrawLine(ray.origin, hit.point, Color.red);
            return hit;
        }


        // 可以使用（a,b) = (b,a)交换值类型或引用类型 tuples需要c#7.0
        //
        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        public static void Swap<T>(ref T a, ref T b, ref T c)
        {
            var temp = a;
            a = b;
            b = c;
            c = temp;
        }

        public static void Swap<T>(ref T a, ref T b, ref T c, ref T d)
        {
            var temp = a;
            a = b;
            b = c;
            c = d;
            d = temp;
        }

        public static void ForeachFourDirection<T>(int x, int y, T[,] tragetArray)
        {
            var offX = new int[4] {1, 0, -1, 0};
            var offY = new int[4] {0, 1, -1, 0};
            for (var i = 0; i < 4; i++)
            {
                x += offX[i];
                y += offY[i];
                var r = tragetArray[x, y];
                Debug.Log(r);
            }
        }

        /// <summary>
        /// 当目标数组为为私有时
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="getArrayObjectFunc"></param>
        /// <typeparam name="T"></typeparam>
        public static void ForeachFourDirection<T>(int x, int y, Func<int, int, T> getArrayObjectFunc)
        {
            var dX = new int[4] {1, 0, -1, 0};
            var dY = new int[4] {0, 1, 0, -1};
            for (var i = 0; i < 4; i++)
            {
                var offX = x + dX[i];
                var offY = y + dY[i];
                var r = getArrayObjectFunc(offX, offY);
                Debug.Log($"{r} : {dX[i]},{dY[i]}");
            }
        }

        public static List<T> GetNeighbors4<T>(int x, int y, int width, int height, Func<int, int, T> getArrayObjectFunc)
        {
            var list = new List<T>();
            var dX = new int[4] {1, 0, -1, 0};
            var dY = new int[4] {0, 1, 0, -1};
            for (var i = 0; i < 4; i++)
            {
                var offX = x + dX[i];
                var offY = y + dY[i];
                if (offX >= 0 && offX < width && offY >= 0 && offY < height)
                {
                    list.Add(getArrayObjectFunc(offX, offY));
                }
            }

            return list;
        }

        public static List<T> GetNeighbors<T>(int x, int y, int width,
            int height, Func<int, int, T> getArrayObjectFunc)
        {
            var list = new List<T>();
            var dX = new int[8] {1, 1, 0, -1, -1, -1, 0, 1};
            var dY = new int[8] {0, 1, 1, 1, 0, -1, -1, -1};
            for (var i = 0; i < 8; i++)
            {
                var offX = x + dX[i];
                var offY = y + dY[i];
                if (offX >= 0 && offX < width && offY >= 0 && offY < height)
                {
                    list.Add(getArrayObjectFunc(offX, offY));
                }
            }

            return list;
        }
    }
}