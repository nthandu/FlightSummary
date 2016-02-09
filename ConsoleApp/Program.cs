namespace FlightSummary
{
    using System;
    using Microsoft.Practices.Unity;
    using Core.Interfaces;
    using Core.Validators;

    class Program
    {
        static void Main(string[] args)
        {
            // not followed TDD / written any tests for console app
            // console app - a client which is using Core library

            if(args==null)
                return;

            if(args.Length<2)
                return;

            var sourceFilePath = args[0];
            var targetPath = args[1];

            using (var container = new UnityContainer())
            {
                RegisterTypes(container);

                var fileReader = container.Resolve<Core.Readers.SimpleFileReader>();
                var abnfFileProcessor = container.Resolve<Core.Processors.AbnfContentProcessor>();
                var outPutWriter = container.Resolve<Core.Output.AbnfOutputProducer>();

                // read file
                var fileContent = fileReader.Read(sourceFilePath);

                // invoke abnf file processor from above read lines
                var summary = abnfFileProcessor.Process(fileContent);
                
                // get summary as text
                var outputAsText = outPutWriter.GetSummaryAsText(summary);
                // TODO : SimpleFileWriter - Should be implementing interface
                // 
                var fileWrtiter = new Core.Writers.SimpleFileWriter();
                fileWrtiter.SaveSummary(targetPath, outputAsText);

                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }

        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<ISummary, Core.Models.Summary>();
            container.RegisterType<ISummaryFormatter, Core.Fomatters.SimpleSummaryFormatter>();
            container.RegisterType<IReader, Core.Readers.SimpleFileReader>();
            container.RegisterType<IInputValidator, SimpleInputValidator>();
        }
    }
}
