using System;
using System.IO;
using System.Diagnostics;
using Covidien.CGRS.PcAgentInterfaceBusiness;
using Utilties;

namespace Covidien.CGRS.UnitTest
{
    class UnitTest
    {
        public void CheckSoftwarePackages_Test1()
        {
            Console.WriteLine("__________________Test-1 Starting_________________");
            bool testCasePassed = false;
            
            string projectpath = System.IO.Path.GetFullPath(@"..\..\");
            string dirPath = projectpath + "\\Inputs\\XMLSourceFiles\\VentilatorSW";
            if (Directory.Exists(dirPath))
            {
                string[] fileEntries = Directory.GetFiles(dirPath, "*.xml", SearchOption.AllDirectories);

                //For each file
                int i = 0;
                for (i = 0; i < fileEntries.Length; i++)
                {
                    string tagName = BusinessServicesBridge.GetConfigFileTag(fileEntries[i]);
                    bool expected = (tagName == null) || (String.Compare(tagName, "ventilator") == 0);
                    if (expected == false)
                    {
                        break;
                    }
                }

                if ((fileEntries.Length > 0) && 
                    (i == fileEntries.Length))
                {
                    testCasePassed = true;
                }
            }

            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-1 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();
        }

        public void CheckSoftwarePackages_Test2()
        {
            Console.WriteLine("__________________Test-2 Starting_________________");
            bool testCasePassed = false;

            string projectpath = System.IO.Path.GetFullPath(@"..\..\");
            string dirPath = projectpath + "\\Inputs\\XMLSourceFiles\\OtherSW";
            if (Directory.Exists(dirPath))
            {
                string[] fileEntries = Directory.GetFiles(dirPath, "*.xml", SearchOption.AllDirectories);
                int i = 0;
                for(i = 0; i < fileEntries.Length; i++)
                {
                    string tagName = BusinessServicesBridge.GetConfigFileTag(fileEntries[i]);
                    if ((tagName == null) || (String.Compare(tagName, "othersoftware") != 0))
                    {
                        break;
                    }
                }

                if ((fileEntries.Length > 0) &&
                    (i == fileEntries.Length))
                {
                    testCasePassed = true;
                }

            }

            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-2 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();
        }

        public void CheckCopyFunctionality_Test3()
        {
            Console.WriteLine("__________________Test-3_1 Starting_________________");
            bool testCasePassed = false; 

            string projectpath = System.IO.Path.GetFullPath(@"..\..\");

            string sourcedirPath = projectpath + "\\Inputs\\CopyDirs\\SourceDir";
            string destdirPath = projectpath + "\\Inputs\\CopyDirs\\DestDir";

            //Doing a fresh start for Dest directory
            DirectoryInfo dest = new DirectoryInfo(destdirPath);
            if (dest.Exists)
            {
                dest.Delete(true);
            }

            if (Directory.Exists(sourcedirPath))
            {
                //copy dirs
                FileSystemUtil.DirectoryCopy(sourcedirPath, destdirPath, true);
                testCasePassed = UnitTestUtils.DirectoryCompare(sourcedirPath, destdirPath);
            }

            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-3_1 {0}...!!!Press any key to continue...", testCasePassed? "PASSED" : "FAILED");
            Console.ReadLine();
            
            Console.WriteLine("__________________Test-3_2 Starting_________________");

            //this checks non-existent source directory - should lead to DirectoryNotFound exception
            string sourceNonExistentDirPath = "C:\\NonExistentSourceDir";
            string destDirForThisTestCase = "C:\\NonExistentDestDir";
            testCasePassed = false;
            try
            {
                FileSystemUtil.DirectoryCopy(sourceNonExistentDirPath, destDirForThisTestCase, true);
            }
            catch(DirectoryNotFoundException e)
            {
                if (e.ToString().Contains(sourceNonExistentDirPath))
                {
                    testCasePassed = true;
                }
            }
            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-3_2 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();
        }

        public void CheckCopyFunctionality_Test4()
        {
            Console.WriteLine("__________________Test-4 Starting_________________");
            bool testCasePassed = false;
            string sampleManifestDir = "C:\\TestDir";
            string expectedManifestFile = "C:\\TestDir\\config\\download.xml";
            string obtainedManifestFile = BusinessServicesBridge.Instance.GetManifestFileNameWithPath(sampleManifestDir);
            testCasePassed = (String.Equals(expectedManifestFile, obtainedManifestFile));
            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-4 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();

        }
        public void CheckCopyFunctionality_Test5()
        {
            Console.WriteLine("__________________Test-5_1 Starting_________________");
            bool testCasePassed = false;
            string rootDir = "C:\\TestDir";
            string packageName = "C:\\TestDir\\10105678_AG.pkg";
            string expectedPackageDirPath = "C:\\TestDir\\10105678_AG";

            string obtainedPackageDirPath = BusinessServicesBridge.GetSoftwareSavePath(rootDir, packageName);

            testCasePassed = (String.Equals(expectedPackageDirPath, obtainedPackageDirPath));
            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-5_1 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();

            Console.WriteLine("__________________Test-5_2 Starting_________________");
            packageName = "10105678_AG";

            obtainedPackageDirPath = BusinessServicesBridge.GetSoftwareSavePath(rootDir, packageName);

            testCasePassed = (String.Equals(expectedPackageDirPath, obtainedPackageDirPath));
            Debug.Assert(testCasePassed == true);
            Console.WriteLine("Unit Test-5_2 {0}...!!!Press any key to continue...", testCasePassed ? "PASSED" : "FAILED");
            Console.ReadLine();
        }

    }
}
