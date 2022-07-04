using SFML.System;
using SFML.Window;
using SFML.Graphics;

partial class Player : Entity {
	const float WALK_SPEED = 60f; //pixels per second
	const float _JUMP_SPEED = 240f; //pixels per second

	static Texture _TEXTURE; protected override Texture _texture {get => _TEXTURE;}
	static Vector2f _PIVOT; protected override Vector2f _pivot {get=>_PIVOT;}
	static float _STANDING_TOP;
	static float _STANDING_HEIGHT;
	static float _CROUCH_TOP;
	static float _CROUCH_HEIGHT;
	static Section _IDLE;
	static Section _CROUCH;
	static Section _STUNNED;
	static Section[] _ATTACK;
	static Section[] _CROUCH_ATTACK;
	static Section[,] _CLIMB_ATTACK;
	static Section[] _CLIMB;
	static float _WALK_FRAME_TIME;
	static Animation _WALK;

	static Player() {
		_TEXTURE = Textures.BELMONT;
		_PIVOT = new Vector2f(8, 0);
		_STANDING_TOP = 2f;
		_STANDING_HEIGHT = 30f;
		_CROUCH_TOP = 2f;
		_CROUCH_HEIGHT = 23f;
		_IDLE		= new Section(new IntRect(0,   0, 16, 32), new Vector2i(0, 0));
		_CROUCH		= new Section(new IntRect(0,  75, 16, 23), new Vector2i(0, 2));
		_STUNNED	= new Section(new IntRect(0, 132, 16, 32), new Vector2i(0 ,0));
		_ATTACK = new Section[] {
			new Section(new IntRect( 0, 33, 24, 32), new Vector2i( 0, 0)),
			new Section(new IntRect(24, 33, 16, 32), new Vector2i( 0, 0)),
			new Section(new IntRect(40, 33, 24, 32), new Vector2i(-8, 0)),
		};
		_CROUCH_ATTACK = new Section[] {
			new Section(new IntRect( 0, 108, 24, 23), new Vector2i( 0, 2)),
			new Section(new IntRect(24, 108, 16, 23), new Vector2i( 0, 2)),
			new Section(new IntRect(40, 108, 24, 23), new Vector2i(-8, 2)),
		};
		_CLIMB = new Section[] {
			new Section(new IntRect( 0, 165, 16, 32), new Vector2i( 0, 0)),
			new Section(new IntRect(16, 165, 16, 32), new Vector2i( 0, 0)),
			new Section(new IntRect(32, 165, 16, 32), new Vector2i( 0, 0)),
		};
		_CLIMB_ATTACK = new Section[,] {
			{
				new Section(new IntRect( 0, 198, 24, 32), new Vector2i( 0, 0)),
				new Section(new IntRect(24, 198, 16, 32), new Vector2i( 0, 0)),
				new Section(new IntRect(40, 198, 22, 32), new Vector2i(-6, 0))
			},
			{
				new Section(new IntRect( 0, 231, 24, 32), new Vector2i( 0, 0)),
				new Section(new IntRect(24, 231, 16, 32), new Vector2i( 0, 0)),
				new Section(new IntRect(40, 231, 22, 32), new Vector2i(-6, 0))
			}
		};
		_WALK_FRAME_TIME = 0.2f;
		_WALK = new Animation(new[] {
			(new IntRect(16, 0, 16, 32), new Vector2i(0, 0), _WALK_FRAME_TIME),
			(new IntRect(32, 0, 16, 32), new Vector2i(0, 0), _WALK_FRAME_TIME),
			(new IntRect(16, 0, 16, 32), new Vector2i(0, 0), _WALK_FRAME_TIME),
			(new IntRect( 0, 0, 16, 32), new Vector2i(0, 0), _WALK_FRAME_TIME),
		});
	}

	GeneralSM _generalSM;
	EffectSM _effectSM;
	InputConfig _config;
	Whip _whip;

	//stateInfo
	public float Health {get; private set;}
	int _moveDir;
	float _walkTime;
	Vector2f _lastGroundedPosition;
	bool _isCrouching;
	float _invincibilityTime;
	int _attackStage;
	int _stunnedStage;
	int _climbStage;
	bool _isAtTopJump;
	float _velocity;
	int _climbDir;
	float _distance;
	bool _isGrounded;

	public Vector2f Position {get => _position; set => _position = value;}
	public int FaceDir {get => _faceDir;}
	public int AttackStage {get => _attackStage;}
	public new Room Room {get => base.Room; set => base.Room = value;}

	public Player() : base(default!) {
		_hitbox = new FloatRect(0f, _STANDING_TOP, 16f, _STANDING_HEIGHT);

		Health = 20f;

		_generalSM = new GeneralSM(this);
		_generalSM.Enter();
		_effectSM = new EffectSM(this);
		_effectSM.Enter();

		_config = new InputConfig(
			Keyboard.Key.Left,
			Keyboard.Key.Right,
			Keyboard.Key.Up,
			Keyboard.Key.Down,
			Keyboard.Key.X,
			Keyboard.Key.Z
		);

		_whip = new Whip(this);

		//stateInfo
		_moveDir = 0;
		_faceDir = 1;
		_walkTime = 0f;
		_lastGroundedPosition = _position;
		_isCrouching = false;
		_invincibilityTime = 0f;
		_attackStage = -1;
		_stunnedStage = -1;
		_climbStage = -1;
		_isAtTopJump = false;
		_velocity = 0f;
		_climbDir = 0;
		_distance = 0f;
		_isGrounded = false;
	}

	public override void Update(float elapsed) {
		//update state machines
		//=====================
		_generalSM.Update(elapsed);
		_effectSM.Update(elapsed);

		//update the section
		//==================
		if (_invincibilityTime / 0.05f % 2f < 1f) {
			if (_attackStage != -1) {
				if (_isCrouching || _isAtTopJump) {
					setSprite(_CROUCH_ATTACK[_attackStage]);
				} else if (_climbStage != -1) {
					setSprite(_CLIMB_ATTACK[_climbDir == 1 ? 1 : 0, _attackStage]);
				} else {
					setSprite(_ATTACK[_attackStage]);
				}
			} else if (_walkTime != 0f) {
				setSprite(_WALK.getSection(_walkTime));
			} else if (_isCrouching || _isAtTopJump) {
				setSprite(_CROUCH);
			} else if (_stunnedStage != -1) {
				setSprite(_stunnedStage switch {
					0 => _STUNNED,
					_ => _CROUCH
				});
			}
			else if (_climbStage != -1) {
				setSprite(_CLIMB[_distance < 0.5 ? (_climbDir == 1 ? 2 : 0) : 1]);
			} else {
				setSprite(_IDLE);
			}
		} else {
			setSprite(Global.EMPTY_SECTION);
		}
	}

	public override void Draw(RenderWindow window) {
		window.Draw(_sprite);

		if (_attackStage != -1) {
			_whip.Draw(window);
		}
	}

	public void Hit(int hitDirection, int damage) {
			_generalSM.OnHit(hitDirection);
			_effectSM.OnHit();

			Health -= damage;
	}

	public void Grab(Item item) {
	}

	public void MoveTo(float destination, int? roomIndex, Vector2f? roomPosition) {
		if(_isGrounded) {
			_generalSM.OnMoveTo(destination, roomIndex, roomPosition);
		}
	}

	void onVoidFall() {
		Health -= 5;
		_position = _lastGroundedPosition;
		_generalSM.OnVoidFall();
		_effectSM.OnVoidFall();
	}
}