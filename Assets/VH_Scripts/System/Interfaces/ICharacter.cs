namespace SmallTown {
public interface ICharacter {
	public bool Initialized { get; }
	public void Setup();
	public void Hit(CharacterHit hit);
}
}