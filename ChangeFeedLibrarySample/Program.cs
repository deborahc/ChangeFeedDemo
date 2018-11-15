using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.ChangeFeedProcessor;

namespace ChangeFeedLibrarySample
{
    class ChangeFeedProcessorSample
    {
        static void Main(string[] args)
        {
            var p = new ChangeFeedProcessorSample();
            p.Run();
        }
        public void Run()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            DocumentCollectionInfo feedCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "DemoDatabase",
                CollectionName = "DemoCollection",
                Uri = new Uri("endpoint"),
                MasterKey = ""
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "DemoDatabase",
                CollectionName = "leases",
                Uri = new Uri("endpoint"),
                MasterKey = "key"
            };

            ChangeFeedProcessorOptions feedProcessorOptions = new ChangeFeedProcessorOptions();

            // ie. customizing lease renewal interval to 15 seconds
            // can customize LeaseRenewInterval, LeaseAcquireInterval, LeaseExpirationInterval, FeedPollDelay 
            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(15);
            feedProcessorOptions.StartFromBeginning = true;
            //feedProcessorOptions.StartTime = DateTime.Now - TimeSpan.FromDays(1);

            var builder = new ChangeFeedProcessorBuilder();
            var processor = await builder
                .WithHostName("SampleHost")
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithObserver<SampleObserver>() // add as many sample observers (aka consumers as needed) via WithObserver<>
                .WithProcessorOptions(feedProcessorOptions)
                .BuildAsync();

            await processor.StartAsync();

            Console.WriteLine("Change Feed Processor started. Press <Enter> key to stop...");
            Console.ReadLine();

            await processor.StopAsync();
        }
    }
}