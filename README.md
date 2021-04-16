# Cxi Cega .NET
Este proyecto es un ejemplo para implementar la librería de Cxi Cega proporcionada por CEGA Security, para ser implementada dentro de un proyecto de .NET Framework para conexión a HSMs Utimaco. 

## Requisitos
- .NET Framework 4.5 en adelante
- Proyecto de 64 bits
- Haber instalado las herramientas de SecurityServer 4.20
- Referencia a la librería cxi.dll

Para otras configuraciones del proyecto contactar a personal de Cega Security

## Librerías
1. **cxi.dll** - Librería en C++ proporcionada por Utimaco para la conexión al HSM. No es necesaria la referencia dentro del proyecto, sólo que se tenga acceso. La referencia se instala con las herramientas de SecurityServer proporcionadas por Utimaco y/o Cega Security.
2. **cxilibrary2.dll** - Librería wrapper entre la librería en C++ de Utimaco y .NET de Cega Security. Proporcionada por Cega Security
3. **CxiCega.dll** - Librería en C# para la implementación de la comunicación a HSM Utimaco en proyectos .NET Framework. Proporcionada por Cega Security.

## Uso Básico
- Para proyectos nuevos: Se hace referencia a las librerías **cxilibrary2.dll** y **CxiCega.dll** en la configuración del proyecto
- Migrando de la librería de Utimaco: Se reemplazan las librerías de Utimaco por las librerías de Cega Security; en caso de presentar algún error corregir las referencias de las funciones

### Conexión
Se crea una instancia del objeto `Cxi` con la dirección IP del HSM y se inicia sesión con un usuario criptográfico
    
    Cxi cxi = new Cxi("192.168.1.100");
    cxi.Logon("USUARIO", "CONTRASENA");

### Buscar Llave
Se crea un objeto `KeyProperties` con el nombre y el grupo de la llave a utilizar

    KeyProperties kp = new KeyProperties("NOMBRE_LLAVE", "NOMBRE_GRUPO");
    byte[] llave = cxi.findKey(0, kp);

### Encriptación y Desencriptación
Para encriptar se crea el mecanismo con los parámetros de modo encrypt y el padding deseado. Se obtienen los bytes a encriptar y se ejecuta la función `cxi.crypt` con la llave correspondiente

    MechParam mech = new MechParam(Cxi.MECH_MODE_ENCRYPT | Cxi.MECH_CHAIN_CBC | Cxi.MECH_PAD_PKCS5);
    byte[] datos = Encoding.UTF8.GetBytes("Cega Security");
    byte[] crypt = cxi.crypt(llave, mech, datos);

Para desencriptar se crea el mecanismo con los parámetros de modo decrypt y el padding deseado. Se ejecuta la función `cxi.crypt` con la llave correspondiente

    MechParam mechDecrypt = new MechParam(Cxi.MECH_MODE_DECRYPT | Cxi.MECH_CHAIN_CBC | Cxi.MECH_PAD_PKCS5);
    byte[] decrypt = cxi.crypt(llave, mechDecrypt, crypt);

### Firma y verificación
Se crea el hash de los datos a firmar con la función `cxi.createHash` y el tipo de hash a utilizar

    string datos = "Cega Security";
    byte[] hash = cxi.createHash(Cxi.MECH_HASH_ALGO_SHA256, datos);

Se crea el mecanismo con el hash y el padding correspondiente y se ejecuta la firma con la función `cxi.sign` con la llave correspondiente

    MechParam mech = new MechParam(Cxi.MECH_HASH_ALGO_SHA256 | Cxi.MECH_PAD_PKCS1);
    byte[] sign = cxi.sign(0, llave, mech, hash);

Posteriormente se puede hacer la verificación de la firma con la función `cxi.verify`

    bool verify = cxi.verify(0, llave, mech, hash, sign);

### Cierre
Es necesario cerrar la conexión al HSM para evitar problemas con el número de conexiones

    cxi.Close();

## Documentación 
La documentación detallada se encuentra en la carpeta Cxi Cega Documentation. Generada con Doxygen, abriendo el index.html se podrá navegar entre las diferentes clases y métodos.

## Errores comunes
Si se presenta algún problema se puede poner en contacto con personal de Cega Security. Por lo pronto estos son algunos errores comunes y su solución:
1. No se encuentra la referencia a CxiCega.dll
    - Validar que la versión del .NET Framework sea mayor o igual a 4.5 y que la arquitectura del proyecto sea de 64 bits.
2. No se encuentra la referencia a cxilibrary2.dll
    - Validar que la instalación del SecurityServer 4.20 corresponde a la instalación predeterminada con las herramientas de CXI.
    - Validar la referencia de cxi.dll en el PATH del equipo, o copiarla en la misma carpeta de ejecución del proyecto.
    - Copiar la librería de cxi.dll en system32 para validar que el equipo la encuentre de manera correcta
    - Verificar/Instalar los redistribuibles de C++ 2015 (Instalar el SecurityServer 4.20 los instala por default)

