using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MemberBLL
    {
        MemberDAO dao = new MemberDAO();

        public Member GetMemberByMemberID(int memberID)
        {
            return dao.GetMemberByMemberID(memberID);
        }
    }
}
