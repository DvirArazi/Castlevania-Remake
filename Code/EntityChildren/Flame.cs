using SFML.System;
using SFML.Graphics;

class Flame : Entity {
    static Texture _TEXTURE; protected override Texture _texture {get => _TEXTURE;}
    static Vector2i _SIZE;
    static Animation _FLICKER;

    static Flame() {
        _TEXTURE = Textures.PARTICLES;
        _SIZE = new Vector2i(8, 16);
        _FLICKER = new Animation (new [] {
            (new IntRect( 0, 0, _SIZE.X, _SIZE.Y), -_SIZE/2, 0.2f),
            (new IntRect( 8, 0, _SIZE.X, _SIZE.Y), -_SIZE/2, 0.2f),
            (new IntRect(16, 0, _SIZE.X, _SIZE.Y), -_SIZE/2, 0.2f),
        });
    }

    float _time;

    public Flame(Room room, Vector2f position) : base(room) {
        _position = position;
        _time = 0f;
    }

    public override void Update(float elapsed) {
        _time += elapsed;

        if (_time < _FLICKER.CycleTime) {
            setSprite(_FLICKER.getSection(_time));
        } else {
            Room.AddToEntitiesKillStack(this);
        }
    }

    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
    }
}