using Hiyazcool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]
public class CList<T> : IList<T>, IReadOnlyCollection<T> {
    [SerializeReference]
    internal List<T> list = new List<T>();
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

    public int Count => list.Count;

    public bool IsReadOnly => false;
    public CList() {
        list = new List<T>();
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

    public IEnumerator<T> GetEnumerator() {
        return list.GetEnumerator();
    }

    public int IndexOf(T item) {
        return list.IndexOf(item);
    }

    public void Insert(int index, T item) {
        list.Insert(index, item);
    }

    public bool Remove(T item) {
        return list.Remove(item);
    }

    public void RemoveAt(int index) {
        list.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return list.GetEnumerator();
    }

}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CList<>), true)]
public class InheritenceListPropertyDrawer : PropertyDrawer {
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        Foldout element = UIToolkitUtils.CreatePropertyFoldout(property);
        SerializedProperty listElement = property.FindPropertyRelative("list");
        Label textLabel = new Label {
            text = "Insert Class Name Here and Press Enter",
            tooltip = "Insert a Derived Class Name here and Press Enter, Invalid Values will Throw Errors"
        };
        TextField textField = new TextField {
            tooltip = "Insert a Derived Class Name here and Press Enter, Invalid Values will Throw Errors"
        };
        element.Add(textLabel);
        element.Add(textField);
        textField.RegisterCallback<KeyDownEvent>((args) => {
            if (args.keyCode == KeyCode.Return) {
                listElement = property.FindPropertyRelative("list");
                Type type = Type.GetType(textField.value);                
                var inputClass = Activator.CreateInstance(type);
                listElement.InsertArrayElementAtIndex(listElement.arraySize);
                listElement.GetArrayElementAtIndex(listElement.arraySize - 1).managedReferenceValue = inputClass;
                textField.value = "";
                UIToolkitUtils.AddListElement(element, listElement, listElement.arraySize - 1);
                listElement.serializedObject.ApplyModifiedProperties();
                element.MarkDirtyRepaint();
                element.Bind(property.serializedObject);
            }
        }, TrickleDown.TrickleDown);
        if (listElement?.arraySize > 0)
        UIToolkitUtils.PopulateElementFromList(
            element,
            listElement
        );
        return element;
    }

}
#endif
