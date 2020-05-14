using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReferenceCollector
{
    [CustomEditor(typeof(ReferenceCollector))]
    internal class ReferenceCollectorEditor : Editor
    {
        private readonly List<ReferenceCollector.KV> temp = new List<ReferenceCollector.KV>();
        private Object tempObj;

        public override void OnInspectorGUI()
        {
            GetData();

            #region ===

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除所有"))
            {
                this.temp.Clear();
            }

            if (GUILayout.Button("删除空引用"))
            {
                for (var i = this.temp.Count - 1; i >= 0; i--)
                {
                    if (this.temp[i].Value == null || string.IsNullOrEmpty(this.temp[i].Key))
                    {
                        this.temp.RemoveAt(i);
                    }
                }
            }

            if (GUILayout.Button("排序"))
            {
                this.temp.Sort();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Space();

            #region ===

            for (var i = 0; i < this.temp.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                this.temp[i].Key = EditorGUILayout.TextField(this.temp[i].Key);
                this.temp[i].Value = EditorGUILayout.ObjectField(this.temp[i].Value, typeof(Object), false);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    this.temp.RemoveAt(i);
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
                this.temp.Add(new ReferenceCollector.KV() {Key = "Key"});
            }

            EditorGUILayout.LabelField(" 或者拖拽到后面框里", GUILayout.Width(108));
            this.tempObj = EditorGUILayout.ObjectField(null, typeof(Object), false);
            if (this.tempObj != null)
            {
                var dragObjs = DragAndDrop.objectReferences;
                for (var j = 0; j < dragObjs.Length; j++)
                {
                    this.temp.Add(new ReferenceCollector.KV() {Key = dragObjs[j].name, Value = dragObjs[j]});
                }
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            SetData();
        }

        private void SetData()
        {
            var a = this.serializedObject.FindProperty("Data");
            a.ClearArray();
            for (var i = 0; i < this.temp.Count; i++)
            {
                a.InsertArrayElementAtIndex(i);
                a.GetArrayElementAtIndex(i).FindPropertyRelative("Key").stringValue = this.temp[i].Key;
                a.GetArrayElementAtIndex(i).FindPropertyRelative("Value").objectReferenceValue = this.temp[i].Value;
            }

            this.serializedObject.ApplyModifiedProperties();
            this.serializedObject.UpdateIfRequiredOrScript();
        }

        private void GetData()
        {
            this.temp.Clear();
            var a = this.serializedObject.FindProperty("Data");
            var len = a.arraySize;
            for (var i = 0; i < len; i++)
            {
                var b = a.GetArrayElementAtIndex(i);
                this.temp.Add(new ReferenceCollector.KV()
                {
                    Key = b.FindPropertyRelative("Key").stringValue,
                    Value = b.FindPropertyRelative("Value").objectReferenceValue
                });
            }
        }
    }
}