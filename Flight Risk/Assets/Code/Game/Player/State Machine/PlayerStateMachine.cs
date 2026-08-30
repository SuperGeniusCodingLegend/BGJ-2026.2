using System.Collections.Generic;
using UnityEngine;

namespace FlightRisk.Game.Player.States
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private List<BaseState> states;
        [SerializeField] private PlayerState initialState = PlayerState.Inactive;

        private readonly Dictionary<PlayerState, BaseState> stateDict = new();
        private BaseState currentState;

        private void Awake()
        {
            InitializeStateMachine();
        }

        private void Start()
        {
            EnterInitialState();
        }

        private void Update()
        {
            RunStateMachine();
        }

        private void InitializeStateMachine()
        {
            foreach (var state in states)
            {
                stateDict.Add(state.GetThisState(), state);
            }
        }

        private void EnterInitialState()
        {
            currentState = stateDict[initialState];
            currentState.gameObject.SetActive(true);
        }

        private void RunStateMachine()
        {
            var nextState = currentState.Tick();
            if (nextState == currentState.GetThisState()) return;
            SwitchState(nextState);
        }

        private void SwitchState(PlayerState nextState)
        {
            currentState.gameObject.SetActive(false);
            currentState = stateDict[nextState];
            currentState.gameObject.SetActive(true);
        }
    }
}
