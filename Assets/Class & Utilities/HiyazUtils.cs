using System;
using UnityEngine;
namespace Hiyazcool {
    /*
     * Refactor Grid to be a struct to be threadsafe and hopeful burst Compatible
     * Make Mesh Utils on my own, more suitable with my own plans
     */
    namespace Meshes // Refactor This Eventually
    {
        public class TextMeshUtils : UnityEngine.Object {
        public const int SORTING_ORDER_DEFAULT = 5000;
        public static TextMesh CreateWorldText(string _text, Transform _parent = null, Vector3 _localPosition = default(Vector3), int _fontSize = 40, Color? _color = null, TextAnchor _textAnchor = TextAnchor.UpperLeft, TextAlignment _textAlignment = TextAlignment.Left, int _sortingOrder = SORTING_ORDER_DEFAULT) {
            if (_color == null)
                _color = Color.white;
            return CreateWorldText(_parent, _text, _localPosition, _fontSize, (Color)_color, _textAnchor, _textAlignment, _sortingOrder);
        }
        public static TextMesh CreateWorldText(Transform _parent, string _text, Vector3 _localPosition, int _fontSize, Color _color, TextAnchor _textAnchor, TextAlignment _textAlignment, int _sortingOrder) {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(_parent, false);
            transform.localPosition = _localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = _textAnchor;
            textMesh.alignment = _textAlignment;
            textMesh.text = _text;
            textMesh.fontSize = _fontSize;
            textMesh.color = _color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = _sortingOrder;
            return textMesh;
            }
        }
    }
    public static class MeshUtils {

        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0, -1);


        private static Quaternion[] cachedQuaternionEulerArr;
        private static void CacheQuaternionEuler() {
            if (cachedQuaternionEulerArr != null)
                return;
            cachedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < 360; i++) {
                cachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
        private static Quaternion GetQuaternionEuler(float _rotFloat) {
            int rot = Mathf.RoundToInt(_rotFloat);
            rot = rot % 360;
            if (rot < 0)
                rot += 360;
            //if (rot >= 360) rot -= 360;
            if (cachedQuaternionEulerArr == null)
                CacheQuaternionEuler();
            return cachedQuaternionEulerArr[rot];
        }


        public static Mesh CreateEmptyMesh() {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[0];
            mesh.uv = new Vector2[0];
            mesh.triangles = new int[0];
            return mesh;
        }

        public static void CreateEmptyMeshArrays(int _quadCount, out Vector3[] _vertices, out Vector2[] _uvs, out int[] _triangles) {
            _vertices = new Vector3[4 * _quadCount];
            _uvs = new Vector2[4 * _quadCount];
            _triangles = new int[6 * _quadCount];
        }
        public static Mesh CreateMesh(Vector3 _pos, float _rot, Vector3 _baseSize, Vector2 _uv00, Vector2 _uv11) {
            return AddToMesh(null, _pos, _rot, _baseSize, _uv00, _uv11);
        }

        public static Mesh AddToMesh(Mesh _mesh, Vector3 _pos, float _rot, Vector3 _baseSize, Vector2 _uv00, Vector2 _uv11) {
            if (_mesh == null) {
                _mesh = CreateEmptyMesh();
            }
            Vector3[] vertices = new Vector3[4 + _mesh.vertices.Length];
            Vector2[] uvs = new Vector2[4 + _mesh.uv.Length];
            int[] triangles = new int[6 + _mesh.triangles.Length];

            _mesh.vertices.CopyTo(vertices, 0);
            _mesh.uv.CopyTo(uvs, 0);
            _mesh.triangles.CopyTo(triangles, 0);

            int index = vertices.Length / 4 - 1;
            //Relocate vertices
            int vIndex = index * 4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            _baseSize *= .5f;

            bool skewed = _baseSize.x != _baseSize.y;
            if (skewed) {
                vertices[vIndex0] = _pos + GetQuaternionEuler(_rot) * new Vector3(-_baseSize.x, _baseSize.y);
                vertices[vIndex1] = _pos + GetQuaternionEuler(_rot) * new Vector3(-_baseSize.x, -_baseSize.y);
                vertices[vIndex2] = _pos + GetQuaternionEuler(_rot) * new Vector3(_baseSize.x, -_baseSize.y);
                vertices[vIndex3] = _pos + GetQuaternionEuler(_rot) * _baseSize;
            }
            else {
                vertices[vIndex0] = _pos + GetQuaternionEuler(_rot - 270) * _baseSize;
                vertices[vIndex1] = _pos + GetQuaternionEuler(_rot - 180) * _baseSize;
                vertices[vIndex2] = _pos + GetQuaternionEuler(_rot - 90) * _baseSize;
                vertices[vIndex3] = _pos + GetQuaternionEuler(_rot - 0) * _baseSize;
            }

            //Relocate UVs
            uvs[vIndex0] = new Vector2(_uv00.x, _uv11.y);
            uvs[vIndex1] = new Vector2(_uv00.x, _uv00.y);
            uvs[vIndex2] = new Vector2(_uv11.x, _uv00.y);
            uvs[vIndex3] = new Vector2(_uv11.x, _uv11.y);

            //Create triangles
            int tIndex = index * 6;

            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex3;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex3;
            triangles[tIndex + 5] = vIndex2;

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uvs;

            //mesh.bounds = bounds;

            return _mesh;
        }

        public static void AddToMeshArrays(Vector3[] _vertices, Vector2[] _uvs, int[] _triangles, int _index, Vector3 _pos, float _rot, Vector3 _baseSize, Vector2 _uv00, Vector2 _uv11) {
            //Relocate vertices
            int vIndex = _index * 4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            _baseSize *= .5f;

            bool skewed = _baseSize.x != _baseSize.y;
            if (skewed) {
                _vertices[vIndex0] = _pos + GetQuaternionEuler(_rot) * new Vector3(-_baseSize.x, _baseSize.y);
                _vertices[vIndex1] = _pos + GetQuaternionEuler(_rot) * new Vector3(-_baseSize.x, -_baseSize.y);
                _vertices[vIndex2] = _pos + GetQuaternionEuler(_rot) * new Vector3(_baseSize.x, -_baseSize.y);
                _vertices[vIndex3] = _pos + GetQuaternionEuler(_rot) * _baseSize;
            }
            else {
                _vertices[vIndex0] = _pos + GetQuaternionEuler(_rot - 270) * _baseSize;
                _vertices[vIndex1] = _pos + GetQuaternionEuler(_rot - 180) * _baseSize;
                _vertices[vIndex2] = _pos + GetQuaternionEuler(_rot - 90) * _baseSize;
                _vertices[vIndex3] = _pos + GetQuaternionEuler(_rot - 0) * _baseSize;
            }

            //Relocate UVs
            _uvs[vIndex0] = new Vector2(_uv00.x, _uv11.y);
            _uvs[vIndex1] = new Vector2(_uv00.x, _uv00.y);
            _uvs[vIndex2] = new Vector2(_uv11.x, _uv00.y);
            _uvs[vIndex3] = new Vector2(_uv11.x, _uv11.y);

            //Create triangles
            int tIndex = _index * 6;

            _triangles[tIndex + 0] = vIndex0;
            _triangles[tIndex + 1] = vIndex3;
            _triangles[tIndex + 2] = vIndex1;

            _triangles[tIndex + 3] = vIndex1;
            _triangles[tIndex + 4] = vIndex3;
            _triangles[tIndex + 5] = vIndex2;
        }
    }
}
