namespace AdvancedConsole.Commands.Modules.Building
{
    public interface IModuleBuilder<in T>
    {
        Module Build(T input);
    }
}