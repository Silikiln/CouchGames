using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Direction = UISelectionInput.Direction;
using SelectionInput = UISelectionInput.ISelectionInput;

public class BaseUISelectionHandler : BaseInputModule {
	public SelectionMapping FirstToSelect;
	public bool StartSelected = false;
	public float SelectDelay = .75f;

	public UISelectionInput.InputMethods[] selectMethods;
	public bool Selecting { get { return CurrentlySelected != null; } }

	public SelectionMapping CurrentlySelected { get; private set; }

	private float delay;
	private List<SelectionInput> selectionMethods;

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
		delay = Mathf.Clamp (delay - Time.deltaTime, 0, SelectDelay);

		bool foundInput = false;
		foreach (SelectionInput selectionMethod in selectionMethods) {
			if (Selecting) {
				if (selectionMethod.Submitting)
					ExecuteEvent(ExecuteEvents.submitHandler);
				if (selectionMethod.Canceling)
					ExecuteEvent (ExecuteEvents.cancelHandler);
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
