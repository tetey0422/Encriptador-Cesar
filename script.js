document.addEventListener("DOMContentLoaded", () => {
    const encryptForm = document.getElementById("encryptForm");
    const decryptForm = document.getElementById("decryptForm");
    const senderName = document.getElementById("senderName");
    const secretMessage = document.getElementById("secretMessage");
    const secretCode = document.getElementById("secretCode");
    const messageDate = document.getElementById("messageDate");
    const rotSelect = document.getElementById("rotSelect");
    const fileInput = document.getElementById("fileInput");
    const resultContainer = document.getElementById("resultContainer");
    const senderResult = document.getElementById("senderResult");
    const messageResult = document.getElementById("messageResult");
    const codeResult = document.getElementById("codeResult");
    const dateResult = document.getElementById("dateResult");
    const rotResult = document.getElementById("rotResult");

    // Establecer la fecha actual como valor predeterminado
    const today = new Date().toISOString().split('T')[0];
    messageDate.value = today;

    // Evento para encriptar y generar XML
    encryptForm.addEventListener("submit", (event) => {
        event.preventDefault();

        // Validar los campos
        const name = senderName.value.trim();
        const message = secretMessage.value.trim();
        const code = secretCode.value.trim();
        const date = messageDate.value;
        const rot = parseInt(rotSelect.value);

        if (!name) {
            showNotification("Por favor, ingresa tu nombre.", "error");
            return;
        }

        if (!message) {
            showNotification("Por favor, ingresa un mensaje secreto.", "error");
            return;
        }

        if (!code) {
            showNotification("Por favor, ingresa un código secreto.", "error");
            return;
        }

        if (!date) {
            showNotification("Por favor, selecciona una fecha.", "error");
            return;
        }

        if (isNaN(rot)) {
            showNotification("Por favor, selecciona un ROT válido.", "error");
            return;
        }

        // Encriptar los datos
        const encryptedName = cesarEncrypt(name, rot);
        const encryptedMessage = cesarEncrypt(message, rot);
        const encryptedCode = cesarEncrypt(code, rot);
        const encryptedDate = cesarEncrypt(date, rot);

        // Generar y descargar el XML
        generateAndDownloadXML(name, encryptedName, encryptedMessage, encryptedCode, encryptedDate, rot);
        
        // Mostrar mensaje de éxito
        showNotification(`Mensaje secreto generado y guardado como MensajeSecreto_${name}.xml`, "success");
    });

    // Evento para desencriptar archivo XML
    decryptForm.addEventListener("submit", (event) => {
        event.preventDefault();

        const file = fileInput.files[0];
        if (!file) {
            showNotification("Por favor, selecciona un archivo XML.", "error");
            return;
        }

        if (!file.name.endsWith(".xml")) {
            showNotification("El archivo seleccionado no es un XML válido.", "error");
            return;
        }

        const reader = new FileReader();
        reader.onload = () => {
            try {
                const content = reader.result;
                const parser = new DOMParser();
                const xmlDoc = parser.parseFromString(content, "application/xml");

                if (xmlDoc.getElementsByTagName("parsererror").length > 0) {
                    showNotification("El archivo XML no es válido o tiene errores de formato.", "error");
                    return;
                }

                // Extraer información del XML
                const encryptedSender = xmlDoc.getElementsByTagName("remitente")[0]?.textContent;
                const encryptedMessage = xmlDoc.getElementsByTagName("mensaje")[0]?.textContent;
                const encryptedCode = xmlDoc.getElementsByTagName("codigo")[0]?.textContent;
                const encryptedDate = xmlDoc.getElementsByTagName("fecha")[0]?.textContent;
                const rot = parseInt(xmlDoc.getElementsByTagName("rot")[0]?.textContent);

                if (!encryptedSender || !encryptedMessage || !encryptedCode || !encryptedDate || isNaN(rot)) {
                    showNotification("El archivo XML no contiene todos los datos necesarios.", "error");
                    return;
                }

                // Descifrar con el ROT proporcionado
                const decryptedSender = cesarDecrypt(encryptedSender, rot);
                const decryptedMessage = cesarDecrypt(encryptedMessage, rot);
                const decryptedCode = cesarDecrypt(encryptedCode, rot);
                const decryptedDate = cesarDecrypt(encryptedDate, rot);

                // Formatear la fecha para mostrarla
                const formattedDate = formatDate(decryptedDate);

                // Mostrar el resultado
                displayResult(decryptedSender, decryptedMessage, decryptedCode, formattedDate, rot);
                
                showNotification("Mensaje descifrado correctamente.", "success");
            } catch (error) {
                showNotification("Ocurrió un error al procesar el archivo XML.", "error");
                console.error(error);
            }
        };

        reader.readAsText(file);
    });

    // Función para mostrar notificaciones
    function showNotification(message, type) {
        // Eliminar notificaciones anteriores
        const oldNotifications = document.querySelectorAll('.notification');
        oldNotifications.forEach(n => n.remove());
        
        // Crear nueva notificación
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.textContent = message;
        
        // Añadir al DOM
        document.body.appendChild(notification);
        
        // Aplicar animación
        setTimeout(() => {
            notification.style.opacity = '1';
        }, 10);
        
        // Eliminar después de 3 segundos
        setTimeout(() => {
            notification.remove();
        }, 3000);
    }
    
    // Función para mostrar el resultado del descifrado
    function displayResult(sender, message, code, date, rot) {
        // Actualizar el contenido
        senderResult.textContent = sender;
        messageResult.textContent = message;
        codeResult.textContent = code;
        dateResult.textContent = date;
        rotResult.textContent = rot;
        
        // Mostrar el contenedor de resultados
        resultContainer.style.display = "block";
        
        // Desplazarse hasta el resultado
        resultContainer.scrollIntoView({ behavior: "smooth" });
    }
    
    // Función para formatear la fecha
    function formatDate(dateStr) {
        try {
            const date = new Date(dateStr);
            return date.toLocaleDateString('es-ES', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric'
            });
        } catch (e) {
            return dateStr; // Si hay un error, devolver la cadena original
        }
    }
});

// Función para encriptar con cifrado César
function cesarEncrypt(text, shift) {
    return text.replace(/[a-z]/gi, (char) => {
        const base = char === char.toUpperCase() ? 65 : 97;
        return String.fromCharCode(((char.charCodeAt(0) - base + shift) % 26) + base);
    });
}

// Función para desencriptar con cifrado César
function cesarDecrypt(text, shift) {
    return text.replace(/[a-z]/gi, (char) => {
        const base = char === char.toUpperCase() ? 65 : 97;
        return String.fromCharCode(((char.charCodeAt(0) - base - shift + 26) % 26) + base);
    });
}

// Función para generar y descargar el archivo XML
function generateAndDownloadXML(originalName, encryptedName, encryptedMessage, encryptedCode, encryptedDate, rot) {
    // Escapar caracteres especiales XML
    function escapeXML(str) {
        return str.replace(/[<>&'"]/g, (char) => {
            const escapeMap = { "<": "&lt;", ">": "&gt;", "&": "&amp;", "'": "&apos;", '"': "&quot;" };
            return escapeMap[char];
        });
    }

    // Crear el contenido XML
    const xmlContent = `<?xml version="1.0" encoding="UTF-8"?>
<mensajeSecreto>
    <remitente>${escapeXML(encryptedName)}</remitente>
    <mensaje>${escapeXML(encryptedMessage)}</mensaje>
    <codigo>${escapeXML(encryptedCode)}</codigo>
    <fecha>${escapeXML(encryptedDate)}</fecha>
    <rot>${rot}</rot>
</mensajeSecreto>`;

    // Crear un blob y descargar
    const blob = new Blob([xmlContent], { type: "application/xml" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = `MensajeSecreto_${originalName.replace(/\s+/g, '_')}.xml`;
    
    // Simular la creación de una carpeta "Mensajes"
    console.log("Guardando en la carpeta 'Mensajes/'...");
    // Nota: En entorno web no se puede crear una carpeta real sin acceso al servidor
    
    link.click();
    URL.revokeObjectURL(link.href);
}