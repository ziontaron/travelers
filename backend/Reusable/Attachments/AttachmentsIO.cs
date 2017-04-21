using Reusable.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Reusable.Attachments
{
    public class AttachmentsIO
    {
        public static List<Attachment> getAttachmentsFromFolder(string folderName, string attachmentKind)
        {
            List<Attachment> attachmentsList = new List<Attachment>();
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                string baseAttachmentsPath = ConfigurationManager.AppSettings[attachmentKind];
                if (folderName != "" && Directory.Exists(baseAttachmentsPath + folderName.Trim()))
                {
                    DirectoryInfo directory = new DirectoryInfo(baseAttachmentsPath + folderName);
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        Attachment attachment = new Attachment();
                        attachment.FileName = file.Name;
                        attachment.Directory = folderName;
                        attachmentsList.Add(attachment);
                    }
                }
            }
            return attachmentsList;
        }

        public static string CreateFolder(string baseDirectory)
        {
            string theNewFolderName = "";
            string currentPath;

            do
            {
                DateTime date = DateTime.Now;
                theNewFolderName = date.ToString("yy") + date.Month.ToString("d2") +
                                date.Day.ToString("d2") + "_" + MD5HashGenerator.GenerateKey(date);
                currentPath = baseDirectory + theNewFolderName;
            } while (Directory.Exists(currentPath));
            Directory.CreateDirectory(currentPath);
            return theNewFolderName;
        }

        public static void ClearDirectory(string targetDirectory)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDirectory);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + targetDirectory);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                return;
            }


            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
