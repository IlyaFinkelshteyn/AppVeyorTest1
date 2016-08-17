using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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