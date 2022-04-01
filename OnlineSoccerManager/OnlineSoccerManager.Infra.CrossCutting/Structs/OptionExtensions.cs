using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Infra.CrossCutting.Structs
{
    public partial class Option
    {
        public static Option<T> Of<T>(T value) => new Option<T>(value, value != null);
    }
}
