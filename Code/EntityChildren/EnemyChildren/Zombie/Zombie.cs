using SFML.System;
using SFML.Graphics;

partial class Zombie : Enemy {
	static Vector2f _PIVOT; protected override Vector2f _pivot {get=>_PIVOT;}
	static readonly float _SPEED = 40f; //pixels per second
	static readonly Animation _WALK;

	static Zombie() {
		_PIVOT = new Vector2f(8, 0);
		_SPEED = 40f;
		_WALK = new Animation(new[] {
			(new IntRect( 0, 0, 16, 32), new Vector2i(0, 0), 0.5f),
			(new IntRect(16, 0, 16, 32), new Vector2i(0, 0), 0.5f),
		});
	}

	float _walkTime;

	public Zombie(Room room, Vector2f position) :
		base(room, position, new FloatRect(0f, 0f, 16f, 32f), 1, 2) {
	}

	protected override void postUpdate(float elapsed) {
		if (_stunnedTime / 0.05f % 2f < 1f) {
			setSprite(_WALK.getSection(_walkTime));
		} else {
			setSprite(Global.EMPTY_SECTION);
		}
	}

	public override void Draw(RenderWindow window) {
		window.Draw(_sprite);
	}

	protected override void preKill() {
		Room.DropHeart(_position + GetMiddleOfBox());

		kill();
	}

	protected override void onActiveUpdate(float elapsed) {
		_position.X += _faceDir * _SPEED * elapsed;
		_walkTime += elapsed;
	}
}