using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hiyazcool
{
    public static class UIToolkitUtils {
        /*  Todo
         *      Finalize the Statics
         *      Clean up InheritenceList into Maybe ClassList?
         *      UniqueList
         *      make an Attribute that can be Applied to other list?
         *      Attribute Tied to the Class that Takes in the Attributes that is wanted ???????????
         *      [List(List2,List5, etc)]
         *      public class something
         * 
         */
        public static Foldout CreatePropertyFoldout(SerializedProperty property) {
            Foldout propertyFoldout = new Foldout() {
                text = property.displayName,
                tooltip = property.tooltip,
            };
            return propertyFoldout;
        }
        public static VisualElement AddElementFromProperty(VisualElement element, SerializedProperty property) {
            element.Add(
                new PropertyField(
                        property
                    ));
            return element;
        }
        public static VisualElement AddElementAndNameFromProperty(VisualElement element, SerializedProperty property) {
            element.Add(
                new PropertyField(
                property,
                property.displayName
                    ));
            return element;
        }
        public static PropertyField CreatePropertyField(SerializedProperty property) {
            return 
                new PropertyField(
                property,
                property.displayName
                    );
        }
        public static VisualElement PopulateElementFromList(VisualElement element, SerializedProperty property) {
            for (int i = 0; i < property.arraySize; i++)
                AddListElement(element, property, i);
            return element;
        }
        public static Foldout CreateFoldoutFromList(SerializedProperty property) {
            if (!property.isArray)
                throw new ArgumentException("Property is not an array!");
            Foldout propertyFoldout = CreatePropertyFoldout(property);
            foreach (SerializedProperty child in property)
                AddElementFromProperty(propertyFoldout, child);
            return propertyFoldout;
        }
        public static VisualElement AddListElement(VisualElement element, SerializedProperty property, int index) {
            SerializedProperty item = property.GetArrayElementAtIndex(index);
            PropertyField field = new PropertyField(item);
            field.AddManipulator(new ContextualMenuManipulator((evt) => {
                evt.menu.AppendAction("Delete Array Item : " + item.displayName, (x) => {
                    element.Remove(field);
                    property.DeleteArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();
                },
                    DropdownMenuAction.AlwaysEnabled);
                evt.menu.AppendAction("Duplicate Array Item : " + item.displayName, (x) => {
                    DuplicateProperty(property, index);
                    AddListElement(element, property, property.arraySize - 1);
                    property.serializedObject.ApplyModifiedProperties();
                    element.MarkDirtyRepaint();
                    element.Bind(property.serializedObject);
                },
                    DropdownMenuAction.AlwaysEnabled);
            }));
            element.Add(field);
            return element;
        }
        public static SerializedProperty DuplicateProperty(SerializedProperty property, Type type) {
            property.InsertArrayElementAtIndex(property.arraySize);
            var Object = Activator.CreateInstance(type);
            property.GetArrayElementAtIndex(property.arraySize - 1).managedReferenceValue = Object;
            return property;
        }
        public static SerializedProperty DuplicateProperty(SerializedProperty property, int index) => DuplicateProperty(property, property.GetArrayElementAtIndex(index).managedReferenceValue.GetType());            
    }
}
