using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum State
    {
        WaitForConnections,
        WaitForStart,
        GameRunning,
        Win,
        Lose
    }

    private State oldState;
    public State state;

	void Start()
    {
        oldState = State.WaitForConnections;
        state = State.WaitForConnections;
	}

    public void GoToState(State _state)
    {
        state = _state;
    }
	
	void Update()
    {
        if( state != oldState )
        {
            switch( state )
            {
            case State.WaitForConnections:
                {
                }
                break;

            case State.WaitForStart:
                {
                }
                break;

            case State.GameRunning:
                {
                }
                break;

            case State.Win:
                {
                }
                break;

            case State.Lose:
                {
                }
                break;
            }

            oldState = state;
        }
	}
}
