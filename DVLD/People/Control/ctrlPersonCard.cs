using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Properties;
using DVLD_BusinessTier;
using static DVLD.frmAddUpdatePerson;

namespace DVLD.People.Control
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        private clsPerson _Person;
        public int PersonID 
        {
            get { return _PersonID; }
        }
        public clsPerson SelectedPersonInfo 
        {
            get { return _Person; }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        public void LoadPersonData(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if(_Person == null) 
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with Person ID. = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        public void LoadPersonData(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        public void ResetPersonInfo() 
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbGendor.Image = Resources.Male_512;
        }
        private void _FillPersonInfo() 
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblName.Text = _Person.FullName;
            pbGendor.ImageLocation = _Person.ImagePath;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text =_Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            lblAddress.Text = _Person.Address;
            _LoadPersonImage();
        }
        private void _LoadPersonImage() 
        {
          
            if (_Person.Gendor == 0) pbGendor.Image = Resources.Male_512; 
            else pbGendor.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if(ImagePath != "") 
            {
                if (File.Exists(ImagePath)) pbGendor.ImageLocation = ImagePath;
                else MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();
            //Refresh Data
            LoadPersonData(_PersonID);
        }
    }
}
