namespace ILBLI.UCore.ILogUnity
{
    public interface IULogProvider
    {
        IULog GetSystemULog();
        IULog GetOperationULog();
        IULog GetHttpRequestULog();
    }

}
