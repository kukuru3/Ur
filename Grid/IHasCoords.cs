using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ur.Grid {
    public interface IHasPosition {
        Coords Position { get; }
    }
}
