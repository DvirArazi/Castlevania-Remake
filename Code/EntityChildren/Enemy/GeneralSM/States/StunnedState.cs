partial class Enemy { partial class GeneralSM {

    class StunnedState : State {
        Enemy _e;
        GeneralSM _sm;

        float _maxTime;

        public StunnedState(GeneralSM generalSM) {
            _sm = generalSM;
            _e = _sm._e;

            _e._stunnedTime = 0f;
            _maxTime = 3f;
        }

        public override void Update(float elapsed) {
            _e._stunnedTime += elapsed;

            if (_e._stunnedTime >= _maxTime) {
                _sm.switchState(_sm._activeState);
            }
        }

        public override void Exit() {
            _e._stunnedTime = 0f;
        }
    }

}}