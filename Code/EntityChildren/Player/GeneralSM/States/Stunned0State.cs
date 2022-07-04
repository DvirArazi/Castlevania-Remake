using SFML.System;

partial class Player { partial class GeneralSM {
	
	class Stunned0State : State {
		Player _p;
		GeneralSM _sm;

		public Stunned0State(GeneralSM generalSM) {
			_sm = generalSM;
			_p = _sm._p;
		}

		public void Start(int hitDirection) {
			_p._moveDir = hitDirection;
		}

		public override void Enter() {
			_p._stunnedStage = 0;
			_p._velocity = -_JUMP_SPEED;
			_p._prevGlobalHitboxTileY = _p.getCrntGlobalHitboxTileY();
		}

		public override void Update(float elapsed) {
			_p._velocity += Global.GRAVITY * elapsed;
			Vector2f expected = _p._position + new Vector2f(_p._moveDir * WALK_SPEED, _p._velocity) * elapsed;

			if (_p._velocity > 0 && _p.fallFloorCollision(expected)) {
				_p._position.X = expected.X;
				float addedY = _p._hitbox.Top + _p._hitbox.Height;
                _p._position.Y = MathF.Floor((expected.Y + addedY) / Stage.TILE_SIZE.Y) * Stage.TILE_SIZE.Y - addedY;
				_sm.switchState(_sm._stunned1State);
			}
			else {
				if (_p._position.Y > _p.Room.Stage.GetHeight()) {
					_p.onVoidFall();
				} else {
					_p._position = expected;
				}
			}
		}

		public override void Exit() {
			_p._stunnedStage = -1;
		}
	}
}}