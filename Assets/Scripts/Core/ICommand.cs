namespace Core
{
    public interface ICommand
    {
        public int Id { get; set; }
        public void Execute();
    }
}