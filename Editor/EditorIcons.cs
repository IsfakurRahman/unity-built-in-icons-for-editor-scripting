using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class EditorIcons : EditorWindow {

    public int selGridInt = 0;
    static List<Texture2D> AllIcons;
    static List<Texture2D> icons = new List<Texture2D>();
    Vector2 scrollview;
    GUIContent iconContent;
    string search;

    [MenuItem("Tools/Editor Icons")]
    static void Init()
    {
        EditorIcons window = (EditorIcons)GetWindow(typeof(EditorIcons));
        window.Show();
        window.minSize = new Vector2(300, 300);

        AllIcons = new List<Texture2D>(Resources.FindObjectsOfTypeAll(typeof(Texture2D)) as Texture2D[]);
        AllIcons.Sort((iconA, iconB) => string.Compare(iconA.name, iconB.name, System.StringComparison.OrdinalIgnoreCase));
        for (int i = 0; i < AllIcons.Count; i++)
        {
            if (!string.IsNullOrEmpty(AllIcons[i].name))
            {
                icons.Add(AllIcons[i]);
            }
        }
        AllIcons.Clear();
    }

    [DidReloadScripts]
    static void Reload()
    {
        Init();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search", GUILayout.Width(50));
        search = EditorGUILayout.TextField(search);
        EditorGUILayout.EndHorizontal();
        if (!string.IsNullOrEmpty(search))
        {
            List<Texture2D> newList = new List<Texture2D>();
            for (int i = 0; i < icons.Count; i++)
            {
                if (icons[i].name.IndexOf(search) >= 0)
                {
                    newList.Add(icons[i]);
                }
            }
            AllIcons = newList;
        }
        else
        {
            AllIcons = icons;
        }
        DrawList(AllIcons);
        
    }

    public void DrawList(List<Texture2D> iconsToDraw)
    {
        scrollview = EditorGUILayout.BeginScrollView(scrollview);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        for (int i = 1; i <= iconsToDraw.Count; i++)
        {
            if (i % ((int)Screen.width / 85) == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.BeginVertical();
            Rect R = EditorGUILayout.GetControlRect(GUILayout.Height(100), GUILayout.Width(75));
            if (R.yMax > scrollview.y && R.yMin < scrollview.y+Screen.height)
            {
                if(GUI.Button(new Rect(R.x, R.y, R.width, R.height - 25), iconsToDraw[i - 1]))
                {
                    CopyText(""+iconsToDraw[i].name+""); 
                    Debug.Log("Icon name:   " + iconsToDraw[i].name + "   [Copied]");
                }
                GUI.TextField(new Rect(R.x, R.y + 80, R.width, 16), iconsToDraw[i - 1].name);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal(); 
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private static void CopyText(string text)
    {
        TextEditor TEditor = new TextEditor();
        TEditor.text = text;
        TEditor.SelectAll();
        TEditor.Copy();
    }
}

