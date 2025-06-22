using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public static class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid guid = new Guid();
            return guid.ToString();
        }
        public static bool CreateFolderIfDoesNotExist(string FolderPath) 
        {
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }return true;
        }
        public static string ReplaceFileNameWithGUID(string SourceFile)
        {
            string FileName = SourceFile;
            FileInfo FileInfo = new FileInfo(FileName);
            string extn = FileInfo.Extension;
            return GenerateGUID() + extn;
        }
        public static bool CopyImageToProjectImagesFolder(ref string SourceFile) 
        {
            string FolderPath = "C:/DVLD-People-Images/";
            if (!CreateFolderIfDoesNotExist(FolderPath)) return false;
            string DestinationFile = FolderPath + ReplaceFileNameWithGUID(SourceFile);
            try
            {
                File.Copy(SourceFile, DestinationFile, true);
            }
            catch (IOException iox) 
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourceFile = DestinationFile;
            return true;
        }
    }
}
