using SFML.Window;


//since a configuration is something that can be used mutliple times,
//change that to a non-static class, remove from managers folder and decide on a name
public class InputConfig {
	public readonly Keyboard.Key Left;
	public readonly Keyboard.Key Right;
	public readonly Keyboard.Key Up;
	public readonly Keyboard.Key Down;
	public readonly Keyboard.Key Jump;
	public readonly Keyboard.Key Attack;

	public InputConfig(
		Keyboard.Key left,
		Keyboard.Key right,
		Keyboard.Key up,
		Keyboard.Key down,
		Keyboard.Key jump,
		Keyboard.Key attack
	) {
		this.Left	 = left;
		this.Right	 = right;
		this.Up		 = up;
		this.Down	 = down;
		this.Jump	 = jump;
		this.Attack	 = attack;
	}

}