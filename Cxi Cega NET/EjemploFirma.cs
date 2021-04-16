using CxiCega;
using System;

namespace Cxi_Cega_NET
{
    class EjemploFirma
    {
        public static string HSM = "3001@127.0.0.1";    //puerto y direccion IP del HSM
        public static string USUARIO = "user";          //usuario criptografico
        public static string CONTRASENA = "pass";       //contraseña del usuario criptografico
        public static string NOMBRE_LLAVE = "key_RSA";  //nombre de la llave a utilizar
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

            //Creacion del HASH para la firma con SHA 256
            string datos = "Cega Security";
            byte[] hash = cxi.createHash(Cxi.MECH_HASH_ALGO_SHA256, datos);

            //Generacion del mecanismo para firmar y la firma con llave RSA
            MechParam mech = new MechParam(Cxi.MECH_HASH_ALGO_SHA256 | Cxi.MECH_PAD_PKCS1);
            byte[] sign = cxi.sign(0, llave, mech, hash);
            Console.WriteLine("Datos firmados " + Convert.ToBase64String(sign));

            //Verificacion de la firma
            bool verify = cxi.verify(0, llave, mech, hash, sign);
            Console.WriteLine("La verificacion es " + verify);

            cxi.Close();

            Console.ReadLine();

        }
    }
}
