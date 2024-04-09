using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public interface IPeriod
    {
        DateOnly StartDate { get; }
        DateOnly EndDate { get; }
    }
}