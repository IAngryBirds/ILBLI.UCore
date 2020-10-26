using System;

namespace ILBLI.UCore.IExceptionUnity
{
    public interface IExceptionHandler
    {
        string WriteLog(Exception ex);
    }
}
