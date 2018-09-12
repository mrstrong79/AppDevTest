using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppDevTest.DomainObjects;

namespace AppDevTest
{
    #region SimpleDelegateTest

    // Declaration
    public delegate void SimpleDelegate();

    public class EventsAndDelegates
    {
        public static void MyFunc()
        {
            Console.WriteLine("I was called by delegate ...");
        }

        /// <summary>
        /// Simple delegate example:
        /// 1) Declaration (at top) define the delegate signature
        /// 2) Inantiation
        /// 3) Invocation
        /// </summary>
        public static void SimpleDelegateTest()
        {
            // Instantiation
            SimpleDelegate simpleDelegate = new SimpleDelegate(MyFunc);

            // Invocation
            simpleDelegate();
        }

        /// <summary>
        /// Example that uses System.EventHandler as the delegate
        /// </summary>
        public static void TestCountDown()
        {
            Countdown countdown = new Countdown(20);
            CountdownMessage cm = new CountdownMessage();
            countdown.CountdownCompleted += new EventHandler(cm.SendCountdownMessage);
            countdown.Decrement();
        }

        /// <summary>
        /// Example that creates a delegate and then an event based off that delegate type
        /// </summary>
        public static void TestMetronome()
        {
            Metronome m = new Metronome();
            Listener l = new Listener();
            l.Subscribe(m);
            m.Start();
        }

        /// <summary>
        /// My own test using events and delegates
        /// </summary>
        public static void ClonePerson()
        {
            Person p = new Person("John", "Smith");
            PersonSubscriber ps = new PersonSubscriber(p);
            ps.ClonePerson();
        }


    }

#endregion

    #region MetronomeExample

    // Test method found above in the class 'EventsAndDelegates'

    public class Metronome
    {
        public EventArgs e = null;
        public delegate void TickHandler(Metronome m, EventArgs e);
        public event TickHandler Tick;
        public void Start()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(3000);
                if (Tick != null) // make sure the event is not null
                {
                    Tick(this, e);
                }
            }
        }
    }
    public class Listener
    {
        public void Subscribe(Metronome m)
        {
            m.Tick += new Metronome.TickHandler(HeardIt);
        }
        private void HeardIt(Metronome m, EventArgs e)
        {
            Console.WriteLine("HEARD IT");
        }

    }

#endregion

    #region MSDNExample - using System.EventHandler

    // First create the Countdown class as:

    public class Countdown
    {
        public event EventHandler CountdownCompleted; // Create the event using 'System.EventHandler' as the delegate type

        public Countdown(int internalCounter)
        {
            InternalCounter = internalCounter;
        }

        public int InternalCounter {get; set; }
        public bool Stop { get; set; }

        protected virtual void OnCountdownCompleted(EventArgs e)
        {
            if (CountdownCompleted != null)
            {
                CountdownCompleted(this, e);
            }
        }

        public void Decrement()
        {
            int loopCounter = 0;
            for (; ; )
            {
                if (loopCounter == 0)
                {
                    Console.WriteLine("{0}", InternalCounter);
                }
                if (Stop)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    InternalCounter = InternalCounter - 1;
                    Console.WriteLine("{0}", InternalCounter);
                    if (InternalCounter == 0)
                        OnCountdownCompleted(new EventArgs());
                }
                loopCounter = loopCounter + 1;
            }

        }
    }

    // Then create an event handler class with a method that will send a message when the CountdownCompleted event fires.
    public class CountdownMessage
    {
        public void SendCountdownMessage(Object sender, EventArgs e)
        {
            Console.WriteLine("The countdown has completed.");
            ((Countdown)sender).Stop = true;
            return;
        }
    }

#endregion

    #region PersonExample

    public class PersonSubscriber
    {
        public Person person { get; set; }
       // public Person newPerson { get; set; }
        public PersonSubscriber(Person p)
        {
            person = p;
        }


        public void ClonePerson()
        {
            person.PersonCloned += new PersonDelegate(LastNameChanged);
            person.PersonCloned += new PersonDelegate(FirstNameChanged);
          //  person.PersonCloned += new PersonDelegate(AgeChanged);
            person.Clone();
        }

        public bool LastNameChanged(Person p)
        {
            if (p.LastName == person.LastName)
            {
                Console.WriteLine("Surnames are the same");
                return true;
            }
            Console.WriteLine("Surnames are NOT the same");
            return false;

        }

        public bool FirstNameChanged(Person p)
        {
            if (p.FirstName == person.FirstName)
            {
                Console.WriteLine("First names are the same");
                return true;
            }
            Console.WriteLine("First names are NOT the same");
            return false;
        }

        //public bool AgeChanged(Person p)
        //{
            
        //}
    }

    #endregion

}
