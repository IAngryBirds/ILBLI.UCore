using System;

namespace ILBLI.UCore.ILogUnity
{
    public interface IULog
    { 
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception ex);
    }
}
