using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public record DomainError(ErrorCode ErrorCode, string Message);

    public enum ErrorCode
    {
        CarNotFound
    }
}
