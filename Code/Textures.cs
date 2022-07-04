using SFML.Graphics;

static class Textures {
	const string PATH = "Assets/Textures/"; 
	public static readonly Texture BELMONT;
	public static readonly Texture WHIP;
	public static readonly Texture ENEMIES;
	public static readonly Texture TILES;
	public static readonly Texture PARTICLES;
	public static readonly Texture ITEMS;
	public static readonly Texture BACKGROUND_0_0;
	public static readonly Texture BACKGROUND_0_1;

	static Textures() {
		BELMONT = new Texture(PATH + "belmont.png");
		WHIP = new Texture(PATH + "whip.png");
		ENEMIES = new Texture(PATH + "enemies.png");
		TILES = new Texture(PATH + "tiles.png");
		PARTICLES = new Texture(PATH + "particles.png");
		ITEMS = new Texture(PATH + "items.png");
		BACKGROUND_0_0 = new Texture(PATH + "backgrounds/background0-0.png");
		BACKGROUND_0_1 = new Texture(PATH + "backgrounds/background0-1.png");

		// BELMONT.Smooth = false;
		// WHIP.Smooth = false;
		// ENEMIES.Smooth = false;
		// TILES.Smooth = false;
		// PARTICLES.Smooth = false;
		// ITEMS.Smooth = false;
	}

}