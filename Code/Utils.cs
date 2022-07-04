using SFML.System;
using SFML.Graphics;

static class Utils {
	public static T at<T> (this T[,] arr, Vector2i tXY) {
		return arr[tXY.X, tXY.Y];
	}

	public static bool exists<T>(this T[,] arr, Vector2i tXY) {
		return 
			tXY.X > 0 && tXY.X < arr.GetLength(0) &&
			tXY.Y > 0 && tXY.Y < arr.GetLength(1)
		;
	}

	public static Vector2f multiply(Vector2f a, Vector2f b)
		=> new Vector2f(a.X * b.X, a.Y * b.Y);

	public static bool boxCollision(FloatRect a, FloatRect b) {
		Vector2f aa = new Vector2f(a.Left, a.Top);
		Vector2f ab = new Vector2f(a.Left + a.Width, a.Top + a.Height);
		Vector2f ba = new Vector2f(b.Left, b.Top);
		Vector2f bb = new Vector2f(b.Left + b.Width, b.Top + b.Height);

		return
			aa.X < bb.X && ba.X < ab.X &&
			aa.Y < bb.Y && ba.Y < ab.Y;
	}

	public static Vector2f Size(this FloatRect rect) {
		return new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height);
	}

	public static float[] toAbsoluteTimes(float[] arr) {
		for (int i = 1; i < arr.Length; i++) {
			arr[i] += arr[i-1];
		}

		return arr;
	}

	public static Vector2f floor(this Vector2f vec) {
		return new Vector2f(MathF.Floor(vec.X), MathF.Floor(vec.Y));
	}
}