using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStateChanger))]
public class GameStateChangerEditor : Editor
{
    private new GameStateChanger target => base.target as GameStateChanger;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("forced start"))
        {
            target.StartSession();
        }
    }
}
