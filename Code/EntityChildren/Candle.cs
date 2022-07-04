using SFML.System;
using SFML.Graphics;

class Candle : Entity {
    static Texture _TEXTURE; protected override Texture _texture {get => _TEXTURE;}
    static Animation _ANIMNATION;

    static Candle() {
        _TEXTURE = Textures.ITEMS;
        _ANIMNATION = new Animation (new [] {
            (new IntRect( 36, 0, 8, 16), new Vector2i(4, 0) , 0.2f),
            (new IntRect( 44, 0, 8, 16), new Vector2i(4, 0) , 0.2f),
        });
    }

    float _time = 0f;

    public Candle(Room room, Vector2f position) : base(room) {
        _position = position;
        _hitbox = new FloatRect(4f, 0f, 8, 16);
    }

    public override void Update(float elapsed) {
        _time += elapsed;

        setSprite(_ANIMNATION.getSection(_time));
    }

    public override void Draw(RenderWindow window) {
        base.Draw(window);
        DrawDebug(window);
    }

    public void Hit() {
        Room.DropHeart(_position + GetMiddleOfBox());
    }
}