using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace AppDevTest
{
    class AppDevSecurity
    {
        public static void FileSecurityDemo()
        {
            try
            {
                string fileName = "test.xml";

                Console.WriteLine("Adding access control entry for "
                    + fileName);

                // Add the access control entry to the file.
                AddFileSecurity(fileName, @"DomainName\AccountName",
                    FileSystemRights.ReadData, AccessControlType.Allow);

                // Add the access control entry to the file.
                AddFileAuditRule(fileName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AuditFlags.Failure);


                Console.WriteLine("Removing access control entry from "
                    + fileName);

                // Remove the access control entry from the file.
                RemoveFileSecurity(fileName, @"DomainName\AccountName",
                    FileSystemRights.ReadData, AccessControlType.Allow);

                // Remove the access control entry from the file.
                RemoveFileAuditRule(fileName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AuditFlags.Failure);

                Console.WriteLine("Done.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        // Adds an ACL entry on the specified file for the specified account.
        public static void AddFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the 
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings. 
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        public static void AddFileAuditRule(string fileName, string Account, FileSystemRights Rights, AuditFlags AuditRule)
        {
            FileInfo fi = new FileInfo(fileName);

            // Get a FileSecurity object that represents the 
            // current security settings.
            FileSecurity fSecurity = fi.GetAccessControl();

            // Add the FileSystemAuditRule to the security settings. 
            fSecurity.AddAuditRule(new FileSystemAuditRule(Account,
                                                            Rights,
                                                            AuditRule));


            fi.SetAccessControl(fSecurity);

        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the 
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);  // FileInfo.GetAccessControl would work aswell here if you wanted to create a FileInfo object...

            // Add the FileSystemAccessRule to the security settings. 
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileAuditRule(string FileName, string Account, FileSystemRights Rights, AuditFlags AuditRule)
        {

            // Get a FileSecurity object that represents the 
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(FileName);

            // Add the FileSystemAuditRule to the security settings. 
            fSecurity.RemoveAuditRule(new FileSystemAuditRule(Account,
                                                            Rights,
                                                            AuditRule));

            // Set the new access settings.
            File.SetAccessControl(FileName, fSecurity);

        }

        #region directory security
        public static void DirectorySecurityExample()
        {
            try
            {
                string DirectoryName = "TestDirectory";

                Console.WriteLine("Adding access control entry for " + DirectoryName);

                // Add the access control entry to the directory.
                AddDirectorySecurity(DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow);

                Console.WriteLine("Removing access control entry from " + DirectoryName);

                // Remove the access control entry from the directory.
                RemoveDirectorySecurity(DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow);

                Console.WriteLine("Done.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        // Adds an ACL entry on the specified directory for the specified account.
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            //DirectorySecurity dSecurity = Directory.GetAccessControl(FileName);

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                            Rights,
                                                            ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }

        // Removes an ACL entry on the specified directory for the specified account.
        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(Account,
                                                            Rights,
                                                            ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);
            // Directory.SetAccessControl(Path.GetFullPath(FileName), dSecurity); // Could also use the static method of SetAccessControl from Directory class here...

        }

        #endregion
    }
}
