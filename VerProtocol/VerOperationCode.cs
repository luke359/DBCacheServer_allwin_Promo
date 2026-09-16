using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerProtocol
{
    public enum VerOperationCode
    {
        Null = 0,
        //====================訪問DB用==============================
        /// <summary>確認版本</summary>
        CheckVersion = 1,
        /// <summary>連線完成訊息</summary>
        ConnectCompleted = 2,
    }
}
