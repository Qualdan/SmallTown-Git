using System.Collections.Generic;

namespace SmallTown {
public static class ListExtensions {


	// ************************************************************************************************************ EXTENSIONS

	public static T Random<T>(this IList<T> list) {
		if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static T TakeItem<T>(this IList<T> list, int index) {
		if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove item from an empty list");
		T item = list[index];
		list.RemoveAt(index);
		return item;
	}

	public static void Shuffle<T>(this IList<T> list) {
		System.Random rng = new System.Random();
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
}