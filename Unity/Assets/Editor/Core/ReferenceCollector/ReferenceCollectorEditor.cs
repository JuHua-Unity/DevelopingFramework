using Model;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(ReferenceCollector))]
    internal class ReferenceCollectorEditor : Editor
    {
        private ReferenceCollector ReferenceCollector;
        private List<ReferenceCollector.KV> temp = new List<ReferenceCollector.KV>();
        private Object tempObj;

        private void OnEnable()
        {
            ReferenceCollector = (ReferenceCollector)target;
        }

        public override void OnInspectorGUI()
        {
            GetData();

            #region ===
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除所有"))
            {
                temp.Clear();
            }
            if (GUILayout.Button("删除空引用"))
            {
                for (int i = temp.Count - 1; i >= 0; i--)
                {
                    if (temp[i].Value == null || string.IsNullOrEmpty(temp[i].Key))
                    {
                        temp.RemoveAt(i);
                    }
                }
            }
            if (GUILayout.Button("排序"))
            {
                temp.Sort();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.Space();
            #region ===
            for (int i = 0; i < temp.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                temp[i].Key = EditorGUILayout.TextField(temp[i].Key);
                temp[i].Value = EditorGUILayout.ObjectField(temp[i].Value, typeof(Object), false);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    temp.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
            #endregion

            EditorGUILayout.Space();
            #region ===
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", GUILayout.Width(40)))
            {
                temp.Add(new ReferenceCollector.KV() { Key = "Key" });
            }
            EditorGUILayout.LabelField(" 或者拖拽到后面框里", GUILayout.Width(108));
            tempObj = EditorGUILayout.ObjectField(null, typeof(Object), false);
            if (tempObj != null)
            {
                Object[] dragObjs = DragAndDrop.objectReferences;
                for (int j = 0; j < dragObjs.Length; j++)
                {
                    temp.Add(new ReferenceCollector.KV() { Key = dragObjs[j].name, Value = dragObjs[j] });
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            SetData();
        }

        private void SetData()
        {
            SerializedProperty a = serializedObject.FindProperty("Data");
            a.ClearArray();
            for (int i = 0; i < temp.Count; i++)
            {
                a.InsertArrayElementAtIndex(i);
                a.GetArrayElementAtIndex(i).FindPropertyRelative("Key").stringValue = temp[i].Key;
                a.GetArrayElementAtIndex(i).FindPropertyRelative("Value").objectReferenceValue = temp[i].Value;
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        private void GetData()
        {
            temp.Clear();
            SerializedProperty a = serializedObject.FindProperty("Data");
            int len = a.arraySize;
            for (int i = 0; i < len; i++)
            {
                SerializedProperty b = a.GetArrayElementAtIndex(i);
                temp.Add(new ReferenceCollector.KV()
                {
                    Key = b.FindPropertyRelative("Key").stringValue,
                    Value = b.FindPropertyRelative("Value").objectReferenceValue
                });
            }
        }
    }
}