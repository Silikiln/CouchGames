using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTeams {
	public static Dictionary<GamepadInput, int> GamepadTeam = new Dictionary<GamepadInput, int>();
	public static int[] TeamSizes {
		get {
			int[] result = new int[GamepadTeam.Values.Max () + 1];
			foreach (int teamIndex in GamepadTeam.Values)
				if (teamIndex > -1)
					result [teamIndex]++;
			return result;
		}
	}
	public static GamepadInput[] TeamMembers(int teamIndex) {
		return GamepadTeam.Where (kv => kv.Value == teamIndex).Select<KeyValuePair<GamepadInput, int>, GamepadInput>(kv => kv.Key).ToArray();
	}


	[System.Serializable]
	public class GameInfo {
		public string Title;
		public string Description;
		public GameTeam[] Teams;

		public int TeamCount { get { return Teams.Length; } }

		public int MaxPlayers { 
			get {
				int result = 0;
				if (Teams != null)
					foreach (GameTeam team in Teams)
						result += team.MaxMembers;
				return result;
			} 
		}

		public int MinPlayers { 
			get {
				int result = 0;
				if (Teams != null)
					foreach (GameTeam team in Teams)
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

		public bool Valid(int currentMembers) {
			return currentMembers >= MinMembers && currentMembers <= MaxMembers;
		}
	}
}
