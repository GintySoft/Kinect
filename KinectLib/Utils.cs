using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GintySoft.KinectLib
{
    public static class Utils
    {
        public static string NewGuidID()
        {
            return Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}
