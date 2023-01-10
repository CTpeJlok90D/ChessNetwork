using Unity.Netcode;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FigurePositionNetcode))]
public class FigureEditor : Editor
{
    private new FigurePositionNetcode target => base.target as FigurePositionNetcode;

    private void OnEnable()
    {
        target.newPositionInspectorGUI = new Vector2Int((int)target.transform.localPosition.x, (int)target.transform.localPosition.z);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Move"))
        {
            if(NetworkManager.Singleton.IsServer)
            {
                target.position = target.newPositionInspectorGUI;
            }
            else
            {
                Debug.LogWarning("You have no permitions to do this");
            }
        }
    }
}