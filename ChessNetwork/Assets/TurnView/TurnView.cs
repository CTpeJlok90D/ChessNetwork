using UnityEngine;
using UnityEngine.Events;

public class TurnView : MonoBehaviour
{
    private Turn _turn;
    private UnityEvent _cicked = new();

    public UnityEvent Clicked => _cicked;

    public TurnView Init(Turn turn, float yOffcet)
    {
        _turn = turn;
        transform.localPosition = new Vector3(turn.MovePosition.x, yOffcet, turn.MovePosition.y);
        return this;
    }

    private void OnMouseDown()
    {
        _turn.Execute();
        Clicked.Invoke();
    }
}
