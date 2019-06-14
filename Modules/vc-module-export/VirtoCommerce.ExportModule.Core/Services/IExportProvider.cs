using VirtoCommerce.ExportModule.Core.Model;

namespace VirtoCommerce.ExportModule.Core.Services
{
    public interface IExportProvider
    {
        string TypeName { get; }
        IExportProviderConfiguration Configuration { get; }
        ExportedTypeMetadata Metadata { get; set; }
        void WriteMetadata(ExportedTypeMetadata metadata);
        void WriteRecord(object objectToRecord);
    }
}
