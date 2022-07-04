using SFML.System;
using SFML.Window;

partial class Player {

	partial class MovementSM : StateMachine {
		Player _p;
		
		WalkState _walkState;
		JumpState _jumpState;
		CrouchState _crouchState;
		Stairs0State _stairs0State;
		Stairs1State _stairs1State;

		float _stairsX = default!;
		int _stairDir = default!;

		public MovementSM(Player player) {
			_p = player;
			
			_walkState = new WalkState(this);
			_jumpState = new JumpState(this);
			_crouchState = new CrouchState(this);
			_stairs0State = new Stairs0State(this);
			_stairs1State = new Stairs1State(this);

			init(_walkState);
		}

		int getWalkDirection() {
			return
				Convert.ToInt32(Keyboard.IsKeyPressed(_p._config.Right)) - 
				Convert.ToInt32(Keyboard.IsKeyPressed(_p._config.Left));
		}

		float? getStairsX(bool up) {
			int x0 = (int)(_p._position.X / Stage.TILE_SIZE.X);
			Vector2i tile = Stage.PositionToTileXY(_p._position + _p._hitbox.Size());
			int shift = up ? 1 : 0;
			int x1 = tile.X;
			int y = tile.Y - shift;

			float px0 = _p._position.X + _p._hitbox.Left;
			float px1 = _p._position.X + _p._hitbox.Left + _p._hitbox.Width;

			_p._climbDir = up ? -1 : 1;

			for (int x = x0 - shift; x <= x1 + 1 - shift; x++) {
				var crnt = _p.Room.Stage.GetTile(new Vector2i(x, y));
				if (crnt != null) {
					_stairDir = 
						Convert.ToInt32(crnt.Type == Stage.TileType.StairRight) -
						Convert.ToInt32(crnt.Type == Stage.TileType.StairLeft);
					if (_stairDir * _p._climbDir == 1) {
						if (px0 < (x + 1.5f)*Stage.TILE_SIZE.X && px1 > (x + 0.5f)*Stage.TILE_SIZE.X) {
							return (x + 0.5f) * Stage.TILE_SIZE.X;
						}
					}
					else if(_stairDir * _p._climbDir == -1) {
						if (px0 < (x + 0.5f)*Stage.TILE_SIZE.X && px1 > (x - 0.5f)*Stage.TILE_SIZE.X) {
							return (x - 0.5f) * Stage.TILE_SIZE.X;
						}
					}
				}
			}

			return null;
		}

		Vector2f? pointLineCollision(Vector2f expected) {
			Vector2f point = new Vector2f(
				_p._hitbox.Left + _p._hitbox.Width/2f,
				_p._hitbox.Top + _p._hitbox.Height
			);
			Vector2f e = expected + point;
			var tileXY = Stage.PositionToTileXY(e);
			var tileType = _p.Room.Stage.GetTile(tileXY)?.Type;
			Vector2f d = default!;

			if (tileType != null) {
				_stairDir = 
						Convert.ToInt32(tileType == Stage.TileType.StairRight) -
						Convert.ToInt32(tileType == Stage.TileType.StairLeft);
				if (_stairDir == 1) {
					d = Stage.TileXYToPosition(tileXY + new Vector2i(1, 1));
				}
				else if (_stairDir == -1) {
					d = Stage.TileXYToPosition(tileXY + new Vector2i(0, 0));
				}
			} else if(_p._moveDir == 1 &&
					_p.Room.Stage.GetTile(tileXY+new Vector2i(-1, 0))?.Type == Stage.TileType.StairRight) {
				if(_p.Room.Stage.GetTile(tileXY+new Vector2i(0, -1))?.Type == Stage.TileType.StairRight) {
					d = Stage.TileXYToPosition(tileXY + new Vector2i(1, -1));
				} else {
					d = Stage.TileXYToPosition(tileXY);
				}
			} else if(_p._moveDir == -1 &&
					_p.Room.Stage.GetTile(tileXY+new Vector2i(1, 0))?.Type == Stage.TileType.StairLeft) {
				if(_p.Room.Stage.GetTile(tileXY+new Vector2i(0, -1))?.Type == Stage.TileType.StairLeft) {
					d = Stage.TileXYToPosition(tileXY + new Vector2i(1, -1));
				} else {
					d = Stage.TileXYToPosition(tileXY + new Vector2i(1, 0));
				}
			}

			Vector2f p = _p._position + point;
			float pM = m(p, e);
			float tM = Stage.TILE_SIZE.Y / Stage.TILE_SIZE.X;
			if (_stairDir == 1) {
				tM = 1/tM;
			}
			float x = (pM*p.X-p.Y-(tM*d.X-d.Y))/(pM-tM);
			float y = pM*(x - p.X) + p.Y;
			Console.WriteLine(p.X + " < " + x + " < " + e.X);
			if (p.X < x && x < e.X) {
				return new Vector2f(x, y)-point;
			}
			return null;
		}

		float m(Vector2f p0, Vector2f p1) {
			return ((p1.Y - p0.Y)/(p1.X - p0.X));
		}
	}
}