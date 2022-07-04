using SFML.Window;

partial class Player{ partial class AttackSM {

	class ReadyState : State {
		Player _p;
		AttackSM _sm;

		public ReadyState(AttackSM attackSM) {
			_sm = attackSM;
			_p = _sm._p;
		}

		public override void Update(float elapsed)
		{
			if (Keyboard.IsKeyPressed(_p._config.Attack)) {
				_sm.switchState(_sm._attackState);
			}
		}
	}

}} 