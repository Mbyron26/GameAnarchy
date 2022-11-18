using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbyronModsCommon {
    public abstract class ModSingleton<Type> {
        public static Type Instance { get; set; }
    }
}
