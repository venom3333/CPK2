using CPK.FilesModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.UnitsOfWork
{
    internal sealed class FilesUow : UnitOfWorkBase, IFilesUow
    {
        public FilesUow(CpkContext context) : base(context)
        {
        }
    }
}
