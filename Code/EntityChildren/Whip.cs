using SFML.System;
using SFML.Graphics;

class Whip : Entity {
	static Texture _TEXTURE; protected override Texture _texture {get => _TEXTURE;}
	static Vector2f _PIVOT; protected override Vector2f _pivot {get=>_PIVOT;}
	static Section[] _ATTACK_SECTIONS;
	static FloatRect[] _HITBOXES;
	static FloatRect[] _FLIPPED_HITBOXES;

	static Whip() {
		_TEXTURE = Textures.WHIP;
		_PIVOT = new Vector2f(8, 0);
		_ATTACK_SECTIONS = new Section[] {
			new Section(new IntRect( 0, 0,  8, 32), new Vector2i( 24, 0)), //mode 0
			new Section(new IntRect( 8, 0, 16, 32), new Vector2i( 16, 0)), //mode 0
			new Section(new IntRect(24, 0, 24, 32), new Vector2i(-28, 0)), //mode 0
			new Section(new IntRect( 0, 33,  8, 32), new Vector2i( 24, 0)), //mode 1, 2
			new Section(new IntRect( 8, 33, 16, 32), new Vector2i( 16, 0)), //mode 1, 2
			new Section(new IntRect(24, 33, 24, 32), new Vector2i(-28, 0)), //mode 1
			new Section(new IntRect( 0, 66, 40, 17), new Vector2i(-44, 0)), //mode 2
		};

		_HITBOXES = new FloatRect[] {
			new FloatRect( 24f, 8f,  8f, 24f),
			new FloatRect( 16f, 5f, 16f, 19f),
			new FloatRect(-28f, 8f, 24f,  8f),
			new FloatRect(-44f, 8f, 32f,  8f),
		};

		_FLIPPED_HITBOXES = new FloatRect[_HITBOXES.Length];
		for (int i = 0; i < _HITBOXES.Length; i++) {
			var c = _HITBOXES[i];
			_FLIPPED_HITBOXES[i] = new FloatRect(-c.Left + 2f * _PIVOT.X - c.Width, c.Top, c.Width, c.Height);
		}
	}

	Player _player;
	float _attack = default!;
	int[] _sectionIs = default!;
	int[] _hitboxIs = default!;

	public Whip(Player player) : base(player.Room) {
		_player = player;

		SwitchMode(2);
	}

	public void Set() {
		_position = _player.Position;
		_faceDir = _player.FaceDir;
		_hitbox = _player.FaceDir == -1 ?
			_HITBOXES[_hitboxIs[_player.AttackStage]] :
			_FLIPPED_HITBOXES[_hitboxIs[_player.AttackStage]];

		_player.Room.WhipEnemyCollision(GetGlobalBox(), _attack);
		_player.Room.WhipCandleCollision(GetGlobalBox());

		setSprite(_ATTACK_SECTIONS[_sectionIs[_player.AttackStage]]);
	}

	public void SwitchMode(int mode) {
		switch (mode) {
			case 0:
				_attack = 1f;
				_sectionIs = new int[] {0, 1, 2};
				_hitboxIs = new int[] {0, 1, 2};
				break;
			case 1:
				_attack = 1.5f;
				_sectionIs = new int[] {3, 4, 5};
				_hitboxIs = new int[] {0, 1, 2};
				break;
			case 2:
				_attack = 1.5f;
				_sectionIs = new int[] {3, 4, 6};
				_hitboxIs = new int[] {0, 1, 3};
				break;
		}
	}

	public override void Draw(RenderWindow window) {
		window.Draw(_sprite);

		DrawDebug(window);
	}
}