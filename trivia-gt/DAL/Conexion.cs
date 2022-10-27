using MySql.Data.MySqlClient;
using System.Data;

namespace trivia_gt.DAL
{
    public static class Conexion
    {
        private static MySqlConnection ConexionSQL { get; set; }


        public static MySqlConnection ObtenerConexion() => ConexionSQL = new MySqlConnection("Server='mysql-server-bb.mysql.database.azure.com';UserID ='bburgos';Password='AdminGT2@22*';Database='trivia-gt'");


    }
}
