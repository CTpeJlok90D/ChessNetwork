using Unity.Netcode;
using UnityEngine;

public class FigurePositionNetcode : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<Vector2Int> _position = new(new Vector2Int());

    public Vector2Int position 
    {
        get
        {
            return new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.z);
        }
        set
        {
            _position.Value = value;
        }
    }

    public override void OnNetworkDespawn()
    {
        transform.localPosition = new Vector3(_position.Value.x, 0 , _position.Value.y);
    }

    private void OnEnable()
    {
        _position.OnValueChanged += OnValueChange;
    }

    private void OnDisable()
    {
        _position.OnValueChanged -= OnValueChange;
    }
   
    private void OnValueChange(Vector2Int oldValue, Vector2Int newValue)
    {
        transform.localPosition = new Vector3(newValue.x, 0, newValue.y);
    }

#if UNITY_EDITOR
    public Vector2Int newPositionInspectorGUI_EDITOR_ONLY;

    private void OnValidate()
    {
        if (Application.isEditor)
        {
            newPositionInspectorGUI_EDITOR_ONLY = position;
            _position.Value = position;
        }
    }
#endif
}
