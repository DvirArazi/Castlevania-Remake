using SFML.System;
using SFML.Graphics;

partial class Game {
	public readonly static float VIEW_HEIGHT;
	public readonly static float ASPECT_RATIO;
	public static float VIEW_WIDTH {get => MathF.Floor(VIEW_HEIGHT*ASPECT_RATIO);}

	static Game() {
		VIEW_HEIGHT = 200f;
		ASPECT_RATIO = 16f/9f;
	}

	View _view;
	RenderWindow _window;
	ScreensSM _screensSM;

	public Game() {
		_view = new View(new FloatRect(0f, 0f, VIEW_WIDTH, VIEW_HEIGHT));

		_window = new RenderWindow(new SFML.Window.VideoMode(
			(uint)(VIEW_WIDTH*2), (uint)(VIEW_HEIGHT*2)),
			"Castlevania"
		);
		_window.SetView(_view);
		_window.SetVerticalSyncEnabled(true);
		_window.Closed += (_, _) => _window.Close();
		uint prevWidth = _window.Size.X;
		_window.Resized += (_, e) => {
			if (e.Width != prevWidth) {
				_window.Size = new Vector2u(e.Width, e.Width * 9/16);
			} else {
				_window.Size = new Vector2u(e.Width, e.Width * 9/16);
			}

			prevWidth = e.Width;
		};

		_screensSM = new ScreensSM(this);
	}

	void update(float elapsed) {
		_screensSM.Update(elapsed);
	}

	void draw() {
		_window.Clear();

		_screensSM.Draw(_window);

		_window.Display();
	}

	public void Run() {
		Clock clock = new Clock();
		while(_window.IsOpen) {
			_window.DispatchEvents();
			update(clock.Restart().AsSeconds());
			draw();
		}
	}

	public void SetViewPos(Vector2f position) {
		_view.Viewport = new FloatRect(position.X, position.Y, VIEW_HEIGHT*ASPECT_RATIO, VIEW_HEIGHT);
		// _view.Center = 
	}
	public void SetViewCenterX(float x) {
		_view.Center = new Vector2f(x, _view.Center.Y);
	}
	public void SetViewCenterX2(float x) {
		// _view.Viewport = new FloatRect(0, 0, VIEW_WIDTH, VIEW_HEIGHT);
		_view.Center = new Vector2f(x, _view.Center.Y);
		_window.SetView(_view);
	}
	public void SetViewPosX(float x) {
		_view.Viewport = new FloatRect(x, _view.Viewport.Top, VIEW_HEIGHT*ASPECT_RATIO, VIEW_HEIGHT);
	}
	public void SetViewPosY(float y) {
		_view.Viewport = new FloatRect(_view.Viewport.Left, y, VIEW_HEIGHT*ASPECT_RATIO, VIEW_HEIGHT);
	}
}