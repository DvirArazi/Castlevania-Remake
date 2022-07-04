using SFML.System;
using SFML.Graphics;

class Teleporter {
    public FloatRect GlobalBox {get; private set;}
    public int RoomIndex {get; private set;}
    public Vector2f RoomPosition {get; private set;}

    public Teleporter(Vector2i tileXY, int roomIndex, Vector2f playerPosition) {
        GlobalBox = new FloatRect(
            Stage.TILE_SIZE.X * tileXY.X, Stage.TILE_SIZE.Y * tileXY.Y,
            Stage.TILE_SIZE.X, Stage.TILE_SIZE.Y
        );
        RoomIndex = roomIndex;
        RoomPosition = playerPosition;
    }
}