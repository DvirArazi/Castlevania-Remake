using SFML.Window;

partial class Player { partial class MovementSM {
	
	class CrouchState : State {
		Player _p;
		MovementSM _sm;

		public CrouchState(MovementSM movementSM) {
			_sm = movementSM;
			_p = _sm._p;
		}

		public override void Enter() {
			_p._isCrouching = true;
			_p._position.Y += _STANDING_TOP + _STANDING_HEIGHT - _CROUCH_TOP - _CROUCH_HEIGHT;
			_p._hitbox.Top = _CROUCH_TOP;
			_p._hitbox.Height = _CROUCH_HEIGHT;
		}

		public override void Update(float elapsed) {
			if (_p._attackStage == -1 && !Keyboard.IsKeyPressed(_p._config.Down)) {
				_sm.switchState(_sm._walkState);
			}
		}

		public override void Exit() {
			_p._isCrouching = false;
			_p._position.Y -= _STANDING_TOP + _STANDING_HEIGHT - _CROUCH_TOP - _CROUCH_HEIGHT;
			_p._hitbox.Top = _STANDING_TOP;
			_p._hitbox.Height = _STANDING_HEIGHT;
		}
	}
}}