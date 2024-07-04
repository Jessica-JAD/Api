using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.Utiles
{
    public class General
    {
        private static string _RESTDBName;
        private static string _RESTServer;
        private static int _RESTIdDatos;
        private static string _RESTEmpresa;

        public static string RESTApiName => "Integracion";
        
        public static int RESTIdApiModulo => 30;
        
        public static string RESTApiUser => "APIUser";

        public static string RESTDBName { get { return _RESTDBName; } }

        public static string RESTServer { get { return _RESTServer; } }

        public static int RESTIdDatos { get { return _RESTIdDatos; } }

        public static string RESTEmpresa { get { return _RESTEmpresa; } }

        public General()
        {
            _RESTIdDatos = 0;
            _RESTServer = "localhost";
            _RESTDBName = "ZunSt";
            _RESTEmpresa = "vacia";
        }

        public static bool CheckPassword(string password)
        {
            string _password = "Secy1cnc,p4se.22"; //Frase: Si el código y los comentarios no coinciden, posiblemente ambos sean erróneos. 2022
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJEQk5hbWUiOiJadW5TdE1lbGlhSCIsIlNlcnZlciI6IlNPRlRVUi1TRVJHSU8iLCJJZERhdG9zIjoiNjQiLCJFbXByZXNhIjoiQ1VCQU5BQ0FOIEVYUFJFU1MgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAiLCJuYmYiOjE2NjMyNjgwMTMsImV4cCI6MTY5NDgwNDAxMywiaXNzIjoiZ2V0LmN1IiwiYXVkIjoiZ2V0LmN1In0.8IwzfzzN8VVtU6fhJQbTbSGk5CkUBbv_1pjGBXJmMzU
            bool tmpRslt = password.Equals(EncriptarCadena(_password));

            return tmpRslt;
        }

        public static string GetToken()
        {
            if (string.IsNullOrWhiteSpace(RESTDBName))
            {
                throw new Exception("Nombre de la base de datos no definido.");
            }
            if (string.IsNullOrWhiteSpace(RESTServer))
            {
                throw new Exception("Servidor no definido.");
            }
            if (RESTIdDatos == 0)
            {
                throw new Exception("Juego de datos no definido.");
            }
            if (string.IsNullOrWhiteSpace(RESTEmpresa))
            {
                throw new Exception("Empresa no definida.");
            }

            var _claims = new[]
            {
                new Claim("DBName", RESTDBName),
                new Claim("Server", RESTServer),
                new Claim("IdDatos", RESTIdDatos.ToString()),
                new Claim("Empresa", RESTEmpresa),
            };

            IConfiguration _configuracion;
            _configuracion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
            string _strkey = _configuracion.GetValue<string>("secret_jwt");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_strkey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "get.cu",
                audience: "get.cu",
                claims: _claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddYears(1), // Un año a partir de la fecha actual
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        /// <summary>
        /// Metodo q devuelve los datos necesarios para la eventual conexion, buscando en los juegos de datos establecidos en el cliente
        /// </summary>
        public static void GetConnectionData(string username, string password, string host, string dbname)
        {
            const string paramZunUser = "ZunUser";
            const string paramZunModulo = "ZunModulo";

            try
            {
                var connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", host, dbname, username, password);

                SqlConnection sqlcon = new SqlConnection(connectionString);
                sqlcon.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlcon,
                    CommandText = @$"SELECT TOP 1
                                        D.id_datos      id_datos,
                                        D.servidor      servidor,
                                        D.origen_datos  basedatos,
                                        E.nombre        empresa
                                    FROM
                                        datos D
                                            INNER JOIN acceso A ON A.id_datos = D.id_datos
                                            INNER JOIN operador O ON O.id_oper = A.id_oper  AND O.activo = 1
                                            INNER JOIN modulo M ON M.id_modulo = D.id_modulo
                                            INNER JOIN juegos_datos JD ON JD.id_juego = D.id_juego AND JD.activo = 1
                                            INNER JOIN entidad E ON E.id_entidad = E.id_entidad
                                    WHERE
                                        O.nombre = @{paramZunUser} AND 
                                        M.id_modulo = @{paramZunModulo}
                                    ORDER BY
                                        id_datos DESC",
                };
                cmd.Parameters.AddWithValue(paramZunUser, General.RESTApiUser);
                cmd.Parameters.AddWithValue(paramZunModulo, General.RESTIdApiModulo);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _RESTIdDatos = reader.GetInt32(reader.GetOrdinal("id_datos"));
                            _RESTServer = reader.GetString(reader.GetOrdinal("servidor"));
                            _RESTDBName = reader.GetString(reader.GetOrdinal("basedatos"));
                            _RESTEmpresa = reader.GetString(reader.GetOrdinal("empresa"));
                        }
                    }
                }

                sqlcon.Close();
            }
            catch (Exception)
            {
                throw new Exception("Error al establecer parámetros de conexión.");
            }
        }

        public static string EncriptarCadena(string pCadena)
        {
            string cEncrypt = "";

            for (int i = 0; i < pCadena.Length; i++)
            {
                cEncrypt += (char)(((int)pCadena[i]) ^ 127); // El operador ^ es el XOR
            }

            return cEncrypt;
        }

    }
}
