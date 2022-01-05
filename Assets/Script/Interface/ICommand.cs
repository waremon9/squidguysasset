public interface ICommand
{
    public void Execute(Player player);
    public void Undo(Player player);
}
