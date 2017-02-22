using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Direction = UISelectionInput.Direction;
using SelectionInput = UISelectionInput.AbstractSelectionInput;

public class BaseUISelectionHandler : BaseInputModule {
	public SelectionMapping FirstToSelect;
	public bool StartSelected = false;
	public bool UseMouse = true;
	public float SelectDelay = .75f;

	public UISelectionInput.InputMethods[] selectMethods;
	public bool Selecting { get { return CurrentlySelected != null; } }

	public SelectionMapping CurrentlySelected { get; private set; }

	public bool IsMouseMoving { get { return currentMousePosition != lastMousePosition; } }

	private bool mouseHovering = false;
	private float delay;
	private List<SelectionInput> selectionMethods;

	private Vector3 currentMousePosition = Vector3.zero, lastMousePosition = Vector3.zero;

	public override void ActivateModule() {
		selectionMethods = UISelectionInput.GetSelectionInputs (selectMethods);

		if (FirstToSelect == null)
			FirstToSelect = GetComponentInChildren<SelectionMapping> (true);

		if (StartSelected) {
			CurrentlySelected = FirstToSelect;
			ExecuteEvent (ExecuteEvents.selectHandler);
		}

		delay = 0;
	}

	public override void Process() {
		if (UseMouse)
			HandleMouse ();

		delay = Mathf.Clamp (delay - Time.deltaTime, 0, SelectDelay);

		bool foundInput = false;
		foreach (SelectionInput selectionMethod in selectionMethods) {
			if (Selecting) {
				if (selectionMethod.Submitting)
					ExecuteEvent(ExecuteEvents.submitHandler);
				if (selectionMethod.Canceling)
					ExecuteEvent (ExecuteEvents.cancelHandler);
			}
			if (selectionMethod.Targetting)
			if (selectionMethod.GetTarget != null) {
				if (Selecting)
					ExecuteEvent (ExecuteEvents.deselectHandler);
				CurrentlySelected = selectionMethod.GetTarget;
				ExecuteEvent (ExecuteEvents.selectHandler);
			}

			Direction direction;
			bool forceDirection = selectionMethod.GetDirection (Time.deltaTime, out direction);
			if (!foundInput && direction != Direction.None && (forceDirection || delay <= 0)) {
				SelectionMapping next = NextSelection (direction);
				if (next == null)
					continue;

				if (!next.Selectable) {
					SelectionMapping first = next;
					do {
						next = NextSelection (next, direction);
					} while (next != first && next != null && !next.Selectable);
					if (next == null || next == first)
						continue;
				}

				foundInput = true;

				if (Selecting)
					ExecuteEvent (ExecuteEvents.deselectHandler);
				CurrentlySelected = next;
				ExecuteEvent (ExecuteEvents.selectHandler);

				delay = SelectDelay;
			}
		}
	}

	private void HandleMouse() {
		lastMousePosition = currentMousePosition;
		currentMousePosition = Input.mousePosition;

		HandleMouseHovering ();
		HandleMouseSubmit();
	}

	protected void HandleMouseHovering() {
		if (!IsMouseMoving)
			return;
		PointerEventData pointer = new PointerEventData (eventSystem);
		pointer.position = (Vector2) currentMousePosition;
		List<RaycastResult> raycasts = new List<RaycastResult> ();
		eventSystem.RaycastAll (pointer, raycasts);
		raycasts = raycasts.FindAll (result => result.gameObject.GetComponent<SelectionMapping> () != null);
		if (raycasts.Count == 0) {
			if (mouseHovering) {
				ExecuteEvent (ExecuteEvents.deselectHandler);
				CurrentlySelected = null;
				mouseHovering = false;
			}
		} else {
			RaycastResult best = raycasts [0];
			for (int i = 1; i < raycasts.Count; i++)
				if (best.depth < raycasts [i].depth)
					best = raycasts [i];

			if (Selecting)
				ExecuteEvent (ExecuteEvents.deselectHandler);

			CurrentlySelected = best.gameObject.GetComponent<SelectionMapping> ();
			ExecuteEvent (ExecuteEvents.selectHandler);
			mouseHovering = true;
		}
	}

	protected void HandleMouseSubmit() {
		if (mouseHovering && Input.GetMouseButtonUp(0)) {
			ExecuteEvent (ExecuteEvents.submitHandler);
		}
	}

	protected SelectionMapping NextSelection (Direction direction) { return NextSelection (CurrentlySelected, direction); }
	protected SelectionMapping NextSelection(SelectionMapping from, Direction direction) {
		if (from == null)
			return FirstToSelect;

		switch (direction) {
			case Direction.Up:
				return from.UpTarget;
			case Direction.Left:
				return from.LeftTarget;
			case Direction.Down:
				return from.DownTarget;
			case Direction.Right:
				return from.RightTarget;
		}

		return null;
	}

	protected void ExecuteEvent<T>(ExecuteEvents.EventFunction<T> handler) where T : IEventSystemHandler {
		ExecuteEvents.Execute (CurrentlySelected.gameObject, new BaseEventData (eventSystem), handler);
	}
}
