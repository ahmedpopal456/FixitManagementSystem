using System;
using System.Collections.Generic;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    enum FixStatusEnum
    {
        New,
        Pending,
        InProgress,
        InReview,
        Done
    }
}
