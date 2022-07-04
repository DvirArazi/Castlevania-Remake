using SFML.Graphics;

partial class Game {

    public partial class ScreensSM : StateMachine {
        Game _g;

        //TitleState _titleState;
        //ConfigState _configState;
        //LoadState _loadState;
        //MapState _mapState;
        LevelScreen _levelState;
    
        public ScreensSM(Game game) {
            _g = game;

            _levelState = new LevelScreen(this);

            init(_levelState);
        }

        public void Draw(RenderWindow window) {
            ((Screen)_crntState).Draw(window);
        }

    }

}