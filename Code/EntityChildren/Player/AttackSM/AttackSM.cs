partial class Player {

	partial class AttackSM : StateMachine {
		Player _p;
		
		ReadyState _readyState;
		AttackState _attackState;

		public AttackSM(Player player) {
			_p = player;

			_readyState = new ReadyState(this);
			_attackState = new AttackState(this);

			init(_readyState);
		}

		// public override void Exit() {
		// 	_crntState.Exit();
		// 	switchState(_readyState);
		// }
	}
}