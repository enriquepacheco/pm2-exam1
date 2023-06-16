using PM2E164.Utiles;
using System;
using System.IO;
using Xamarin.Forms;

namespace PM2E164
{
    public partial class App : Application
    {

        static LugaresRepositorio lugaresRepositorio;
        public static LugaresRepositorio LugaresRepositorio
        {
            get
            {
                if (lugaresRepositorio == null)
                {
                    string dbname = "Proc.db3";
                    string dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string dbfulp = Path.Combine(dbpath, dbname);
                    lugaresRepositorio = new LugaresRepositorio(dbfulp);
                }

                return lugaresRepositorio;
            }
        }


        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
