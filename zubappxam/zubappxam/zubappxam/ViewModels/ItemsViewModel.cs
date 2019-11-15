using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using zubappxam.Models;
using zubappxam.Views;
using zubappxam.Services;

namespace zubappxam.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Tareas Realizadas";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AzureServices manager;
            manager = AzureServices.DefaultManager;

            MessagingCenter.Subscribe<NewItemPage, ZubItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ZubItem;
                //Items.Add(newItem);
                //await DataStore.AddItemAsync(newItem);
                //public async Task SaveTaskAsync(ZubItem item)
                await manager.SaveTaskAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}