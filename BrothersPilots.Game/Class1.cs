namespace BrothersPilots.Applications
{
    public class Game
    {
        private readonly string _name;

        public Game(int a)
        {
            _name = (a / 2).ToString();
        }

        public string Name() => _name;


        // Browse our samples repository: https://github.com/nanoframework/samples
        // Check our documentation online: https://docs.nanoframework.net/
        // Join our lively Discord community: https://discord.gg/gCyBu8T
    }
}
