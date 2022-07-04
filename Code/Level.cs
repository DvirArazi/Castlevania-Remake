using SFML.System;
using SFML.Graphics;

class Level {
    Game _game;

    View _gameplayView;
    View _uiView;
    UserInterface _ui;
    Player _player;
    RoomPreset[] _presets;
    Room _crntRoom;
    Room _nextRoom;

    public Level(Game game, RoomPreset[] presets) {
        _game = game;
        _gameplayView = new View(new FloatRect(0f, 0f, Game.VIEW_WIDTH, Game.VIEW_HEIGHT));
        _uiView = new View(new FloatRect(0f, 0f, Game.VIEW_WIDTH, Game.VIEW_HEIGHT));
        _ui = new UserInterface(this);
        _presets = presets;

        _player = new Player();
        _player.Position = new Vector2f(50f, 0f);

        _crntRoom = new Room(this, _player, _presets[0]);
        _nextRoom = _crntRoom;
    }

    public void Update(float elapsed) {
        if (_crntRoom != _nextRoom) {
            _crntRoom = _nextRoom;
        }

        _crntRoom.Update(elapsed);
    }

    public void Draw(RenderWindow window) {
        window.SetView(_gameplayView);
        _crntRoom.DrawGameplay(window);
        window.SetView(_uiView);
        _ui.Draw(window);
    }

    //ROOM
    public Vector2f ViewCenter {
        get {
            return _gameplayView.Center;
        }
        set {
            _gameplayView.Center = value;
        }
    }

    public Vector2f ViewSize {
        get {
            return _gameplayView.Size;
        }
    }

    public void SwitchRoom(int roomIndex, Vector2f playerPosition) {
        _nextRoom = new Room(this, _player, _presets[roomIndex]);
        _player.Position = playerPosition;
    }

    //UI
    public Player GetPlayer() {
        return _crntRoom.Player;
    }
}