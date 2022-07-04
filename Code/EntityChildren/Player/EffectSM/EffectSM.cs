using SFML.System;
using SFML.Window;

partial class Player {

	partial class EffectSM : StateMachine {
		Player _p;

		NormalState _normalState;
		InvincibleState _invincibleState;

		public EffectSM(Player player) {
			_p = player;

			_normalState = new NormalState(this);
			_invincibleState = new InvincibleState(this);

			init(_normalState);
		}

		public void OnHit() {
			_invincibleState.Start(3f);
			switchState(_invincibleState);
		}

		public void OnVoidFall() {
			_invincibleState.Start(2f);
			switchState(_invincibleState);
		}
	}
}