using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerProtocol
{
    public enum VerErrorCode
    {
        Ok = 0,
        InvalidOperation = 1,
        InvalidParameter = 2,
        CustomError = 3,
        InvalidMinStartRateLimit = 4,
        IllegalUser = 5
    }
}
