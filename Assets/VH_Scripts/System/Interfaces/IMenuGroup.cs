using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public interface IMenuGroup {
	public List<CanvasGroup> CanvasGroups();
	public CanvasGroup GetCurrentGroup();
	public void SetCurrentGroup(CanvasGroup group);
	public void CheckGroups(CanvasGroup group, bool alsoCurrent = false);
	public void HideContent();
}
}