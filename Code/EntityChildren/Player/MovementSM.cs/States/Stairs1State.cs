using SFML.System;
using SFML.Window;

partial class Player { partial class MovementSM {
	
	class Stairs1State : State {
		Player _p;
		MovementSM _sm;

        static float _SPEED = 4.0f; //Half tile per second
        bool _isClimbing = false;
        Vector2f _initPos = default!;

		public Stairs1State(MovementSM movementSM) {
			_sm = movementSM;
			_p = _sm._p;
		}

		public override void Enter() {
			_p._climbStage = 0;
            _p._faceDir = - _sm._stairDir*_p._climbDir;
            _initPos = _p.Position;
            _isClimbing = true;
 		}

		public override void Update(float elapsed) {
            if (_p._attackStage == -1) { 
                if (Keyboard.IsKeyPressed(_p._config.Jump)) {
                    _p._moveDir = _sm.getWalkDirection();
                     if (_p._moveDir != 0) {
                        _p._faceDir = _p._moveDir;
                    }
                    _p._velocity = -_JUMP_SPEED;
                    _sm.switchState(_sm._jumpState);
                } else if (!_isClimbing) {
                    var vDir = getVDir();
                    if (vDir != 0) {
                        _p._climbDir = vDir;
                        _p._faceDir = -_sm._stairDir*_p._climbDir;
                        _isClimbing = true;
                        _initPos = _p.Position;
                    }
                } else {
                    Vector2f deltaPos = _p._climbDir * new Vector2f(-_sm._stairDir * Stage.TILE_SIZE.X/2f, Stage.TILE_SIZE.Y/2f);
                    _p._distance += _SPEED * elapsed;
                    if (_p._distance < 1) {
                        _p._position = _initPos + _p._distance * deltaPos;
                    } else {
                        if (!isStair(_p.Room.Stage.GetTile(Stage.PositionToTileXY(StairPos()))?.Type)) {
                            _p._position = _initPos + deltaPos;
                            if (_p.groundedFloorCollision()) {
                                 _p._walkTime =_WALK_FRAME_TIME;
                                _sm.switchState(_sm._walkState);
                            }
                            else {
                                _sm.switchState(_sm._jumpState);
                            }
                        } else if (
                            (Keyboard.IsKeyPressed(_p._config.Up) && _p._climbDir == -1) ||
                            (Keyboard.IsKeyPressed(_p._config.Down) && _p._climbDir == 1)) {
                            _p._position = _initPos + _p._distance * deltaPos; 
                            _initPos += deltaPos;
                            _p._distance -= 1f;
                        } else {
                            _p._position = _initPos + deltaPos;
                            _initPos = _p._position;
                            _isClimbing = false;
                            _p._distance = 0f;
                        }
                    }
                }
            }
		}

		public override void Exit() {
			_p._climbStage = -1;
            _p._distance = 0f;
            _isClimbing = false;
		}

        bool isStair(Stage.TileType? type) {
            return type == Stage.TileType.StairLeft || type == Stage.TileType.StairRight;
        }

        Vector2f StairPos() {
            return new Vector2f(
                _initPos.X + (_sm._stairDir == _p._climbDir ? -0.5f : 1) * Stage.TILE_SIZE.X,
                _initPos.Y + _p._hitbox.Top + _p._hitbox.Height + (_p._climbDir == 1 ? 0.5f : -1) * Stage.TILE_SIZE.Y
            );
        }

        int getVDir() {
            return 
                (
                    Convert.ToInt32(Keyboard.IsKeyPressed(_p._config.Down) ||
                    (_sm._stairDir == 1 ?
                        Keyboard.IsKeyPressed(_p._config.Left) :
                        Keyboard.IsKeyPressed(_p._config.Right))
                    )
                ) -
                (
                    Convert.ToInt32(Keyboard.IsKeyPressed(_p._config.Up) ||
                    (_sm._stairDir == 1 ?
                        Keyboard.IsKeyPressed(_p._config.Right) :
                        Keyboard.IsKeyPressed(_p._config.Left))
                    )
                );
                
        }
	}
}}