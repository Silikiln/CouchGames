using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISelectionInput {
	public enum Direction {
		None = -1,
		Up = 0,
		Left = 1,
		Down = 2,
		Right = 3
	}

	public enum InputMethods {
		KeyboardWASD,
		KeyboardArrows,
		GamepadAny
	}

	public static List<ISelectionInput> GetSelectionInputs(params InputMethods[] methodsToGet) {
		List<ISelectionInput> result = new List<ISelectionInput> ();
		foreach (InputMethods method in methodsToGet)
			switch (method) {
				case InputMethods.KeyboardWASD:
					result.Add (new KeyboardWASDSelectionInput ());
					break;
				case InputMethods.KeyboardArrows:
					result.Add (new KeyboardArrowSelectionInput ());
					break;
				case InputMethods.GamepadAny:
					result.Add (new GamepadAnySelectionInput ());
					break;
			}
		return result;
	}

	public interface ISelectionInput {
		bool GetDirection (float delta, out Direction direction);
		bool Submitting { get; }
		bool Canceling { get; }
	}

	public class KeyboardWASDSelectionInput : ISelectionInput {
		private KeyCode[] keyboardInputs = new KeyCode[] {
			KeyCode.W,
			KeyCode.A,
			KeyCode.S,
			KeyCode.D
		};

		public bool GetDirection(float delta, out Direction direction) {
			for (int i = 0; i < keyboardInputs.Length; i++)
				if (Input.GetKey (keyboardInputs [i])) {
					direction = (Direction)i;
					return Input.GetKeyDown (keyboardInputs [i]);
				}
			direction = Direction.None;
			return false;
		}

		public bool Submitting { get { return Input.GetKeyDown (KeyCode.Space); } }
		public bool Canceling { get { return Input.GetKeyDown (KeyCode.Escape); } }
	}

	public class KeyboardArrowSelectionInput : ISelectionInput {
		private KeyCode[] keyboardInputs = new KeyCode[] {
			KeyCode.UpArrow,
			KeyCode.LeftArrow,
			KeyCode.DownArrow,
			KeyCode.RightArrow
		};

		public bool GetDirection(float delta, out Direction direction) {
			for (int i = 0; i < keyboardInputs.Length; i++)
				if (Input.GetKey (keyboardInputs [i])) {
					direction = (Direction)i;
					return Input.GetKeyDown (keyboardInputs [i]);
				}
			direction = Direction.None;
			return false;
		}

		public bool Submitting { get { return Input.GetKeyDown (KeyCode.Return); } }
		public bool Canceling { get { return Input.GetKeyDown (KeyCode.Delete); } }
	}

	public class GamepadAnySelectionInput : ISelectionInput {
		private const float deadzone = .1f;

		private Gamepad.InputCode[] leftStickInputs = new Gamepad.InputCode[] {
			Gamepad.InputCode.LeftStickUp,
			Gamepad.InputCode.LeftStickLeft,
			Gamepad.InputCode.LeftStickDown,
			Gamepad.InputCode.LeftStickRight,
		};
		private Gamepad.InputCode[] rightStickInputs = new Gamepad.InputCode[] {
			Gamepad.InputCode.RightStickUp,
			Gamepad.InputCode.RightStickLeft,
			Gamepad.InputCode.RightStickDown,
			Gamepad.InputCode.RightStickRight
		};
		private Gamepad.InputCode[] dpadInputs = new Gamepad.InputCode[] {
			Gamepad.InputCode.DpadUp,
			Gamepad.InputCode.DpadLeft,
			Gamepad.InputCode.DpadDown,
			Gamepad.InputCode.DpadRight
		};

		public bool GetDirection(float delta, out Direction direction) {
			Gamepad.AllGamepads.ForEach (gamepad => gamepad.Update ());

			for (int i = 0; i < rightStickInputs.Length; i++)
				foreach (Gamepad gamepad in Gamepad.AllGamepads) {
					if (gamepad.GetInputActive(leftStickInputs[i], rightStickInputs[i], dpadInputs[i])) {
						direction = (Direction) i;
						return gamepad.InputStarted(leftStickInputs[i], rightStickInputs[i], dpadInputs[i]);
					}
				}

			direction = Direction.None;
			return false;
		}

		public bool Submitting { get { return Gamepad.AllGamepads.Any(gamepad => gamepad.GetInputActive(Gamepad.InputCode.A)); } }
		public bool Canceling { get { return Gamepad.AllGamepads.Any(gamepad => gamepad.GetInputActive(Gamepad.InputCode.B)); } }
	}
}
