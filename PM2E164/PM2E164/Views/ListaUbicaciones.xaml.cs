using PM2E164.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E164.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaUbicaciones : ContentPage
    {

        public ListaUbicaciones()
        {
            BindingContext = this;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ListaLugares.ItemsSource = await App.LugaresRepositorio.GetAll();
        }

        private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null)
                return;

            var respuesta = await DisplayActionSheet("Acción", "Cancelar", null, new string[] { "Ir a la ubicación", "Eliminar" });
            var lugar = (e.CurrentSelection.First() as LugarVisitado);

            switch (respuesta)
            {
                case "Ir a la ubicación":

                    break;
                case "Eliminar":
                    await App.LugaresRepositorio.Delete(lugar);
                    ListaLugares.ItemsSource = await App.LugaresRepositorio.GetAll();
                    await DisplayAlert("Aviso", "Hecho!", "Aceptar");
                    break;
                default:
                    break;
            }

            if (await DisplayAlert("Acción", "Desea ir a la ubicación indicada", "Yes", "No"))
            {
                await DisplayAlert("", .Descripcion, "Ok");
            }

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
