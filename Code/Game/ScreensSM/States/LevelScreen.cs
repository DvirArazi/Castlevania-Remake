using SFML.System;
using SFML.Graphics;

partial class Game { partial class ScreensSM {

    public class LevelScreen : Screen {
        Game _g;
        ScreensSM _sm;

        Level _level;

        public LevelScreen(ScreensSM screensSM) {
            _sm = screensSM;
            _g = _sm._g;

            _level = new Level(_g, LevelPresets.preset00);
        }

        public override void Update(float elapsed) {
            _level.Update(elapsed);
        }

        public override void Draw(RenderWindow window) {
            _level.Draw(window);
        }
    }

}}