using SFML.System;

partial class Player { partial class GeneralSM {
	
	class ActiveState : State {
		Player _p;
		GeneralSM _sm;

		MovementSM _movementSM;
		AttackSM _attackSM;

		public ActiveState(GeneralSM generalSM) {
			_sm = generalSM;
			_p = _sm._p;

			_movementSM = new MovementSM(_p);
			_attackSM = new AttackSM(_p);
		}

		public override void Enter() {
			_movementSM.Enter();
			_attackSM.Enter();
		}

		public override void Update(float elapsed) {
			_movementSM.Update(elapsed);
			_attackSM.Update(elapsed);
		}

		public override void Exit() {
			_movementSM.Exit();
			_attackSM.Exit();
		}
	}
}}