using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SpeechLib;

namespace AppDevTest
{
    public class AppDevComInterop
    {
        /// <summary>
        /// References the COM dll SpeechLib.dll, when run speaks whatever is passed in as string
        /// </summary>
        public static void TestComMethod()
        {
            SpVoice voice = new SpVoice();
            voice.Speak("Hello World", SpeechVoiceSpeakFlags.SVSFDefault);
        }

        // Import and declare a COM method
        [DllImport("user32")] // can be the name if the dll is included in the assembly
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        // Import and declare a COM method however specify a different name for the method.
        //[DllImport(@"c:\windows\system32\user32.dll", EntryPoint = "MessageBox")]
        //[DllImport("user32.dll", EntryPoint = "MessageBox")]
        [DllImport("user32", EntryPoint = "MessageBox")]
        public static extern int ShowMessage(IntPtr hWnd, String text, String caption, uint type);

        // Using dllimportattribute, if you dont specify the path the dll should be in one of:
        //the same directory as your EXE
        //the directory specified in SetDllDirectory(), if used
        //the system directory (c:\windows\system32 by default)
        //the 16-bit system directory (c:\windows\system by default)
        //the current default directory (Environment.CurrentDirectory)
        //one of the directories listed in the PATH environment variable



        // This method can either use 'MessageBox' or 'ShowMessage' to do the same thing...
        public static void TestMessageBox()
        {
            //MessageBox(new IntPtr(0), "Hello, World", "Message Box", 0);
            ShowMessage(new IntPtr(0), "Hello, World", "Message Box", 0);
        }

        public static void ThrowCustomException()
        {
            throw new CustomException();
        }

    }


    // The following exception class can be used to immediately return control to a calling COM class
    public class CustomException : ApplicationException
    {
        public static int COR_E_ARGUMENT = unchecked((int)0x0402983);
        public CustomException()
        {
            HResult = COR_E_ARGUMENT;
        }
    }
}
