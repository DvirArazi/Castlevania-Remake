partial class Enemy {

    partial class GeneralSM : StateMachine {
        Enemy _e;

        ActiveState _activeState;
        StunnedState _stunnedState;

        public GeneralSM(Enemy enemy) {
            _e = enemy;

            _activeState = new ActiveState(this);
            _stunnedState = new StunnedState(this);

            init(_activeState);
        }

        public void OnHit(float attack) {
            switchState(_stunnedState);
        }
    }

}