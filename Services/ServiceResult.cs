using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace VPM.Services
{
    public struct ServiceResult
    {
        public bool Success;
        public object Data;
        public IList<string> Errors;
        public ServiceResult(bool success, object data = null, string[] errors = null)
        {
            Success = success;
            Data = data;
            Errors = errors;
        }
        
        
    }
}
