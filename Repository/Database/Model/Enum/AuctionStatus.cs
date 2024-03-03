using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.Enum
{
    public enum AuctionStatus
    {
        NOT_STARTED, ONGOING, PENDING_PAYMENT, SUCCESS, CANCELLED,FAILED_TO_PAY
    }
}
