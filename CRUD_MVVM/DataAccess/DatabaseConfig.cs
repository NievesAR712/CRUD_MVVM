using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_MVVM.DataAccess
{
    public static class DatabaseConfig
    {
        public static string ConnectionString => "Server=(localdb)\\NievesLocal;Database=CRUD_MVVM;Trusted_Connection=True;";
    }
}