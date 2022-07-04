abstract class StateMachine {
	State _defaultState;
	protected State _crntState {get; private set;}
	State _nextState;

	protected StateMachine() {
		_defaultState = default!;
		_crntState = default!;
		_nextState = default!;
	}

	protected void init(State state) {
		_defaultState = state;
		_crntState = state;
		_nextState = state;
	}

	public virtual void Enter() {
		_crntState.Enter();
	}

	public void Update(float elapsed) {
		if (_crntState != _nextState) {
			_crntState.Exit();
			_crntState = _nextState;
			_crntState.Enter();
		}

		_crntState.Update(elapsed);
	}

	public virtual void Exit() {
		_crntState.Exit();
		// _crntState = _defaultState;
		// _nextState = _defaultState;
		switchState(_defaultState);
	}

	protected void switchState(State state) {
		_nextState = state;
	}
}