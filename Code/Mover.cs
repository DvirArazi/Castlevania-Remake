using SFML.System;
using SFML.Graphics;

class Mover {
    public FloatRect GlobalBox {get; private set;}
    public float Destination;
    public int? RoomIndex;
    public Vector2f? RoomPosition;

    public Mover(Vector2i tileXY, float destination, int? roomIndex = null, Vector2f? roomPosition = null) {
        GlobalBox = new FloatRect(
            Stage.TILE_SIZE.X * tileXY.X, Stage.TILE_SIZE.Y * tileXY.Y,
            Stage.TILE_SIZE.X, Stage.TILE_SIZE.Y
            );
        Destination = destination;
        RoomIndex = roomIndex;
        RoomPosition = roomPosition;
    }
}