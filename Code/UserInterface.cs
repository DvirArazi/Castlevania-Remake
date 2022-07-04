using SFML.System;
using SFML.Graphics;

class UserInterface {
    Level _level;

    Vector2f _healthPosition;

    public UserInterface(Level level) {
        _level = level;

        _healthPosition = new Vector2f(5f, 5f);
    }

    public void Draw(RenderWindow window) {
        RectangleShape rect = new RectangleShape(new Vector2f(3f, 6f));
        rect.FillColor = Color.Red;//new Color(255, 0, 0, 0);
        for (int i = 0; i < _level.GetPlayer().Health; i++) {
            rect.Position = _healthPosition + new Vector2f(i * 4f, 0f);
            window.Draw(rect);
        }
    }

}