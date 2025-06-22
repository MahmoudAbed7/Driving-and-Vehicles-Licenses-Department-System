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

namespace DVLD
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0, Update = 1 }
        public enum enGendor {Male = 0, Female = 1}
        enMode Mode = enMode.AddNew;
        private int _PersonID;
        private clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            Mode = enMode.Update;
        }
        private void _FillCountriesInComboBox() 
        {
            DataTable dt = clsCountry.GetAllCountries();
            foreach (DataRow dr in dt.Rows) 
            {
                cbCountry.Items.Add(dr["CountryName"]);
            }
        }
        private void _ResetDefualtValue ()
        {
            _FillCountriesInComboBox();
            if(Mode == enMode.AddNew) 
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else lblTitle.Text = "Edit Person";

            if (rbMale.Checked) pbPersonImage.InitialImage = Resources.Male_512;
            else pbPersonImage.InitialImage = Resources.Female_512;

            llRemoveImage.Visible = pbPersonImage.ImageLocation != null;

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            rbMale.Checked = true;
        }
        private void _LoadData() 
        {
            _Person = clsPerson.Find(_PersonID);
            if (_Person == null) 
            {
                MessageBox.Show("No Person With ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            if (_Person.Gendor == 0) rbMale.Checked = true;
            else rbFemale.Checked = true;

            if (_Person.ImagePath != "") pbPersonImage.ImageLocation = _Person.ImagePath;

            llRemoveImage.Visible = _Person.ImagePath != "";
        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValue();
            if(Mode == enMode.Update) _LoadData();
        }
        private bool _HandlePersonImage()
        {
            if(_Person.ImagePath != pbPersonImage.ImageLocation) 
            {
                if(_Person.ImagePath != "") 
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException) { }
                }
            }
            if (pbPersonImage.ImageLocation != null)
            {
                string SourceFilePath = pbPersonImage.ImageLocation.ToString();

                if (clsUtil.CopyImageToProjectImagesFolder(ref SourceFilePath))
                {
                    pbPersonImage.ImageLocation = SourceFilePath;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) 
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!_HandlePersonImage()) return;

            int NationalCountryID = clsCountry.Find(cbCountry.Text).ID;
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.Email = txtEmail.Text;
            _Person.Phone = txtPhone.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked) _Person.Gendor = (short)enGendor.Male;
            else _Person.Gendor = (short)enGendor.Female;
            _Person.NationalityCountryID = NationalCountryID;

            if (pbPersonImage.ImageLocation != null) _Person.ImagePath = pbPersonImage.ImageLocation;
            else _Person.ImagePath = "";

            if (_Person.Save()) 
            {
                lblTitle.Text = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();
                Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else 
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.png;*.jpej;*.jpg;*.bmp;*.gif";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                string ImagePath = openFileDialog1.FileName;
                pbPersonImage.Load(ImagePath);
                llRemoveImage.Visible = true;
            }
        }
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;
            if (string.IsNullOrEmpty(Temp.Text)) 
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required");
            }
            else 
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(txtEmail.Text.Trim() == "") { return; }

            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim())) 
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim())) 
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is requied");
            }
            else 
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
    }
}
