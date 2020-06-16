using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test2.Exceptions
{
    public class MusicianDoesNotAddedException : Exception
    {
        public MusicianDoesNotAddedException(string message) : base(message)
        {
        }

        public MusicianDoesNotAddedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MusicianDoesNotAddedException()
        {
        }
    }
}
