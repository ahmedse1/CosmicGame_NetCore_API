using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Response
{
    public class tblUserResponse
    {
        public int WrUserId { get; set; }
        public string WrUserName { get; set; }
        public string WrEmail { get; set; }
        public string WrMobile { get; set; }
        public string WrPassword { get; set; }
        public bool WrIsApproved { get; set; }
        public bool? WrIsActive { get; set; }

    }
}
