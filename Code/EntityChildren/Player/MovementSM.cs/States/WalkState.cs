using SFML.System;
using SFML.Window;

partial class Player { partial class MovementSM {
	
	class WalkState : State {
		Player _p;
		MovementSM _sm;

		float _fallShift;

		public WalkState(MovementSM movementSM) {
			_sm = movementSM;
			_p = _sm._p;

			_fallShift = 0f;
		}

		public override void Enter() {
			_p._isGrounded = true;
		}

		public override void Update(float elapsed)
		{
			_p._moveDir = _sm.getWalkDirection();

			if(!_p.groundedFloorCollision()) {
				_fallShift = -_p._faceDir * Stage.TILE_SIZE.X;
				_sm.switchState(_sm._jumpState);
			} 
			else if (_p._attackStage == -1) {

				if (Keyboard.IsKeyPressed(_p._config.Jump)) {
					_p._velocity = -_JUMP_SPEED;
					_sm.switchState(_sm._jumpState);
				} else if (Keyboard.IsKeyPressed(_p._config.Up)) {
					var stairsX = _sm.getStairsX(true);
					if (stairsX != null) {
						_sm._stairsX = (float)stairsX;
						_sm.switchState(_sm._stairs0State);
					}
				} else if (Keyboard.IsKeyPressed(_p._config.Down)) {
					var stairsX = _sm.getStairsX(false);
					if (stairsX != null) {
						_sm._stairsX = (float)stairsX;
						_sm.switchState(_sm._stairs0State);
					} else {
						_sm.switchState(_sm._crouchState);
					}
				} 
				if (_p._moveDir != 0) {
					_p._walkTime += elapsed;
					_p._faceDir = _p._moveDir;
					_p._position.X += _p._moveDir * WALK_SPEED * elapsed;
				} else {
					_p._walkTime = 0f;
				}

			} else {
				_p._walkTime = 0f;
			}
		}

		public override void Exit() {
			_p._walkTime = 0;
			_p._isGrounded = false;

			_p._lastGroundedPosition = _p._position;
			_p._lastGroundedPosition.X += _fallShift;
			_fallShift = 0f;
		}
	}
}}