using System;
using PS_project.Models;

namespace PS_project.Utils.Exceptions
{
    class PS_Exception : Exception
    {
        public PS_Exception() : base() { }
        public virtual ErrorModel GetError() { return null; }
    }
}