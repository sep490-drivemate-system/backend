using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Enum
{
    public enum EmailType
    {
        VerifyOPTCode,
        ForgotPassword,  
        InstructorRegistration,
        InstructorReschedule,
        RejectSession,
        DriverReschedule,
        PostRejected,
        WithdrawRejected,
        BanUser
    }
}
