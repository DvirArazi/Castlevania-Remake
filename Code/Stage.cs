using SFML.System;
using SFML.Graphics;

class Stage {
	public enum TileType {
			Floor,
			Wall,
			FloorWall,
			StairLeft,
			StairRight
		}

	public class Tile {
		public TileType Type;
		public IntRect Rect;
		public Vector2i Offset;
		public bool Flipped;

		public Tile(TileType type, IntRect rect, Vector2i offset = default, bool flipped = false) {
			Type = type;
			Rect = rect;
			Offset = offset;
			Flipped = flipped;
		}
	}

	public static readonly Vector2i TILE_SIZE;
	public static readonly Dictionary<char, Tile?> RECT_DICT;

	static Stage(){
		RECT_DICT = new () {
			{'*', null},
			{'#', new Tile(TileType.Floor,	    new IntRect( 0, 0, 16, 16))},
			{'<', new Tile(TileType.StairLeft , new IntRect(16, 0, 16, 35), new Vector2i(0, -19), true)},
			{'>', new Tile(TileType.StairRight, new IntRect(16, 0, 16, 35), new Vector2i(0, -19))}
		};
		TILE_SIZE = new Vector2i(16, 16);
	}

	public static Vector2i PositionToTileXY(Vector2f position) {
		return new Vector2i(
			(int)(position.X / TILE_SIZE.X),
			(int)(position.Y / TILE_SIZE.Y)
		);
	}

	public static Vector2f TileXYToPosition(Vector2i tileXY) {
		return new Vector2f(
			tileXY.X * TILE_SIZE.X,
			tileXY.Y * TILE_SIZE.Y
		); 
	}

	public Tile?[,] Layout;

	public Stage(Tile?[,] layout) {
		Layout = layout;
	}

	public void Draw(RenderWindow window) {
		Sprite sprite = new Sprite();
		sprite.Texture = Textures.TILES;

		for (int x = 0; x < Layout.GetLength(0); x++) {
			for (int y = 0; y < Layout.GetLength(1); y++) {
				var crnt = Layout[x, y];
				if (crnt != null) {
					sprite.Position = new Vector2f(
						x * TILE_SIZE.X + crnt.Offset.X + (crnt.Flipped ? crnt.Rect.Width : 0), 
						y * TILE_SIZE.Y + crnt.Offset.Y
					);
					sprite.TextureRect = crnt.Rect;
					sprite.Scale = new Vector2f(crnt.Flipped ? -1 : 1, 1);
					window.Draw(sprite);
				}
			}
		}
	}

	public Tile? GetTile(Vector2i tileXY) {
		if (Layout.exists(tileXY)) {
			return Layout.at(tileXY);
		}
		
		return null;
	}

	public int GetWidth() {
		return Layout.GetLength(0) * (int)TILE_SIZE.X;
	}

	public int GetHeight() {
		return Layout.GetLength(1) * (int)TILE_SIZE.Y;
	}
}