
namespace XMLConverter.Model
{
    using Contracts;

    public interface IModel
    {
        void GenerateNewFile();

        void ValidateFiles();

        void ValidateSettings(Mappings mappings);
    }
}
