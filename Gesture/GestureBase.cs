using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GintySoft
{
    public class GestureBase
    {
        public event Action<string> OnGestureDetected;
        public int MinimalPeriodBetweenGestures { get; set; }
    }
}
