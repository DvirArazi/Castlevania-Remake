using SFML.System;
using SFML.Graphics;

abstract class Entity {
	protected virtual Vector2f _pivot {get;}
	protected abstract Texture _texture {get;}

	protected Vector2f _position = default!;
	protected FloatRect _hitbox = default!;

	protected Sprite _sprite;

	public Room Room {get; protected set;}

	//stateInfo
	protected int _faceDir;

	public Entity(Room room) {
		_pivot = default!;

		Room = room;

		_sprite = new Sprite();
		_sprite.Texture = _texture;

		_faceDir = -1;
	}

	public virtual void Update(float elapsed) {}

	public virtual void Draw(RenderWindow window) {
		window.Draw(_sprite);
	}

	public void DrawDebug(RenderWindow window) {
		RectangleShape hitBoxRect = new RectangleShape(new Vector2f(_hitbox.Width, _hitbox.Height));
		hitBoxRect.Position = _position + new Vector2f(_hitbox.Left, _hitbox.Top);
		hitBoxRect.FillColor = new Color(0, 255, 0, 128);

		RectangleShape posRect = new RectangleShape(new Vector2f(1f, 1f));
		posRect.Position = _position;
		posRect.FillColor = Color.Red;

		window.Draw(hitBoxRect);
		window.Draw(posRect);
	}

	protected void setSprite(Section section) {
		_sprite.Position = (_position + 2f * new Vector2f(
			Convert.ToInt32(_faceDir == 1) * _pivot.X,
			0f// * _pivot.Y //add that functionality if there's actually a reason to turn the sprite upside down
		)).floor();
		_sprite.Origin = -(Vector2f)section.Offset;
		_sprite.Scale = new Vector2f(-_faceDir, 1f);
		_sprite.TextureRect = section.Rect;
	}

	protected bool groundedFloorCollision() {
		int tile0X = (int)(_position.X / Stage.TILE_SIZE.X);
		Vector2i tile1XY = Stage.PositionToTileXY(_position + _hitbox.Size());

		for (int x = tile0X; x <= tile1XY.X; x++) {
			Vector2i crntTileXY = new Vector2i(x, tile1XY.Y);

			if (Room.Stage.GetTile(crntTileXY)?.Type == Stage.TileType.Floor) {
				return true;
			}
		}

		return false;
	}

	protected int _prevGlobalHitboxTileY = default!;

	protected int getCrntGlobalHitboxTileY() {
		return (int)(_position.Y + _hitbox.Top + _hitbox.Height / Stage.TILE_SIZE.Y);
	}

	protected bool fallFloorCollision(Vector2f expected) {
		int tile0X = (int)(expected.X / Stage.TILE_SIZE.X);
		Vector2i tile1XY = Stage.PositionToTileXY(expected + _hitbox.Size());

		if (tile1XY.Y > _prevGlobalHitboxTileY) {
			for (int x = tile0X; x <= tile1XY.X; x++) {
				Vector2i crntTileXY = new Vector2i(x, tile1XY.Y);

				if (Room.Stage.GetTile(crntTileXY)?.Type == Stage.TileType.Floor) {
					return true;
				}
			}
		}

		_prevGlobalHitboxTileY = tile1XY.Y;

		return false;
	}

    public Vector2f GetBoxPosition() {
		return new Vector2f(_hitbox.Left, _hitbox.Top);
	}

	public Vector2f GetMiddleOfBox() {
		return new Vector2f(_hitbox.Width / 2f, _hitbox.Height / 2f);
	}

	public FloatRect GetGlobalBox() {
		return new FloatRect(_position.X + _hitbox.Left, _position.Y + _hitbox.Top, _hitbox.Width, _hitbox.Height);
	}
}