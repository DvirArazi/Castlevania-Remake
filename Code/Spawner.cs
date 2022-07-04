using SFML.System;

class Spawner {
    Room _room;
    Func<Vector2f, Enemy> _spawn;
    Vector2f _position;
    Vector2f _enemyPosition;
    bool _isInView;
    bool _isAlive;

    public Spawner(Room room, Func<Vector2f, Enemy> spawn, Vector2i tileXY, Vector2f? enemyPosition = null) {
        _room = room;
        _spawn = spawn;
        _position = Stage.TileXYToPosition(tileXY);
        _enemyPosition = enemyPosition?? _position;
        _isInView = false;
        _isAlive = false;
    }

    public void Set() {
        if (!_isInView) {
            if (!_isAlive && _room.isInViewProx(_position)) {
                _isInView = true;
                _isAlive = true;
                Enemy enemy = _spawn(_enemyPosition);
                enemy.setSpawner(this);
                _room.SpawnEnemy(enemy);
            }
        }
        else {
            if (!_room.isInViewProx(_position)) {
                _isInView = false;
            }
        }
    }

    public void DeathNotice() {
        _isAlive = false;
    }
}