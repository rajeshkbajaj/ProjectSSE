// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Utilties
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using Serilog;

    public class FileSystemUtil
    {
        /// <summary>
        ///     Copies the contect of source directory to destination directory .
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var sourcedir = new DirectoryInfo(sourceDirName);

            if (!sourcedir.Exists)
            {
                throw new DirectoryNotFoundException(
                                                     "Source directory does not exist: "
                                                     + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            CreateDirectoryIfNotExists(destDirName);

            // Get the files in the directory and copy them to the new location.
            foreach (var file in sourcedir.GetFiles())
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subdir in sourcedir.GetDirectories())
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        ///     Checks for the existence of a directory and deletes the directory with recursive option.
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteDirectory(string dirPath)
        {
            try
            { 
                var source = new DirectoryInfo(dirPath);
                if (source.Exists)
                {
                    source.Delete(true);
                }
            }
            catch (Exception e)
            {
                Log.Information($"FileSystemUtil::DeleteDirectory dir:{dirPath} Exception:{e.Message}");
            }
        }

        public static bool CreateDirectoryIfNotExists(string dirName)
        {
            try
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                    SetDirectoryAccessControl(dirName);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Unale to create Directory {dirName}. Error: {e.Message}");
            }

            return false;
        }

        public static bool CreateFileIfNotExists(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    SetDirectoryAccessControl(Path.GetDirectoryName(filePath));
                    using (var filePtr = new StreamWriter(Path.GetFileName(filePath)))
                    {
                        Log.Warning("Created {0} as it is not present", filePath);

                        filePtr.Close();
                        SetFileAccessControl(filePath);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Unale to create File {filePath}. Error: {e.Message}");
            }

            return false;
        }

        public static bool SetDirectoryAccessControl(string path)
        {
            try
            {
                if (IsAdministrator())
                {
                    var sec = (new DirectoryInfo(path)).GetAccessControl();
                    var everyone = new SecurityIdentifier(WellKnownSidType.LocalSid, null);
                    sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                    (new DirectoryInfo(path)).SetAccessControl(sec);
                }
                return true;
            }
            catch (Exception exp)
            {
                Log.Error($"Failed to set the permissions for {path}. Error {exp.Message}");
            }
            return false;
        }
        public static bool SetFileAccessControl(string filename)
        {
            try
            {
                FileSecurity sec = new FileInfo(filename).GetAccessControl();
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.LocalSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                new FileInfo(filename).SetAccessControl(sec);
                return true;
            }
            catch (Exception exp)
            {
                Log.Error($"Failed to set the permissions for {filename}. Error {exp.Message}");
            }
            return false;
        }

        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}