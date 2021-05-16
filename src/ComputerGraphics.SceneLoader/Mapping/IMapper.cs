namespace ComputerGraphics.SceneLoader.Mapping
{
    internal interface IMapper<out TDomain, in TInput>
    {
        public TDomain Map(TInput input);
    }
}