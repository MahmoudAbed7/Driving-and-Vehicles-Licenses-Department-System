using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessTier;

namespace DVLD.Users.Control
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID;
        public int UserID { get { return _UserID; } }
        private clsUser _User;
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserData(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(UserID);
            if(_User == null) 
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillUserInfo();
        }
        private void _FillUserInfo() 
        {
            ctrlPersonCard1.LoadPersonData(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();

            if (_User.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }
        private void _ResetPersonInfo() 
        {
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}
