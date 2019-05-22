using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileSwLib.UtilTest
{
    public abstract class UtilTestLib
    {
        public static ILog log;
        public static void SetLog(ILog Log)
        {
            log = Log;
        }

    }
}
