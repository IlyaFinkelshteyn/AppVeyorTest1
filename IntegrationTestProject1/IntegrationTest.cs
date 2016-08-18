using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class IntegrationTest
    {
        [AssemblyInitialize]
        public static void InitialiseAllTests(TestContext content)
        {
            //Run once at start of the test run, before all tests
            RunBatchCommands();
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        private static void RunBatchCommands()
        {
            // Look in the solution for this batch file
            var batchFilePath =
                Environment.ExpandEnvironmentVariables(Environment.CurrentDirectory +
                                                       @"\..\..\..\DoCommandLineStuff.bat");
            var startInfo = new ProcessStartInfo(batchFilePath, @"anip acertpath q")
            {
                UseShellExecute = true,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                Verb = @"runas" // needs to run elevated
            };

            using (var process = new Process {StartInfo = startInfo})
            {
                process.Start();

                bool isWow64Process;
                if (!IsWow64Process(process.Handle, out isWow64Process))
                    throw new Exception(Marshal.GetLastWin32Error());
                Console.WriteLine("isWow64Process: " + isWow64Process);

                process.WaitForExit(); //We need to wait until this process completes and the CMD window closes
            }
        }

        [TestMethod, TestCategory("Integration")]
        public void ThisTestShouldNotBeRun()
        {
            // We should not get here, because the batch file should have not completed.
        }
    }
}