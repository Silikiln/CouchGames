using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Direction = UISelectionInput.Direction;
using SelectionInput = UISelectionInput.ISelectionInput;

public class BaseUISelectionHandler : BaseInputModule {
	public bool LoopHorizontally = false;
	public bool LoopVertically = true;
	public GameObject FirstToSelect;
	public float SelectDelay = .75f;

	public UISelectionInput.InputMethods[] selectMethods;
	public bool Selecting { get { return CurrentlySelected != null; } }
	public Selectable CurrentlySelected { 
		get { 
			return currentIndex > -1 ? 
				possibleSelectables[currentIndex] :
				null;
		}
	}

	private int currentIndex = -1;
	private float delay;
	private List<Selectable> possibleSelectables = new List<Selectable>();
	private List<SelectionInput> selectionMethods;

	public override void ActivateModule() {
		selectionMethods = UISelectionInput.GetSelectionInputs (selectMethods);

		possibleSelectables.AddRange(GetComponentsInChildren<Selectable> (true));

		if (FirstToSelect != null && ((currentIndex = possibleSelectables.IndexOf (FirstToSelect.GetComponent<Selectable> ())) > -1)) {
			ExecuteEvent(ExecuteEvents.selectHandler);
		}

		delay = 0;
	}

	public override void Process() {
		delay -= (delay > 0 ? Time.deltaTime : 0);

		foreach (SelectionInput selectionMethod in selectionMethods) {
			if (Selecting) {
				if (selectionMethod.Submitting)
					ExecuteEvent(ExecuteEvents.submitHandler);
				if (selectionMethod.Canceling)
					ExecuteEvent (ExecuteEvents.cancelHandler);
			}

			Direction direction;
			bool forceDirection = selectionMethod.GetDirection (Time.deltaTime, out direction);
			if (direction != Direction.None && (forceDirection || delay <= 0)) {
				int nextIndex = currentIndex;

				switch (direction) {
					case Direction.Up:
						if (nextIndex > 0)
							nextIndex--;
						else if (LoopVertically)
							nextIndex = possibleSelectables.Count - 1;
						break;
					case Direction.Down:
						if (currentIndex < possibleSelectables.Count - 1)
							nextIndex++;
						else if (LoopVertically)
							nextIndex = 0;
						break;
				}

				if (nextIndex == currentIndex)
					continue;
					
				if (Selecting)
					ExecuteEvent (ExecuteEvents.deselectHandler);
				currentIndex = nextIndex;
				ExecuteEvent (ExecuteEvents.selectHandler);

				delay = SelectDelay;
			}
		}
	}

	protected void ExecuteEvent<T>(ExecuteEvents.EventFunction<T> handler) where T : IEventSystemHandler {
		ExecuteEvents.Execute (CurrentlySelected.gameObject, new BaseEventData (eventSystem), handler);
	}
}
