using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Utilities
{
	/// <summary>
	/// Generic class providing a <see cref="Compare(T, T)"/> method to sort a <see cref="IEnumerable{T}"/>
	/// with a prioritized value set.
	/// </summary>
	/// <remarks>
	/// Prioritized values will always go before the rest. Each prioritized value has their own priority,
	/// with higher priority means lower order in the sorted <see cref="IEnumerable{T}"/>.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class PrioritizedComparer<T> where T : IComparable
	{
		private Dictionary<T, int> PriorityDefinition { get; set; }

		/// <summary>
		/// Constructs a <see cref="PrioritizedComparer{T}"/> object with <paramref name="objects"/>
		/// as the prioritized set, with decreasing priorities.
		/// </summary>
		/// <param name="objects"></param>
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

		/// <summary>
		/// Performs a prioritized comparison.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>
		/// <list type="bullet">
		/// <item> &lt; 0 if a goes before b in sorted order.</item>
		/// <item> 0 if a and b has the same order.</item>
		/// <item> &gt; 0 if a goes after b in sorted order.</item>
		/// </list>
		/// </returns>
		public int Compare(T a, T b)
		{
			int priorityA;
			int priorityB;
			var resA = PriorityDefinition.TryGetValue(a, out priorityA);
			if (!resA) priorityA = int.MinValue;
			var resB = PriorityDefinition.TryGetValue(b, out priorityB);
			if (!resB) priorityB = int.MinValue;
			var temp = priorityA - priorityB;
			return temp == 0 ? a.CompareTo(b) : temp;
		}
	}
}
