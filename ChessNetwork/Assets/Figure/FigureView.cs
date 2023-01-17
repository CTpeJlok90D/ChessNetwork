using System.Collections.Generic;
using UnityEngine;

public class FigureView : MonoBehaviour
{
    [SerializeField] private Figure _ownFigure;
    [SerializeField] private TurnView _turnViewPrefub;
    [Header("Turn position")]
    [SerializeField] private float _yOffcet = 0.01f;

    private List<TurnView> _instances = new();

    private void OnEnable()
    {
        _ownFigure.Selected.AddListener(OnSelect);
        _ownFigure.Deselected.AddListener(OnDeselect);
    }
    private void OnDisable()
    {
        _ownFigure.Selected.RemoveListener(OnSelect);
        _ownFigure.Deselected.RemoveListener(OnDeselect);
    }

    private void OnSelect()
    {
        if (GameStateChanger.Singletone.CurrentState == GameStateChanger.State.WaitingLobby)
        {
            return;
        }
        foreach (Turn turn in _ownFigure.PossibleTurns)
        {
            TurnView instance = Instantiate(_turnViewPrefub, _ownFigure.Board.transform).Init(turn, _yOffcet);
            instance.Clicked.AddListener(RemoveAllTurnView);
            _instances.Add(instance);
        }
    }

    private void OnDeselect()
    {
        RemoveAllTurnView();
    }


    private void RemoveAllTurnView()
    {
        foreach (TurnView turn in _instances)
        {
            Destroy(turn.gameObject);
        }
        _instances.Clear();
    }
}
