using SFML.System;
using SFML.Graphics;

abstract partial class Enemy : Entity {
	static Texture _TEXTURE; protected override Texture _texture {get => _TEXTURE;}

	static Enemy() {
		_TEXTURE = Textures.ENEMIES;
	}

	GeneralSM _generalSM;
	Spawner? _spawner;
	protected float _stunnedTime;

	public Vector2f Position {get => _position;}
	public int Attack {get; private set;}
	public float Health {get; private set;}
	public int FaceDir {get => _faceDir;}

	public Enemy(Room room, Vector2f position,
		FloatRect hitbox, int health, int attack) : 
		base(room) {

		_position = position;
		_faceDir = _position.X < Room.Player.Position.X ? 1 : -1;
		_hitbox = hitbox;

		_generalSM = new GeneralSM(this);

		_stunnedTime = 0f;
		
		Attack = attack;
		Health = health;
	}

	public override void Update(float elapsed) {
		_generalSM.Update(elapsed);
		postUpdate(elapsed);
	}

	protected virtual void postUpdate(float elapsed) {}

	protected virtual void preKill() {}

	protected virtual void onActiveUpdate(float elapsed) {}

	protected void kill() {
		_spawner?.DeathNotice();
		Room.AddToEnemiesKillStack(this);
		Room.CreateFlame(_position + GetMiddleOfBox());
	}

	public void setSpawner(Spawner spawner) {
		_spawner = spawner;
	}

	public void Hit(float attack) {
		if (_stunnedTime == 0) {
			Health -= attack;

			if (Health <= 0) {
				preKill();
			} else {
				_generalSM.OnHit(attack);
			}
		}
	}
}