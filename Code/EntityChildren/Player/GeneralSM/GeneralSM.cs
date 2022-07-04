using SFML.System;

partial class Player {

	partial class GeneralSM : StateMachine {
		Player _p;

		ActiveState _activeState;
		Stunned0State _stunned0State;
		Stunned1State _stunned1State;
		ControlledState _controlledState;

		float _destination;
		int? _roomIndex = null;
		Vector2f? _roomPosition = null;

		public GeneralSM(Player player) {
			_p = player;

			_activeState = new ActiveState(this);
			_stunned0State = new Stunned0State(this);
			_stunned1State = new Stunned1State(this);
			_controlledState = new ControlledState(this);

			init(_activeState);
		}

		public void OnHit(int hitDirection) {
			_stunned0State.Start(hitDirection);
			switchState(_stunned0State);
		}

		public void OnVoidFall() {
			switchState(_stunned1State);
		}

		public void OnMoveTo(float destination, int? roomIndex, Vector2f? roomPosition) {
            _destination = destination;
			_roomIndex = roomIndex;
			_roomPosition = roomPosition;
            switchState(_controlledState);
        }
	}
}