using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Utilities
{
	public class PrioritizedComparer<T> where T : IComparable
	{
		private Dictionary<T, int> PriorityDefinition { get; set; }

		public PrioritizedComparer(Dictionary<T, int> priorityDefinition)
		{
			PriorityDefinition = priorityDefinition;
		}

		public PrioritizedComparer(params T[] objects)
		{
			PriorityDefinition = new Dictionary<T, int>();
			int counter = objects.Length;
			foreach (var obj in objects)
			{
				if (PriorityDefinition.TryGetValue(obj, out _))
					throw new NotImplementedException();
				PriorityDefinition[obj] = counter--;
			}
		}

		public int Compare(T a, T b)
		{
			int priorityA;
			int priorityB;
			var resA = PriorityDefinition.TryGetValue(a, out priorityA);
			var resB = PriorityDefinition.TryGetValue(b, out priorityB);
			if (!resA || !resB)
				return a.CompareTo(b);
			return priorityA - priorityB;
		}
	}
}
