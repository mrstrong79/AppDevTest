using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AppDevTest
{
    public class Threading
    {
        public static void TestThreadPoolNoParam()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
            Console.WriteLine("Main thread starts");
            Thread.Sleep(10000);
            Console.WriteLine("Main thread exits.");
        }

        public static void ThreadProc(Object stateInfo)
        {
            Console.WriteLine("Hello from the thread pool. I will count from 1 to 100");
            for (int counter = 1; counter < 101; counter++)
            {
                Console.WriteLine(counter);
                Thread.Sleep(500);
            }
        }

        public static void TestThreadPoolWithParam()
        {
            string s = "Hello from the thread pool";
            ThreadPool.QueueUserWorkItem(ThreadProcess, s);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProcess), s);

            Console.WriteLine("Main Thread does some stuff...");
            Thread.Sleep(1000);

            Console.WriteLine("Main thread exits");
        }
        
        public static void ThreadProcess(Object stateInfo)
        {
            string s = (string)stateInfo;
            Console.WriteLine(s);
        }

        public static void showThread()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(10);
            }
        }

        public static void MultipleThreads()
        {
            ThreadStart operation = new ThreadStart(showThread);

            for (int i = 0; i < 5; i++)
            {
                // Creates, but does not start, a new thread 
                Thread theThread = new Thread(operation);

                // Starts the work on a new thread
                theThread.Start();
            }
        }




        public static void TestThreading()
        {
            Thread DoWorkThread = new Thread(new ThreadStart(DoWork));

            DoWorkThread.Start();

            Thread.Sleep(1000);

            DoWorkThread.Abort();

            Console.WriteLine("The main thread is ending...");
            Thread.Sleep(4000);
        }

        public static void DoWork()
        {
            Console.WriteLine("Here we are running in the DoWork method");

            try
            {
                Thread.Sleep(3000);
            }
            catch (Exception)
            {
                Console.WriteLine("DoWork has been aborted");
                
            }
            finally
            {
                Console.WriteLine("Use finally to clean up resources");
            }
            Console.WriteLine("DoWork has ended");
        }



        public static void TestLocks()
        {
            MemFile m = new MemFile();
            Thread t1 = new Thread(new ThreadStart(m.ReadFile));
            Thread t2 = new Thread(new ThreadStart(m.WriteFile));
            Thread t3 = new Thread(new ThreadStart(m.ReadFile));
            Thread t4 = new Thread(new ThreadStart(m.ReadFile));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
        }

        public static void TestInterlock()
        {
            MyNum n = new MyNum();
            for (int a = 0; a < 10; a++)
            {
                for(int i=0;i<1000;Interlocked.Increment(ref i))
                {
                    Thread t = new Thread(new ThreadStart(n.AddOne));
                    t.Start();
                }
                Thread.Sleep(3000);
                Console.WriteLine(n.number);
            }
        }


        private static AutoResetEvent[] waitHandles = new AutoResetEvent[]
                                                          {
                                                              new AutoResetEvent(false), new AutoResetEvent(false),
                                                              new AutoResetEvent(false)
           
                                                          };   
        public static void TestWaitAll()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), new ThreadInfo(3000, waitHandles[0]));
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), new ThreadInfo(2000, waitHandles[1]));
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), new ThreadInfo(1000, waitHandles[2]));
            Thread.CurrentThread.GetApartmentState();
            WaitHandle.WaitAll(waitHandles); // Wait for all our threads to complete...
            Console.WriteLine("Main thread is complete.");
            Console.ReadKey();
        }



        private static void DoTask(Object state)
        {
            ThreadInfo t = (ThreadInfo) state;
            Thread.Sleep(t.ms);
            Console.WriteLine("Waited for " + t.ms.ToString() + " ms.");
            t.are.Set();
        }

        public static void TestThreadingWithResult()
        {
            Multiply m = new Multiply("Hello World", 13, new ResultDelegate(ResultCallback));

            Thread t = new Thread(new ThreadStart(m.ThreadProc));
            t.Start();
            Console.WriteLine("Main thread does some work, then waits");
            t.Join(); // wait for the thread to complete.  Works when waiting for a single thread...
            Console.WriteLine("Thread completed");
        }

        public static void ResultCallback(int retVal)
        {
            Console.WriteLine("Returned value: {0}", retVal);
        }

    }



    public delegate void ResultDelegate(int value);

    public class Multiply
    {
        private string greeting;
        private int value;

        private ResultDelegate callback;

        public Multiply(string _greeting, int _value, ResultDelegate _callback)
        {
            greeting = _greeting;
            value = _value;
            callback = _callback;
        }

        public void ThreadProc()
        {
            Console.WriteLine(greeting);
            if (callback != null)
                callback(value*2);
        }
    }

    public class MemFile
    {
        string file = "Hello World!";
        ReaderWriterLock rwl = new ReaderWriterLock();

        public void ReadFile()
        {
            //lock(this)
            rwl.AcquireReaderLock(10000); // Allow a thread to continue only if no other thread has a write lock...
            {
                for (int i=0;i<=3;i++)
                {
                    Console.WriteLine(file);
                    Thread.Sleep(1000);
                }
            }
            rwl.ReleaseReaderLock();
        }

        public void WriteFile()
        {
            //lock (this)
            rwl.AcquireWriterLock(10000); // Allows a thread to continue only if no other thread has a reader or writer lock
            {
                file += " It's a nice day!";
            }
            rwl.ReleaseWriterLock();
        }

    }

    public class MyNum
    {
        public int number = 0;
        public void AddOne()
        {
            //number++; // In a threading situation this could get read and incremented by any number of threads leading to the wrong values being used as they are out of date...
            // Need to use Interlocked.Increment to avoid this.
            Interlocked.Increment(ref number);
        }
    }

    public class ThreadInfo
    {
        public AutoResetEvent are;
        public int ms;

        public ThreadInfo(int _ms, AutoResetEvent _are)
        {
            ms = _ms;
            are = _are;
        }
    }

        // From MSDN - asynchronous with callback

    //The BeginInvoke method initiates the asynchronous call. It has the same parameters as the method you want to execute asynchronously, 
    //plus two additional optional parameters. The first parameter is an AsyncCallback delegate that references a method to be called when 
    // the asynchronous call completes. The second parameter is a user-defined object that passes information into the callback method. 
    // BeginInvoke returns immediately and does not wait for the asynchronous call to complete. BeginInvoke returns an IAsyncResult, which 
    // can be used to monitor the progress of the asynchronous call.

    public class AsyncDemo
    {
        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
    }
    // The delegate must have the same signature as the method
    // it will call asynchronously.
    public delegate string AsyncMethodCaller(int callDuration, out int threadId);

    public class AsyncMain
    {

        public static void AsyncCallWithEndInvoke()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Call EndInvoke to wait for the asynchronous call to complete,
            // and to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }


        public static void AsyncCallWithWaitHandle()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }

        public static void AsyncCallCompletion()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            // Poll while simulating work.
            while (result.IsCompleted == false)
            {
                Thread.Sleep(10);
            }

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }

        // Asynchronous method puts the thread id here.
        private static int threadId;

        public static void AsyncCallBackOnCompletion()
        {
            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.  Include an AsyncCallback
            // delegate representing the callback method, and the data
            // needed to call EndInvoke.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId,
                new AsyncCallback(CallbackMethod),
                caller);

            Console.WriteLine("Press Enter to close application.");
            Console.ReadLine();
        }

        // Callback method must have the same signature as the
        // AsyncCallback delegate.
        static void CallbackMethod(IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncMethodCaller caller = (AsyncMethodCaller)ar.AsyncState;

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, ar);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }




    }


}
