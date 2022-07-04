using SFML.System;

partial class Player { partial class GeneralSM {
	
	class ControlledState : State {
		static float _SPEED = 20f;
		
		Player _p;
		GeneralSM _sm;

		public ControlledState(GeneralSM generalSM) {
			_sm = generalSM;
			_p = _sm._p;
		}

		public override void Enter() {
			_p._faceDir = _p._position.X < _sm._destination ? 1 : -1;
		}

		public override void Update(float elapsed) {
			_p._position.X += _p._faceDir * _SPEED * elapsed;

			_p._walkTime += elapsed;

			if (_p._faceDir * (_p._position.X - _sm._destination) > 0) {
				_sm.switchState(_sm._activeState);
				if (_sm._roomIndex != null) {
					_p.Room.SwitchRoom((int)_sm._roomIndex, (Vector2f)_sm._roomPosition!);
				}
			}
		}

		public override void Exit() {
			_p._walkTime = 0;
		}
	}
}}