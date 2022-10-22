using MySql.Data.MySqlClient;
using System.Data;
using System.Xml;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class AvatarDAL : IDALBase<AvatarBE>
    {
        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        public AvatarDAL() => _conexionSQL = Conexion.ObtenerConexion();

        public void CrearComando(string cmdText, CommandType tipo, MySqlConnection conexion)
        {
            _comandoSQL = new MySqlCommand(cmdText, conexion)
            {
                CommandType = tipo
            };
        }

        public void CrearParametro(string nombre, object valor)
        {
            throw new NotImplementedException();
        }

        public void AgregarParametro(MySqlParameter parametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ListarDS(AvatarBE entidad)
        {
            try
            {
                string sql = "SELECT a.idAvatar, a.url, a.tag FROM avatar a where a.tag like 'AVATAR%'";

                using (MySqlConnection connection = _conexionSQL)
                {
                    CrearComando(sql, CommandType.Text, _conexionSQL);


                    using (MySqlDataAdapter da = new MySqlDataAdapter(_comandoSQL))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            DataSet _ds = new DataSet();
                            connection.Open();
                            da.Fill(dt);

                            return dt;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (_conexionSQL.State == ConnectionState.Open)
                    _conexionSQL.Close();
                throw new Exception(ex.Message);
            }
        }

        public List<AvatarBE> Listar(AvatarBE entidad)
        {
            try
            {
                List<AvatarBE> _lista = new List<AvatarBE>();
                AvatarBE _be;

                DataTable _dt = new DataTable();

                _dt = ListarDS(entidad);

                foreach (DataRow item in _dt.Rows)
                {
                    _be = new AvatarBE
                    {
                        IdAvatar = (int)item["IdAvatar"],
                        URL = item["url"].ToString(),
                        Tag = item["tag"].ToString()

                    };

                    _lista.Add(_be);
                }

                return _lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Actualizar(AvatarBE entidad)
        {
            throw new NotImplementedException();
        }

        public int Crear(AvatarBE entidad)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(AvatarBE entidad)
        {
            throw new NotImplementedException();
        }




    }
}
