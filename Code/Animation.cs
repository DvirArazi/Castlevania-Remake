using SFML.System;
using SFML.Graphics;

class Animation {
	public class Frame {
		public readonly Section section;
		public readonly float switchTime;

		public Frame(IntRect rect, Vector2i offset, float switchTime) {
			section = new Section(rect, offset);
			this.switchTime = switchTime;
		}
	}

	Frame[] _frames;
	public float CycleTime {get; private set;}

	public Animation((IntRect rect, Vector2i offset, float duration)[] frames) {
		
		_frames = new Frame[frames.Length];
		
		float crntTime = 0f;
		for (int i = 0; i < frames.Length; i++) {
			crntTime += frames[i].duration;
			_frames[i] = new Frame(frames[i].rect, frames[i].offset, crntTime);
		}
		CycleTime = crntTime;
	}

	public Section getSection(float time) {
		time %= CycleTime;
		foreach (var frame in _frames) {
			if (time < frame.switchTime) {
				return frame.section;
			}
		}

		return _frames[0].section;
	}
}