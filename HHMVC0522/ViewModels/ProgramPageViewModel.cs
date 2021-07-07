using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.ViewModels
{
    public class ProgramPageViewModel
    {

        ProgramBLL pBLL = new ProgramBLL();
        private int _memberID;
        private bool _hasActiveProgram;
        private RegisterProgramViewModel _registerProgramViewModel;
        private ShowProgramViewModel _showProgramViewModel;

        public ProgramPageViewModel(int memberID) {

            _memberID = memberID;
            _hasActiveProgram = pBLL.HasActiveProgram(_memberID);
            if (_hasActiveProgram)
            { _showProgramViewModel = new ShowProgramViewModel(_memberID);}
            else { _registerProgramViewModel = new RegisterProgramViewModel(_memberID); }
        }


        public int MemberID { get { return _memberID; } }

        public string Today { get { return DateTime.Now.ToString(CDictionary.MMddyyyy); } }


        public bool HasActiveProgram { get { return _hasActiveProgram; } }

        public RegisterProgramViewModel RegisterView { get{ return _registerProgramViewModel; } }

        public ShowProgramViewModel ProgramView { get { return _showProgramViewModel; } }




    }


}
