using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTeams {
	public static Dictionary<GamepadInput, int> GamepadTeam = new Dictionary<GamepadInput, int>();

	[System.Serializable]
	public class GameInfo {

		public string Title;
		public string Description;
		public GameTeam[] TeamDescription;

		public int TeamCount { get { return TeamDescription.Length; } }

		public int MaxPlayers { 
			get {
				int result = 0;
				if (TeamDescription != null)
					foreach (GameTeam team in TeamDescription)
						result += team.MaxMembers;
				return result;
			} 
		}

		public int MinPlayers { 
			get {
				int result = 0;
				if (TeamDescription != null)
					foreach (GameTeam team in TeamDescription)
						result += team.MinMembers;
				return result;
			} 
		}
	}

	[System.Serializable]
	public class GameTeam {
		public string TeamName;
		public int MinMembers;
		public int MaxMembers;
	}
}
