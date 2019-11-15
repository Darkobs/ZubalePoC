using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using zubappxam.Models;

namespace zubappxam.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public ZubItem zubItem { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            zubItem = new ZubItem
            {
                Nombre = "Escribe tu nombre",
                Correo = "@correo.com",
                Empresa = "Elige empresa",
                Direccion = "Ciudad de México",
                Cuestionario = "Marketing"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            zubItem.Nombre = EntryNombre.Text;
            zubItem.Correo = EntryCorreo.Text;
            zubItem.Empresa = EntryEmpresa.Text;
            zubItem.Direccion = EntryDireccion.Text;
            zubItem.Cuestionario = EntryCuestionario.Text;

            MessagingCenter.Send(this, "AddItem", zubItem);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}