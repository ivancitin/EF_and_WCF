# EF_and_WCF

**This is a Visual Studio for Mac project, targeting .NET Framework 4.7.1**

I need help in adding an Inter Process Communication Service (IPC) to an existing application, the existing application uses EF to access an SQLite DB. The IPC service will not interact with the DB at all.

But for some reason i'm not able to figure out, adding the IPC service using WCF makes the application to crash. The IPC service is interfering with the EF context somehow, in a way i haven't been able to find out.

If you run the tests in this example, it will work just find, but in the test class, changing the order to initialize the IPC service first, make all the tests to fail, with the error: `OneTimeSetUp: System.NotSupportedException : Specified method is not supported.`

Looking for someone who has faced the same issue, and solved it, willing to help me.

Thanks
