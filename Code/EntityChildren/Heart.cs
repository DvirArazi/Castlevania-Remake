using SFML.System;
using SFML.Graphics;

class Heart : Item {
    Vector2f _start;

    public Heart(Room room, Vector2f position) : base(room, position, ItemType.Heart) {
        _position = position;
        _start = _position;
    }

    protected override void fall(float elapsed) {
        if (_falling) {
            float eX(float eY) => _start.X + MathF.Sin((eY-_start.Y)*2*MathF.PI/32f) * 8f;
            float eY = _position.Y + 25f * elapsed;
            Vector2f expected = new Vector2f(eX(eY), eY);
            if (fallFloorCollision(expected)) {
                _falling = false;
                float addedY = _hitbox.Top + _hitbox.Height;
                _position.Y = MathF.Floor((eY + addedY) / Stage.TILE_SIZE.Y) * Stage.TILE_SIZE.Y - addedY;
            }
            else {
                _position.Y = eY;
            }

            _position.X = eX(_position.Y);
        }
    }

    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
    }

}