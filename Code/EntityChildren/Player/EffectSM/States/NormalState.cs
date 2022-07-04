partial class Player { partial class EffectSM {
	
	class NormalState : State {
		Player _p;
		EffectSM _sm;

		public NormalState(EffectSM effectSM) {
			_sm = effectSM;
			_p = _sm._p;
		}

		public override void Update(float elapsed) {
			_p.Room.PlayerEnemyCollision(_p.GetGlobalBox());
		}
	}
}}