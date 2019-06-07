# envioMail
# envioMail
Utilidad de consola Windows para automatizar envíos de correos electrónicos. La aplicación tiene dos funciones o métodos definidos, paso de todos los parámetros desde la línea de comando, o paso de estructura XML con la configuración necesaria.

## Archivo XML.
Esta estructura se basa en un archivo con nombre **datos_envio_mail.xml** y ubicado en el mismo lugar donde se encuentra el ejecutable de enviomail. Para ello, no es necesario más que llamar a la aplicación sin necesidad de usar ningún parámetro
### Estructura de archivo.
Con el sisguiente ejemplo: 
```
<?xml version="1.0" encoding="UTF-8"?>
<dataroot>
	<servidor>
		<smtp>smtp.email.com</smtp>
		<puerto>587</puerto>
		<ssl>si</ssl>
		<usuario>mi@email.com</usuario>
		<password>contraseña</password>
		<correo_emisor>mi@email.com</correo_emisor>
		<nombre_emisor>El administrador</nombre_emisor>
		<asunto>Pruebas de informe</asunto>
		<mensaje>Adjunto tienes el informe de la copia</mensaje>
	</servidor>
	<destinatarios>
		<email>destinatario@email.com</email>
	</destinatarios>
	<adjuntos>
	<ruta>c:\copias-windows\informe.txt</ruta>
	</adjuntos>
</dataroot>
```

Para el nodo servidor, encontramos los siguientes tag:
* stmp: Servidor stmp que usaremos para el envío del correo electrónico.
* puerto: Puerto usado por el servidor.
* SSL: Valores si/no, indica el protocolo a usar.
* usuario: Usuario de correo electrónico.
* password: Contraseña usada para el envio.
* correo_emisor: Dirección de correo electrónico que parace en el from del email.
* nombre_emisor: Nombre que aparece en el from del email.
* asunto: Asunto del correo electrónico.
* mensaje: Mensaje del correo electrónico.

Para el nodo destinatario:
* email: Dirección del destinatario, puede contener más de un tag dentro de este nodo.

Para el nodo adjuntos:
* ruta: Dirección donde encontrar el archivo que se va a adjuntar, puede contener más de un tag dentro de este nodo.


### Paso de parámetros.
Se trata del uso más simple pero a la vez, más flexible de uso de la aplicación, ya que los datos necesarios para el envío del correo electrónico se realizan desde la misma línea de comando, sin necesidad de utilizar ningún archivo xml. Los parámetro usados son los siguientes:
* de: Dirección de correo electrónico quien enviar el correo.
* para: Dirección de correo electrónico quien recibe el correo (sólo se puede indicar uno).
* asunto: Asunto del correo electrónico.
* texto: Cuerpo del mensaje. Permite codificación HTML
* smtp: Servidor de smtp.
* puerto: Puerto usado por el servidor.
* SSL: si/no va a usar el protocolo de cifrado.
* usuario: usuario de envío del correo electrónico
* password: Contraseña usada 
* ruta_log: (opcional) Ruta de archivo txt que actúa como log.
* ruta_adjunto: (opcional) Ruta de ubicación del documento adjunto, admite uno o más documentos adjuntos.

Como ejemplo la invocación puede ser
```
c:\enviomail.exe mi@email.com destino@gemail.com “Envio de factura” “Adjunto a este correo encontrarás la factura de su pedido” stmp.mail.com 587 si usuario contraseña c:\log.txt c:\factura.pdf