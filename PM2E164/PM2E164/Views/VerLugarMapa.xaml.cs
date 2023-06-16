using Plugin.Geolocator;
using PM2E164.Models;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PM2E164.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerLugarMapa : ContentPage
    {

        public LugarVisitado LugarVisitado { get; }
        public VerLugarMapa(LugarVisitado lugarVisitado)
        {
            InitializeComponent();
            LugarVisitado = lugarVisitado;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            var geo = CrossGeolocator.Current;

            if (geo == null || !CrossGeolocator.IsSupported || !geo.IsGeolocationEnabled)
            {
                await DisplayAlert("Aviso", "Su ubicación no está encendida", "Aceptar");
            }

            if (LugarVisitado == null) return;

            var mapPosition = new Position(LugarVisitado.Latitud, LugarVisitado.Longitud);

            mapa.Pins.Add(new Pin
            {
                Position = mapPosition,
                Label = LugarVisitado.Descripcion
            });

            mapa.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(1)));

        }

        private async void BtCompartir_Clicked(object sender, System.EventArgs e)
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, "image.jpeg");
            File.WriteAllBytes(cacheFile, LugarVisitado.Foto);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = LugarVisitado?.Descripcion,
                File = new ShareFile(cacheFile)
            });
        }

    }
}