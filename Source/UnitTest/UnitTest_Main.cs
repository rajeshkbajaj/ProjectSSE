using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covidien.CGRS.UnitTest
{
    class UnitTest_Main
    {
        static void Main(string[] args)
        {
            RunallTests();
        }

        public static void RunallTests()
        {
            UnitTest ut = new UnitTest();
            ut.CheckSoftwarePackages_Test1();
            ut.CheckSoftwarePackages_Test2();
            ut.CheckCopyFunctionality_Test3();
            ut.CheckCopyFunctionality_Test4();
            ut.CheckCopyFunctionality_Test5();
        }
    }
}
