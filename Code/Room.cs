using SFML.System;
using SFML.Graphics;

class Room {
    Level _level;
    public Stage Stage {get; private set;}
    public Player Player {get; set;}
    Sprite _background;
    EntityGroup<Enemy> _enemies;
    EntityGroup<Item> _items;
    EntityGroup<Entity> _particles;
    EntityGroup<Candle> _candles;
    IEntityGroup<Entity>[] _groups;
    Spawner[] _spawners;
    Teleporter[] _teleporters;
    Mover[] _movers;

    public Room(Level level, Player player, RoomPreset preset) {
        _level = level;

        _enemies = new EntityGroup<Enemy>();
        _items = new EntityGroup<Item>();
        _particles = new EntityGroup<Entity>();
        _candles = new EntityGroup<Candle>();
        _groups = new IEntityGroup<Entity>[] {
            _enemies, _items, _particles, _candles
        };
        _spawners = new Spawner[preset.GetSpanwers.Length];
        _teleporters = new Teleporter[preset.GetTeleporters.Length];
        _movers = new Mover[preset.GetMovers.Length];

        int width = preset.Layout.IndexOf('x');
		preset.Layout = preset.Layout.Remove(width, 1);

        Stage = new Stage(new Stage.Tile?[width, preset.Layout.Length/width]);

        int crntSpawnerI = 0;
        int crntTeleporterI = 0;
        int crntMoverI = 0;
        var charDict = new Dictionary<char, Action<int, int>?>() {
			{'*', null},
			{'#', (x, y) => Stage.Layout[x, y] = new Stage.Tile(Stage.TileType.Floor, new IntRect( 0, 0, 16, 16))},
			{'<', (x, y) => Stage.Layout[x, y] = new Stage.Tile(Stage.TileType.StairLeft , new IntRect(16, 0, 16, 35), new Vector2i(0, -19), true)},
			{'>', (x, y) => Stage.Layout[x, y] = new Stage.Tile(Stage.TileType.StairRight, new IntRect(16, 0, 16, 35), new Vector2i(0, -19))},
            {'C', (x, y) => _candles.Entities.Add(new Candle(this, new Vector2f(x*Stage.TILE_SIZE.X, y*Stage.TILE_SIZE.Y)))},
            {'S', (x, y) => {
                _spawners[crntSpawnerI] = preset.GetSpanwers[crntSpawnerI](this, new Vector2i(x, y));
                crntSpawnerI++;
            }},
            {'T', (x, y) => {
                _teleporters[crntTeleporterI] = preset.GetTeleporters[crntTeleporterI](new Vector2i(x, y));
                crntTeleporterI++;
            }},
            {'M', (x, y) => {
                _movers[crntMoverI] = preset.GetMovers[crntMoverI](new Vector2i(x, y));
                crntMoverI++;
            }}
		};

		for (int x = 0; x < Stage.Layout.GetLength(0); x++) {
			for (int y = 0; y < Stage.Layout.GetLength(1); y++) {
                var crnt = charDict[preset.Layout[x + width * y]];
                if (crnt != null) {
				    crnt(x, y);
                }
			}
		}

        Player = player;
        Player.Room = this;

        _background = preset.Background;
    }

    public void Update(float elapsed) {
        Player.Update(elapsed);

        foreach (var group in _groups) {
            foreach (var entity in group) {
                entity.Update(elapsed);
            }
            group.EmptyKillStack();
        }
        Array.ForEach(_spawners, v => v.Set());

        playerGrabItem();
        setGameplayView();
        playerTeleporterCollision();
        playerMoverCollision();
    }

    public void DrawGameplay(RenderWindow window) {
        window.Draw(_background);
        Stage.Draw(window);

        foreach (var group in _groups) {
            foreach (var entity in group) {
                entity.Draw(window);
            }
        }

        Player.Draw(window);
    }

    void setGameplayView() {
        float x = Player.Position.X;
        if (x < Game.VIEW_WIDTH/2f) {
            x = Game.VIEW_WIDTH/2f;
        } else if (x > Stage.GetWidth() - Game.VIEW_WIDTH/2f) {
            x = Stage.GetWidth() - Game.VIEW_WIDTH/2f;
        }

        _level.ViewCenter = new Vector2f(x, _level.ViewCenter.Y).floor();
    }

    void playerGrabItem() {
        foreach (var item in _items) {
            if (Utils.boxCollision(Player.GetGlobalBox(), item.GetGlobalBox())) {
                Player.Grab(item);
                AddToItemKillStack(item);
            }
        }
    }

    public void playerTeleporterCollision() {
        foreach (var teleporter in _teleporters) {            
            if (Utils.boxCollision(Player.GetGlobalBox(), teleporter.GlobalBox)) {
                _level.SwitchRoom(teleporter.RoomIndex, teleporter.RoomPosition);
            }
        }
    }

    //PLAYER
    public void PlayerEnemyCollision(FloatRect playerBox) {
        foreach (var enemy in _enemies) {
            if (Utils.boxCollision(playerBox, enemy.GetGlobalBox())) {
                Player.Hit(enemy.FaceDir, enemy.Attack);
            }
        }
    }

    public void playerMoverCollision() {
        foreach (var mover in _movers) {            
            if (Utils.boxCollision(Player.GetGlobalBox(), mover.GlobalBox)) {
                Player.MoveTo(mover.Destination, mover.RoomIndex, mover.RoomPosition);
            }
        }
    }

    //WHIP
    public void WhipEnemyCollision(FloatRect whipBox, float attack) {
        foreach (var enemy in _enemies) {
            if (Utils.boxCollision(whipBox, enemy.GetGlobalBox())) {
                enemy.Hit(attack);
            }
        }
    }

    public void WhipCandleCollision(FloatRect whipBox) {
        foreach (var candle in _candles) {
            if (Utils.boxCollision(whipBox, candle.GetGlobalBox())) {
                candle.Hit();
                _candles.KillStack.Push(candle);
            }
        }
    }

    //ENEMY
    public void AddToEnemiesKillStack(Enemy enemy) {
        _enemies.KillStack.Push(enemy);
    }

    public void CreateFlame(Vector2f position) {
        Flame flame = new Flame(this, position);
        _particles.Entities.Add(flame);
    }

    //ENEMY + CANDLE
    public void DropHeart(Vector2f position) {
        Heart heart = new Heart(this, position);
        _items.Entities.Add(heart);
    }

    //FLAME (and probably more entities in the future)
    public void AddToEntitiesKillStack(Entity entity) {
        _particles.KillStack.Push(entity);
    }

    //ITEM
    public void AddToItemKillStack(Item item) {
        _items.KillStack.Push(item);
    }

    //SPAWNER
    public bool isInViewProx(Vector2f position) {
        FloatRect r = new FloatRect(
            _level.ViewCenter.X-Game.VIEW_WIDTH/2f-20f, _level.ViewCenter.Y-Game.VIEW_HEIGHT/2f-20f,
            _level.ViewSize.X*2f+20f, _level.ViewSize.Y*2f+20f);
        return position.X > r.Left && position.X < r.Left + r.Width &&
            position.Y > r.Top && position.Y < r.Top + r.Height; 
    }

    public void SpawnEnemy(Enemy enemy) {
        _enemies.Entities.Add(enemy);
    }

    //MOVER
    public void SwitchRoom(int roomIndex, Vector2f roomPosition) {
        _level.SwitchRoom(roomIndex, roomPosition);
    }
}