using SFML.System;

class GravityItem : Item {
    float _velocity;

    public GravityItem(Room room, Vector2f position, ItemType type) : base(room, position, type) {
        _velocity = 0f;
    }

    protected override void fall(float elapsed) {
        _velocity += Global.GRAVITY * elapsed;
        Vector2f expected = new Vector2f(_position.X, _position.Y + _velocity * elapsed);
        if (fallFloorCollision(expected)) {
            _falling = false;
            float addedY = _hitbox.Top + _hitbox.Height;
            _position.Y = MathF.Floor((expected.Y + addedY) / Stage.TILE_SIZE.Y) * Stage.TILE_SIZE.Y - addedY;
        }
        else {
            _position.Y = expected.Y;
        }
    }
}