using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;


namespace CS4227_Database_API.Interceptors
{
    public class FactoryCallLoggingInterceptor : IInterceptor
    {
        TextWriter writer;
        public FactoryCallLoggingInterceptor(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void Intercept(IInvocation invocation)
        {
            var name = ($"{invocation.Method.DeclaringType}.{invocation.Method.Name}");
            List<string> args = (List<string>)invocation.GetArgumentValue(0);
            string argVals = "";
            foreach(string i in args)
            {
                argVals += i + "; ";
            }
            //writer.WriteLine(args);

            writer.WriteLine();
            writer.WriteLine($"Calling: {name}");
            writer.WriteLine($"Args: {argVals}");

            var watch = Stopwatch.StartNew();
            invocation.Proceed(); //Intercepted method is executed here.
            watch.Stop();
            var executionTime = watch.ElapsedMilliseconds;

            writer.WriteLine($"Done: result was {invocation.ReturnValue}");
            writer.WriteLine($"Execution Time: {executionTime} ms.");
            writer.WriteLine();
        }
    }
}
