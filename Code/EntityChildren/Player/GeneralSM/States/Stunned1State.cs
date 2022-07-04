partial class Player { partial class GeneralSM {
	
	class Stunned1State : State {
		Player _p;
		GeneralSM _sm;

		float _time;

		public Stunned1State(GeneralSM generalSM) {
			_sm = generalSM;
			_p = _sm._p;

			_time = 0;
		}

		public override void Enter() {
			_p._stunnedStage = 1;
			_p._position.Y += _STANDING_TOP + _STANDING_HEIGHT - _CROUCH_TOP - _CROUCH_HEIGHT;
			_p._hitbox.Top = _CROUCH_TOP;
			_p._hitbox.Height = _CROUCH_HEIGHT;
		}

		public override void Update(float elapsed) {
			_time += elapsed;
			if (_time >= 0.3f) {
				_sm.switchState(_sm._activeState);
			}
		}

		public override void Exit() {
			_time = 0f;
			_p._stunnedStage = -1;
			_p._position.Y -= _STANDING_TOP + _STANDING_HEIGHT - _CROUCH_TOP - _CROUCH_HEIGHT;
			_p._hitbox.Top = _STANDING_TOP;
			_p._hitbox.Height = _STANDING_HEIGHT;
		}
	}
}}