using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using zubappxam.Models;
using zubappxam.Services;
//#define OFFLINE_SYNC_ENABLED

namespace zubappxam.Services
{
    public class AzureServices
    {
        //    MobileServiceClient client;
        //    IMobileServiceSyncTable<ZubItem> zubTable;

        //    public async Task Initialize()
        //    {
        //        if (client?.SyncContext?.IsInitialized ?? false)
        //        {
        //            return;
        //        }

        //        client = new MobileServiceClient(Constants.ApplicationURL);

        //        var fileName = "localdb.db";
        //        var store = new MobileServiceSQLiteStore(fileName);
        //        store.DefineTable<ZubItem>();

        //        await client.SyncContext.InitializeAsync(store);

        //        zubTable = client.GetSyncTable<ZubItem>();
        //    }

        //    public async Task SyncZub()
        //    {
        //        await Initialize();
        //    }

        //    public async Task<IEnumerable<ZubItem>> GetItems()
        //    {
        //        await Initialize();

        //        return null;
        //    }

        static AzureServices defaultInstance = new AzureServices();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<ZubItem> zubTable;
#else
        IMobileServiceTable<ZubItem> zubTable;
#endif

        const string offlineDbPath = @"localstore.db";

        private AzureServices()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<zubTable>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.zubTable = client.GetSyncTable<ZubItem>();
#else
            this.zubTable = client.GetTable<ZubItem>();
#endif
        }

        public static AzureServices DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return zubTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<ZubItem>; }
        }

        public async Task<ObservableCollection<ZubItem>> GetItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                //IEnumerable<ZubItem> items = await zubTable
                //    .Where(todoItem => !todoItem.Done)
                //    .ToEnumerableAsync();

                var data = await zubTable
                    .OrderBy(c => c.Nombre)
                    .ToEnumerableAsync();

                return new ObservableCollection<ZubItem>(data);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }

        public async Task SaveTaskAsync(ZubItem item)
        {
            try
            {
                if (item.Id == null)
                {
                    await zubTable.InsertAsync(item);
                }
                else
                {
                    await zubTable.UpdateAsync(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}
