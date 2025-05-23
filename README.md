# Encriptador César

Este proyecto es una aplicación web que permite encriptar y desencriptar mensajes utilizando el cifrado César. También permite cargar archivos XML con mensajes encriptados para su desencriptación.

## Características

- **Encriptación de texto**: Encripta un texto ingresado por el usuario utilizando un desplazamiento ROT (0-26).
- **Descarga de archivo XML**: Descarga el texto encriptado como un archivo XML.
- **Desencriptación de archivos XML**: Carga un archivo XML con un mensaje encriptado y lo desencripta automáticamente identificando el ROT utilizado.
- **Interfaz amigable**: Diseño responsivo y fácil de usar.

## Tecnologías utilizadas

- **HTML5**: Estructura de la aplicación.
- **CSS3**: Estilos y diseño responsivo.
- **JavaScript**: Lógica de encriptación, desencriptación y manejo de archivos.

## Cómo usar

### Encriptar texto
1. Ingresa el texto que deseas encriptar en el campo "Texto a encriptar".
2. Selecciona el tipo de encriptado (ROT) en el menú desplegable.
3. Haz clic en el botón "Encriptar".
4. El texto encriptado se descargará automáticamente como un archivo XML.

### Desencriptar archivo
1. Haz clic en "Selecciona un archivo XML" y carga un archivo con un mensaje encriptado.
2. Haz clic en el botón "Desencriptar".
3. La aplicación identificará el ROT utilizado y mostrará el mensaje desencriptado.

## Estructura del proyecto
```
. 
├── index.html # Archivo principal de la interfaz 
├── styles.css # Estilos de la aplicación 
├── script.js # Lógica de encriptación y desencriptación 
└── README.md # Documentación del proyecto
```

## Ejemplo de archivo XML generado

```xml
<?xml version="1.0" encoding="UTF-8"?>
<message>
    <encrypted>Uifsf jt b tfdsfu nfttbhf</encrypted>
</message>
```

## Instalación
1. Clona este repositorio:

    ```git clone https://github.com/tu-usuario/encriptador-cesar.git```

2. Abre el archivo index.html en tu navegador.

## Créditos

- **Autor**: Jefrey Zanches A.
- **Año**: 2025

## Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo LICENSE para más detalles.

```Puedes personalizar este archivo según tus necesidades.```