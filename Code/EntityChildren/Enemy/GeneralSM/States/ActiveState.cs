partial class Enemy { partial class GeneralSM {

    class ActiveState : State {
        Enemy _e;
        GeneralSM _sm;

        public ActiveState(GeneralSM generalSM) {
            _sm = generalSM;
            _e = _sm._e;
        }

        public override void Update(float elapsed) {
            _e.onActiveUpdate(elapsed);
        }
    }

}}