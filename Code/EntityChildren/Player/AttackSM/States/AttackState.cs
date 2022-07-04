partial class Player{ partial class AttackSM {

	class AttackState : State {
		Player _p;
		AttackSM _sm;

		float[] _switchTimes;
		float _time;

		public AttackState(AttackSM attackSM) {
			_sm = attackSM;
			_p = _sm._p;

			_switchTimes = Utils.toAbsoluteTimes(new float[] {0.15f, 0.1f, 0.3f});

			_time = 0f;
		}

		public override void Enter() {
			_p._attackStage = 0;
		}

		public override void Update(float elapsed) {
			_time += elapsed;

			for (int i = 0; i < _switchTimes.Length; i++) {
				if (_time < _switchTimes[i]) {
					_p._attackStage = i;
					break;
				}
			}

			_p._whip.Set();

			if (_time >= _switchTimes.Last()) {
				_sm.switchState(_sm._readyState);
			}			
		}

		public override void Exit() {
			_time = 0;
			_p._attackStage = -1;
		}
	}

}} 