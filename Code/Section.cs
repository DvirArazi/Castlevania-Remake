using SFML.System;
using SFML.Graphics;

public class Section {
	public readonly IntRect Rect;
	public readonly Vector2i Offset;

	public Section(IntRect rect, Vector2i offset) {
		this.Rect = rect;
		this.Offset = offset;
	}

	public IntRect GetBox() {
		return new IntRect(Offset.X, Offset.Y, Rect.Width, Rect.Height);
	}
}