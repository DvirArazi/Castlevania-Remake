using SFML.System;
using SFML.Window;

partial class Player
{
    partial class MovementSM
    {

        class JumpState : State
        {
            Player _p;
            MovementSM _sm;

            public JumpState(MovementSM movementSM)
            {
                _sm = movementSM;
                _p = _sm._p;
            }

            public override void Enter()
            {
                setTopPose();
                _p._prevGlobalHitboxTileY = _p.getCrntGlobalHitboxTileY();
            }

            public override void Update(float elapsed)
            {
                _p._velocity += Global.GRAVITY * elapsed;
                Vector2f expected = _p._position + new Vector2f(_p._moveDir * WALK_SPEED, _p._velocity) * elapsed;

                //in considereation
                //=================
                // if (Keyboard.IsKeyPressed(_p._config.Up)) {
				// 	Vector2f? point = _sm.pointLineCollision(expected);
				// 	if (point != null) {
				// 		_sm._stairsX = ((Vector2f)point).X;
                //         _p._position = ((Vector2f)point);

				// 		_sm.switchState(_sm._stairs1State);
                //         return;
				// 	}
				// } else if (Keyboard.IsKeyPressed(_p._config.Down)) {
				// 	var stairsX = _sm.getStairsX(false);
				// 	if (stairsX != null) {
				// 		_sm._stairsX = (float)stairsX;
				// 		_sm.switchState(_sm._stairs1State);
                //         return;
				// 	}
				// }

                if (_p._velocity > 0 && _p.fallFloorCollision(expected)) {
                    _p._position.X = expected.X;
                    float addedY = _p._hitbox.Top + _STANDING_HEIGHT;
                    _p._position.Y = MathF.Floor((expected.Y + addedY) / Stage.TILE_SIZE.Y) * Stage.TILE_SIZE.Y - addedY;
                    _sm.switchState(_sm._walkState);
                } else {
                    setTopPose();

                    if (_p._position.Y > _p.Room.Stage.GetHeight()) {
                        _p.onVoidFall();
                    } else {
                        _p._position = expected;
                    }
                }
            }

            void setTopPose() {
                if (MathF.Abs(_p._velocity) < 150f) {
                    _p._isAtTopJump = true;
                    _p._hitbox.Height = _CROUCH_HEIGHT;
                } else {
                    _p._isAtTopJump = false;
                    _p._hitbox.Height = _STANDING_HEIGHT;
                }
            }

            public override void Exit()
            {
                _p._isAtTopJump = false;
                _p._hitbox.Height = _STANDING_HEIGHT;
                _p._velocity = 0f;
                _p._moveDir = 0;
            }
        }
    }
}