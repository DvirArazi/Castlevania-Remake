partial class Player { partial class EffectSM {
	
	class InvincibleState : State {
		Player _p;
		EffectSM _sm;

		float _maxTime = default!;

		public InvincibleState(EffectSM effectSM) {
			_sm = effectSM;
			_p = _sm._p;
		}

		public void Start(float maxTime) {
			_p._invincibilityTime = 0f;
			_maxTime = maxTime;
		}

		public override void Update(float elapsed) {
			_p._invincibilityTime += elapsed;

			if (_p._invincibilityTime >= _maxTime) {
				_sm.switchState(_sm._normalState);
			}
		}

		public override void Exit() {
			_p._invincibilityTime = 0;
		}
	}
}}