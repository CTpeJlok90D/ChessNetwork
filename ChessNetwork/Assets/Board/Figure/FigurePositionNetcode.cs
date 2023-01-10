using Unity.Netcode;
using UnityEngine;

public class FigurePositionNetcode : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<Vector2Int> _position = new(new Vector2Int());

    private Vector2Int _startPosition;

    public Vector2Int position 
    {
        get
        {
            return new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        }
        set
        {
            _position.Value = value;
            MoveFigureClientRpc(_position.Value);
        }
    }

    [ClientRpc]
    private void MoveFigureClientRpc(Vector2Int newPosition)
    {
        transform.localPosition = new Vector3(newPosition.x, 0, newPosition.y);
    }

    private void Awake()
    {
        _startPosition = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _position.Value = _startPosition;
            transform.localPosition = new Vector3(_startPosition.x, _startPosition.y);
        }
    }

    public override void OnNetworkDespawn()
    {
        transform.localPosition = _position.Value.ToVector3();
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
    public Vector2Int newPositionInspectorGUI;
#endif
}
