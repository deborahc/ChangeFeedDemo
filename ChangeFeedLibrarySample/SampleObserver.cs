using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Newtonsoft.Json;

namespace ChangeFeedLibrarySample
{
    class SampleObserver : IChangeFeedObserver
    {
        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;  // Note: requires targeting .Net 4.6+.
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> docs, CancellationToken cancellationToken)
        {
            Console.WriteLine("ProcessChangesAsync: partition {0}, {1} docs", context.PartitionKeyRangeId, docs.Count);

            foreach (var doc in docs)
            {
                Console.WriteLine("Changed document: {0}", doc);
                var vehicleRecord = JsonConvert.DeserializeObject<VehicleRecord>(doc.ToString());
                Console.WriteLine("Changed document event name: {0}", vehicleRecord.EventName);
                // You can write any logic here
                // E.g. write a new document, send to another service
            }

            return Task.CompletedTask;
        }
    }
}
