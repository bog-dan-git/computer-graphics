namespace ComputerGraphics.Ioc.Framework.Tests.Services
{
    public interface ISomeSingletonService
    {
        int Counter { get; }
        void IncreaseCounter();
    }
    public class SomeSingletonService : ISomeSingletonService
    {
        private int _counter = 0;
        public int Counter => _counter;
        public void IncreaseCounter()
        {
            _counter++;
        }
    }
}