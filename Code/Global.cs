using SFML.System;
using SFML.Graphics;

static class Global {
    public readonly static Section EMPTY_SECTION;
    
	public readonly static float GRAVITY; //pixels per second^2

    static Global() {
        EMPTY_SECTION = new Section(new IntRect(), new Vector2i());
        GRAVITY = 720f;
    }
}