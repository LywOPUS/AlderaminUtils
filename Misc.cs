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
        public static Vector3 GetWorldMousePosition()
        {
            var mousePos = Input.mousePosition;
            if (Camera.main is null) return default;
            mousePos.z = Camera.main.nearClipPlane;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        /// <summary>
        /// 获取鼠标在世界空间的位置
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector3 GetWorldMousePosition(float distance)
        {
            if (Camera.main is null) return default;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hit, distance) ? hit.point : default;
        }

        /// <summary>
        ///  随机一个数组中的元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        public static void RandomArray(int[] array, int length)
        {
            int index;
            int value;
            for (int i = length - 1; i > 0; i--)
            {
                index = Random.Range(0, i + 1);
                value = array[i];
                array[i] = array[index];
                array[index] = value;
            }
        }
    }
}