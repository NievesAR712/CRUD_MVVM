

namespace CRUD_MVVM.Utilidades
{
    public static class ConexionDB
    {
        public static string DevolverRuta(string nombreBD)
        {
            string rutaBD = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Para Android: LocalApplicationData
                rutaBD = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rutaBD = Path.Combine(rutaBD, nombreBD);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Para iOS: MyDocuments -> .. -> Library
                rutaBD = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rutaBD = Path.Combine(rutaBD, "..", "Library", nombreBD);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                // Para Windows: AppData (puedes ajustar la ruta según tu necesidad)
                rutaBD = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rutaBD = Path.Combine(rutaBD, nombreBD);
            }

            return rutaBD;
        }
    }
}
