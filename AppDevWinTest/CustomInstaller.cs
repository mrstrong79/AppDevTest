using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Configuration.Install;
using System.IO;

namespace AppDevWinTest
{
    [RunInstaller(true)]
    public class CustomInstaller : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            File.CreateText("Install.txt");
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
            File.CreateText("Commit.txt");
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            File.Delete("Commit.txt");
            File.Delete("Install.txt");
        }
        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);
            File.Delete("Install.txt");
        }

    }
}
