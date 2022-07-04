using SFML.System;
using SFML.Graphics;

class RoomPreset {
	public string Layout;
	public Func<Room, Vector2i, Spawner>[] GetSpanwers;
	public Func<Vector2i, Teleporter>[] GetTeleporters;
	public Func<Vector2i, Mover>[] GetMovers;
	public Sprite Background;

	public RoomPreset(
		string layout,
		Func<Room, Vector2i, Spawner>[] getSpawners, Func<Vector2i, Teleporter>[] getTeleporters, Func<Vector2i, Mover>[] getMovers,
		Texture backgroundTexture, Vector2f backgroundPosition) {

		Layout = layout;
		GetTeleporters = getTeleporters;
		GetSpanwers = getSpawners;
		GetMovers = getMovers;
		Background = new Sprite();
		Background.Texture = backgroundTexture;
		Background.Position = backgroundPosition;
	}
}

static class LevelPresets {
	public static readonly RoomPreset[] preset00 = new RoomPreset[] {
		new RoomPreset( 
			"***********************************************x" +
			"***********************************************" +
			"***********************************************" +
			"***********************************************" +
			"***********************************************" +
			"***********************************************" +
			"***********************************************" +
			"*******************C***************************" +
			"*******C******************S******C*************" +
			"******************************************MMMM*" +
			"###############################################",
			new Func<Room, Vector2i, Spawner>[] {
				(room, tileXY) => new Spawner(room, position => new Zombie(room, position), tileXY),
			},
			new Func<Vector2i, Teleporter>[] {
			},
			new Func<Vector2i, Mover>[] {
				(tileXY) => new Mover(tileXY, 43f*Stage.TILE_SIZE.X),
				(tileXY) => new Mover(tileXY, 43f*Stage.TILE_SIZE.X),
				(tileXY) => new Mover(tileXY, 44f*Stage.TILE_SIZE.X, 1, new Vector2f(48f, 128f)),
				(tileXY) => new Mover(tileXY, 43f*Stage.TILE_SIZE.X)
			},
			Textures.BACKGROUND_0_0,
			new Vector2f(0f, 0f)
		),
		new RoomPreset( 
			"****************************************x" +
			"****************************************" +
			"****************************************" +
			"****************************************" +
			"********************>*******************" +
			"**************>****>********************" +
			"*************>****>********S************" +
			"**<**C**CCC*>****>**********************" +
			"***<<****>###*S*******#############*****" +
			"****<<>#############<**************#####" +
			"*#####<**************<******************" +
			"*******<************######**************",
			new Func<Room, Vector2i, Spawner>[] {
				(room, tileXY) => new Spawner(room, position => new Zombie(room, position), tileXY),
				(room, tileXY) => new Spawner(room, position => new Zombie(room, position), tileXY),
			},
			new Func<Vector2i, Teleporter>[] {
				// (tileXY) => new Teleporter(tileXY, 1, new Vector2f(0f, 144f))
			},
			new Func<Vector2i, Mover>[] {
			},
			Textures.BACKGROUND_0_1,
			new Vector2f(0f, 0f)
		),
	};
}