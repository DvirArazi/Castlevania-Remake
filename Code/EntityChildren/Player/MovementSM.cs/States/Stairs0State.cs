using SFML.Window;

partial class Player { partial class MovementSM {
	
	class Stairs0State : State {
		Player _p;
		MovementSM _sm;

		bool _toRight = default!;

		public Stairs0State(MovementSM movementSM) {
			_sm = movementSM;
			_p = _sm._p;
		}

		public override void Enter() {
			_toRight = _sm._stairsX > _p._position.X;
			_p._faceDir = _toRight ? 1 : -1;
 		}

		public override void Update(float elapsed) {
            if (_p._attackStage == -1) {
                if (Keyboard.IsKeyPressed(_p._config.Up) || Keyboard.IsKeyPressed(_p._config.Down)) {
                    if (_toRight == _sm._stairsX > _p._position.X) {
						_p._walkTime += elapsed;
						_p._position.X += _p._faceDir * WALK_SPEED * elapsed;
					} else {
						_p._position.X = _sm._stairsX;
						_sm.switchState(_sm._stairs1State);
					}
                } else {
					_sm.switchState(_sm._walkState);
				}
            }
		}

		public override void Exit() {
			_p._walkTime = 0f;
		}
	}
}}