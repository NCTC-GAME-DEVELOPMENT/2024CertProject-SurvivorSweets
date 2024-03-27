using Hiyazcool.Meshes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hiyazcool {
    namespace Collections {
        /* need To Seperate out Files
         * 
         */


        #region normal

        /// <summary>
        /// A floating point Number with Min - Max value of 1.00 to -1.00
        /// </summary>
        [Serializable]
        public struct normal {
            private sbyte _value;
            public static implicit operator float(normal self) {
                return self._value * 0.01f;
            }
        }
        #endregion
        #region Grid
        /*  Need to Refactor this Eventually
         *  Mostly from CodeMonkey, can be improved upon
         */
        [Serializable]
        public class OldGrid<TGridObject> {
            public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
            public class OnGridValueChangedEventArgs : EventArgs {
                public int x;
                public int y;
            }
            private readonly int width;
            private readonly int height;
            private readonly float cellSize;
            private Vector3 originPosition;
            private readonly TGridObject[,] gridArray;
            public OldGrid(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<OldGrid<TGridObject>, int, int, TGridObject> _createGridObject, bool _showDebug = false) {
                width = _width;
                height = _height;
                cellSize = _cellSize;
                originPosition = _originPosition;

                gridArray = new TGridObject[_width, _height];

                for (int x = 0; x < gridArray.GetLength(0); x++) {
                    for (int y = 0; y < gridArray.GetLength(1); y++) {
                        gridArray[x, y] = _createGridObject(this, x, y);
                    }
                }
#if UNITY_EDITOR
                if (_showDebug) {
                    TextMesh[,] debugTextArray = new TextMesh[_width, _height];

                    for (int x = 0; x < gridArray.GetLength(0); x++) {
                        for (int y = 0; y < gridArray.GetLength(1); y++) {
                            debugTextArray[x, y] = TextMeshUtils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + (new Vector3(_cellSize, _cellSize) * .5f), 30, Color.white, TextAnchor.MiddleCenter);
                            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        }
                    }
                    Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
                    OnGridValueChanged += (object _sender, OnGridValueChangedEventArgs _eventArgs) => {
                        debugTextArray[_eventArgs.x, _eventArgs.y].text = gridArray[_eventArgs.x, _eventArgs.y]?.ToString();
                    };
                }
#endif
            }
            public int GetWidth => width;
            public int GetHeight => height;
            public float GetCellsize => cellSize;
            public Vector3 GetWorldPosition(int x, int y) {
                return (new Vector3(x, y) * cellSize) + originPosition;
            }
            public void GetXY(Vector3 _worldPosition, out int _x, out int _y) {
                _x = Mathf.FloorToInt((_worldPosition - originPosition).x / cellSize);
                _y = Mathf.FloorToInt((_worldPosition - originPosition).y / cellSize);
            }
            public void SetGridValue(int _x, int _y, TGridObject _value) {
                if (_x >= 0 && _y >= 0 && _x < width && _y < height) {
                    gridArray[_x, _y] = _value;
                    OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = _x, y = _y });
                }
            }
            public void TriggerGridValueChanged(int _x, int _y) {
                OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = _x, y = _y });
            }
            public void SetGridValue(Vector3 _worldPosition, TGridObject _value) {
                GetXY(_worldPosition, out int x, out int y);
                SetGridValue(x, y, _value);
            }
            public TGridObject GetGridValue(int _x, int _y) {
                return _x >= 0 && _y >= 0 && _x < width && _y < height ? gridArray[_x, _y] : default;
            }
            public TGridObject GetGridValue(Vector3 _worldPosition) {
                GetXY(_worldPosition, out int x, out int y);
                return GetGridValue(x, y);
            }
        }
        [Serializable]
        public class Grid<T> : IEnumerable<T> {
            public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
            public class OnGridValueChangedEventArgs : EventArgs {
                public int x;
                public int y;
            }
            private readonly int width;
            private readonly int height;
            private readonly float cellSize;
            private float3 originPosition;
            [SerializeField]
            private bool showDedug;
            [SerializeField]
            private T[,] gridArray;
            public T this[int x,int y] {
                get => CheckBounds(x,y) ? gridArray[x,y] : default;

                set {
                    if (CheckBounds(x, y)) {
                        gridArray[x, y] = value;
                        OnGridValueChanged.Invoke(
                            this,
                            new OnGridValueChangedEventArgs {
                                x = x,
                                y = y
                            });
                    }
                }
            }
            public T this[int2 int2] {
                get => CheckBounds(int2) ? gridArray[int2.x, int2.y] : default;
                set {
                    if (CheckBounds(int2)) {
                        gridArray[int2.x, int2.y] = value;
                        OnGridValueChanged.Invoke(
                            this,
                            new OnGridValueChangedEventArgs {
                                x = int2.x,
                                y = int2.y
                            });
                    }
                }
            }
            public T this[Vector3 vector3] {
                get => this[GetXY(vector3)];
                set => this[GetXY(vector3)] = value;
            }
            public T this[float3 float3] {
                get => this[GetXY(float3)];
                set => this[GetXY(float3)] = value;
            }
            public int GetWidth => width;
            public int GetHeight => height;
            public float GetCellsize => cellSize;
            public Grid(int width, int height, float cellSize, float3 originPosition) {
                this.width = width;
                this.height = height;
                this.cellSize = cellSize;
                this.originPosition = originPosition;

                gridArray = new T[width, height];
                PopulateGrid(ref gridArray);
                #region Debug Draws
#if UNITY_EDITOR
                if (showDedug) {
                    TextMesh[,] debugTextArray = new TextMesh[width, height];

                    for (int x = 0; x < gridArray.GetLength(0); x++) {
                        for (int y = 0; y < gridArray.GetLength(1); y++) {
                            debugTextArray[x, y] = TextMeshUtils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetTheoreticalWorldPosition(x, y) + (new float3(cellSize, cellSize, 0) * .5f), 10, Color.white, TextAnchor.MiddleCenter);
                            Debug.DrawLine(GetWorldPosition(x, y), GetTheoreticalWorldPosition(x, y + 1), Color.white, 100f);
                            Debug.DrawLine(GetWorldPosition(x, y), GetTheoreticalWorldPosition(x + 1, y), Color.white, 100f);
                        }
                    }
                    Debug.DrawLine(GetTheoreticalWorldPosition(0, height), GetTheoreticalWorldPosition(width, height), Color.white, 100f);
                    Debug.DrawLine(GetTheoreticalWorldPosition(width, 0), GetTheoreticalWorldPosition(width, height), Color.white, 100f);
                    OnGridValueChanged += (object _sender, OnGridValueChangedEventArgs _eventArgs) => {
                        debugTextArray[_eventArgs.x, _eventArgs.y].text = gridArray[_eventArgs.x, _eventArgs.y]?.ToString();
                    };
                }
#endif
                #endregion
            }
            public void TriggerGridValueChanged(int x, int y) 
                => OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });          
            #region Coord Conversions
            public float3 GetTheoreticalWorldPosition(int x, int y)
                => (new float3(x, y, 0) * cellSize) + originPosition;
            public float3 GetTheoreticalWorldPosition(int2 int2)
                =>  (new float3(int2, 0) * cellSize) + originPosition;
            public float3 GetWorldPosition(int x, int y) 
                => CheckBounds(x,y) ? (new float3(x, y, 0) * cellSize) + originPosition : default;
            public float3 GetWorldPosition(int2 int2)
                => CheckBounds(int2) ? (new float3(int2, 0) * cellSize) + originPosition : default;
            public void GetXY(float3 worldPosition, out int x, out int y) {
                int2 temp = GetXY(worldPosition);
                x = temp.x;
                y = temp.y;
            }
            public int2 GetXY(float3 worldPosition) {
            int2 temp = new int2(
                    Mathf.FloorToInt((worldPosition - originPosition).x / cellSize),
                    Mathf.FloorToInt((worldPosition - originPosition).x / cellSize));
                return CheckBounds(temp) ? temp : InvalidInt2;
            }
            public void GetXY(float3 worldPosition, out int2 int2) {
                int2 = GetXY(worldPosition);
            }
            public int2 GetXY(Vector3 worldPosition)
                => GetXY((float3) worldPosition);
            public void GetXY(Vector3 worldPosition, out int x, out int y)
                => GetXY((float3) worldPosition, out x, out y);
            public void GetXY(Vector3 worldPosition, out int2 int2)
                => GetXY((float3) worldPosition, out int2);
            #endregion
            #region Range Checks
            public bool CheckBounds(int x, int y) {
                if (x < 0 || y < 0) {
                    ThrowHelper.ArgumentOutOfRange(ThrowHelper.ExceptionArgument.grid, ThrowHelper.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                    return false;
                }
                if (x > width - 1 || y > height - 1) {
                    ThrowHelper.ArgumentOutOfRange(ThrowHelper.ExceptionArgument.grid, ThrowHelper.ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
                    return false;
                }
                return true;
            }
            public bool CheckBounds(int2 int2) 
                => CheckBounds(int2.x, int2.y);
            #endregion
            #region Static
            protected static int2 InvalidInt2 => new int2(-1,-1);
            protected static T[,] CreateEmptyGrid(int2 int2) {
                return CreateEmptyGrid(int2.x, int2.y);
            }
            protected static T[,] CreateEmptyGrid(int x, int y) {
                T[,] grid = new T[x,y];
                return grid;
            }
            protected static void PopulateGrid(ref T[,] grid) {
                for (int i = 0; i < grid.GetLength(0); i++) {
                    for (int j = 0; j < grid.GetLength(1); j++) {
                        grid[i,j] = default;
                    }
                }
            }
            #endregion
            public IEnumerator<T> GetEnumerator() {
                throw new NotImplementedException();
            }
            IEnumerator IEnumerable.GetEnumerator() {
                throw new NotImplementedException();
            }
        }


        #endregion
        #region Cycleable List 
        /*  Need to Add Comments/Summaries
         *  Maybe and a Shuffle to give a random item, just because
         *      Resources to look at
         *      - https://docs.unity3d.com/Manual/UIElements.html
         *      - https://docs.unity3d.com/Manual/UIE-Binding.html
         *      - https://docs.unity3d.com/Manual/UIE-bind-to-list.html
         *      - https://docs.unity3d.com/Manual/UIE-ListView-TreeView.html
         *      - https://docs.unity3d.com/Manual/UIE-uxml-element-ListView.html
         *      
         *          Microsoft List Code
         *      - https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,cf7f4095e4de7646
         */
        /// <summary>
        /// Edited copy of System.Collections.List&lt;&gt; to allow Inspector Serialization of classes derived of System.Collections.List&lt;&gt;.
        /// </summary>
        /// <typeparam Type="T"></typeparam>
        [Serializable]
        public class Basic_List<T> : IList<T>, IReadOnlyList<T> {
            private static readonly T[] emptyArray = new T[0];
            private const int DEFAULT_CAPACITY = 4;
            protected T[] values;
            [ContractPublicPropertyName("Count")]
            private readonly int size;
            public T this[int _index] {
                get => values[_index];
                set {
                    if (value is null) {
                        values[_index] = default;
                    } else {
                        this[_index] = value;
                    }
                }
            }
            public int Count {
                get {
                    Contract.Ensures(Contract.Result<int>() >= 0);
                    return size;
                }
            }
            public bool IsReadOnly => values.IsReadOnly;
            int IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>) values).Count;
            public void Add(T item) {
                if (size == values.Length) {
                }

                //values.Add(item);
            }
            public void Clear() {
                ((ICollection<T>) values).Clear();
            }
            public bool Contains(T item) {
                return ((ICollection<T>) values).Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex) {
                ((ICollection<T>) values).CopyTo(array, arrayIndex);
            }
            public IEnumerator<T> GetEnumerator() {
                return ((IEnumerable<T>) values).GetEnumerator();
            }
            public int IndexOf(T item) {
                return ((IList<T>) values).IndexOf(item);
            }
            public void Insert(int index, T item) {
                ((IList<T>) values).Insert(index, item);
            }
            public bool Remove(T item) {
                return ((ICollection<T>) values).Remove(item);
            }
            public void RemoveAt(int index) {
                ((IList<T>) values).RemoveAt(index);
            }
            IEnumerator IEnumerable.GetEnumerator() {
                return values.GetEnumerator();
            }
        }
        /// <summary>
        /// A List with a Index that tracks the current selected Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public class CycleableList<T> : IList<T> {
            [SerializeField]
            private List<T> list;
            private int index;
            public int Index {
                get => index;
                set => index = value > list.Count - 1 ? 0 : value < 0 ? list.Count - 1 : value;
            }
            public T Current {
                get => this[index];
                set {
                    if (value is null) {
                        list.RemoveAt(index);
                    } else {
                        this[index] = value;
                    }
                }
            }
            public CycleableList() {
                list = new List<T>();
            }
            public List<T> List {
                get => list;
                set => list = value;
            }

            public int Count => list.Count;

            public bool IsReadOnly => ((ICollection<T>) list).IsReadOnly;

            public T this[int index] {
                get => list[index];
                set {
                    if (value is null) {
                        list.RemoveAt(index);
                    } else {
                        this[index] = value;
                    }
                }
            }
            /// <summary>
            /// Increases the index by one, looping to the begining when reachingthe end of the list, and returns the newly selected object.
            /// </summary>
            /// <returns>Newly Selected Item</returns>
            public T Next() {
                return this[++Index];
            }
            /// <summary>
            /// Decreases the index by one, looping tho the end when reaching the begining of the list, and returns the newly selected object.
            /// </summary>
            /// <returns>Newly Selected Item</returns>
            public T Previous() {
                return this[--Index];
            }

            public List<T> ToList() {
                return new List<T>(list);
            }

            public int IndexOf(T item) {
                return list.IndexOf(item);
            }

            public void Insert(int index, T item) {
                list.Insert(index, item);
            }

            public void RemoveAt(int index) {
                list.RemoveAt(index);
            }

            public void Add(T item) {
                list.Add(item);
            }

            public void Clear() {
                list.Clear();
            }

            public bool Contains(T item) {
                return list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex) {
                list.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item) {
                return list.Remove(item);
            }

            public IEnumerator<T> GetEnumerator() {
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return list.GetEnumerator();
            }
        }
        #region Editor
#if UNITY_EDITOR
        /*  Section to Make the Inspector look Nicer
         *  Need To research more
         *  Create This to use an Object View 
         *  https://docs.unity3d.com/Manual/UIE-ElementRef.html
         */
        [CustomPropertyDrawer(typeof(CycleableList<>), true)]
        // [CustomPropertyDrawer(typeof(List<>), true)]
        public class CycleableListDrawerUIE : PropertyDrawer {
            public override VisualElement CreatePropertyGUI(SerializedProperty property) {
                Foldout foldout = UIToolkitUtils.CreatePropertyFoldout(property);
                SerializedProperty listProperty = property.FindPropertyRelative("list");
                Label selection;
                List<PropertyField> listChildren = new List<PropertyField>();
                if (listProperty?.arraySize > 0) {
                    selection = new Label {
                        text =
                       "Current Selection: " +
                           listProperty.GetArrayElementAtIndex(
                               0).name
                    };
                    foreach (SerializedProperty item in listProperty) {
                        
                        listChildren.Add(UIToolkitUtils.CreatePropertyField(item));
                    }
                    Debug.Log("Array is not Empty");
                } else {
                    selection = new Label {
                        text = " Current Selection : NULL"
                    };
                    Debug.Log("Array is Empty");
                }
                foldout.Add(selection);
                ScrollView listView = new ScrollView(ScrollViewMode.Vertical);
                GroupBox groupBox =  new GroupBox("Test Label");
                foreach (PropertyField item in listChildren) {
                    listView.Add(item);
                    item.tooltip = "Something";
                    //foldout.Add(item);
                    //groupBox.Add(item);
                }
                foldout.Add(listView);
                return foldout;
                //foldout.Add(groupBox);
                //var radiogroup = new RadioButtonGroup("Options", new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" });
                //radiogroup.RegisterValueChangedCallback(evt => Debug.Log(evt.newValue));
                //var toolbarButton = new ToolbarButton(() => { Debug.Log("Button clicked"); }) { text = "Click me" };
                //foldout.Add(radiogroup);
                //var toolbar = new Toolbar();
                //var button = new ToolbarButton(() => { Debug.Log("Button clicked2"); }) { text = "Click me2" };
                //toolbar.Add(button);
                //toolbar.Add(toolbarButton);
                //toolbar.Add(new ToolbarToggle() { text = "Toggle me" });
                //foldout.Add(toolbar);

                
                //Func<VisualElement> makeItem = () => new PropertyField();
                //Action<VisualElement, int> bindItem = (element, i) => { element.tooltip = listChildren[i].tooltip; };
                //ListView listView = new ListView(listChildren, 40 ,makeItem,bindItem);

                //foldout.Add(base.CreatePropertyGUI(property));

            }
        }
#endif
        #endregion
        #endregion
    }
}
