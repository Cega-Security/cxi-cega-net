using CxiCega;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cxi_Cega_NET
{
    class EjemploEncriptacion
    {
        public static string HSM = "3001@127.0.0.1";    //puerto y direccion IP del HSM
        public static string USUARIO = "user";          //usuario criptografico
        public static string CONTRASENA = "pass";       //contraseña del usuario criptografico
        public static string NOMBRE_LLAVE = "key_AES";  //nombre de la llave a utilizar
        public static string NOMBRE_GRUPO = "group";    //grupo de la llave

        static void Main(string[] args)
        {
            //Conectarse al HSM
            Cxi cxi = new Cxi(HSM);
            Console.WriteLine("Conectado a HSM " + HSM);

            //Iniciar sesion
            cxi.Logon(USUARIO, CONTRASENA);
            Console.WriteLine("Usuario inicio sesion " + USUARIO);

            //Crear objeto Key Properties para buscar la llave, si no tiene specifier por defecto es -1
            KeyProperties kp = new KeyProperties(NOMBRE_LLAVE, NOMBRE_GRUPO);
            byte[] llave = cxi.findKey(0, kp);
            Console.WriteLine("Llave encontrada " + NOMBRE_LLAVE);

            //Generacion del mecanismo para encriptar y la encriptacion con llave AES
            MechParam mech = new MechParam(Cxi.MECH_MODE_ENCRYPT | Cxi.MECH_CHAIN_CBC | Cxi.MECH_PAD_PKCS5);
            byte[] datos = Encoding.UTF8.GetBytes("Cega Security");
            byte[] crypt = cxi.crypt(llave, mech, datos);
            Console.WriteLine("Datos cifrados " + Convert.ToBase64String(crypt));

            //Generacion del mecanismo para desencriptar y la desencriptacion con llave AES
            MechParam mechDecrypt = new MechParam(Cxi.MECH_MODE_DECRYPT | Cxi.MECH_CHAIN_CBC | Cxi.MECH_PAD_PKCS5);
            byte[] decrypt = cxi.crypt(llave, mechDecrypt, crypt);
            Console.WriteLine("Datos descifrados " + Encoding.UTF8.GetString(decrypt));

            cxi.Close();

            Console.ReadLine();

        }
    }
}
