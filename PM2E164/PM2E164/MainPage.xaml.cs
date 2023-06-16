using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using PM2E164.Views;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2E164
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ObtenerUbicacion();
        }

        async Task ObtenerUbicacion()
        {
            var geo = CrossGeolocator.Current;

            if (geo == null || !CrossGeolocator.IsSupported || !geo.IsGeolocationEnabled)
            {
                await DisplayAlert("Aviso", "Para continuar necesita activar la ubicación", "Aceptar");
                BtAgregar.IsEnabled = false;
                return;
            }
            BtAgregar.IsEnabled = true;

            geo.DesiredAccuracy = 50;

            var posicion = await geo.GetLastKnownLocationAsync();
            if (posicion != null)
            {
                ActualizarCoordenadas(posicion);
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                geo.PositionChanged += Geo_PositionChanged;

                if (!geo.IsListening)
                {
                    await geo.StartListeningAsync(TimeSpan.FromSeconds(10), 100);
                }

                var position = await CrossGeolocator.Current.GetPositionAsync();
                ActualizarCoordenadas(posicion);
            }

        }

        private void Geo_PositionChanged(object sender, PositionEventArgs e)
        {
            ActualizarCoordenadas(e.Position);
        }

        void ActualizarCoordenadas(Position ubicacion)
        {
            TxtLongitud.Text = ubicacion?.Longitude.ToString();
            TxtLatitud.Text = ubicacion?.Latitude.ToString();
        }

        private async void BtTomarFoto_Clicked(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Pictures",
                Name = "temp.jpg",
                SaveToAlbum = true
            });

            if (photo != null)
            {
                ImgLugar.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }

        private async void BtAgregar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtLatitud.Text?.Trim()))
            {
                await DisplayAlert("Aviso", "No se ha encontrado el valor de Latitud", "Aceptar");
                return;
            }


            if (string.IsNullOrEmpty(TxtLongitud.Text?.Trim()))
            {
                await DisplayAlert("Aviso", "No se ha encontrado el valor de Longitud", "Aceptar");
                return;
            }

            if (ImgLugar.Source == null)
            {
                await DisplayAlert("Aviso", "No has tomado una imagen", "Aceptar");
                return;
            }


            if (string.IsNullOrEmpty(TxtDescripcion.Text?.Trim()))
            {
                await DisplayAlert("Aviso", "No has ingresado una descripción", "Aceptar");
                return;
            }

            if (await App.LugaresRepositorio.Add(new Models.LugarVisitado
            {
                Longitud = Convert.ToDouble(TxtLongitud.Text),
                Latitud = Convert.ToDouble(TxtLatitud.Text),
                Descripcion = TxtDescripcion.Text,
                Foto = await GetImageBytes(),
            }) > 0)
            {
                TxtLongitud.Text = null;
                TxtLatitud.Text = null;
                TxtDescripcion.Text = null;
                ImgLugar.Source = null;
                await DisplayAlert("Éxito", "Registrado correctamente", "Aceptar");
            }
            else
                await DisplayAlert("Aviso", "Ocurrió un error al registrar", "Aceptar");

        }

        public async Task<byte[]> GetImageBytes()
        {
            if (ImgLugar.Source != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = await ((StreamImageSource)ImgLugar.Source).Stream(CancellationToken.None);
                    stream.CopyTo(memory);
                    byte[] fotobyte = memory.ToArray();

                    return fotobyte;
                }
            }

            return null;
        }

        private async void BtListado_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaUbicaciones());
        }

        private void BtSalir_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
