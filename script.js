document.addEventListener("DOMContentLoaded", () => {
    const formEncrypt = document.querySelector("form[action='/encrypt']");
    const formDecrypt = document.getElementById("decryptForm");
    const textInput = document.getElementById("textInput");
    const rotSelect = document.getElementById("rotSelect");
    const fileInput = document.getElementById("fileInput");
    const resultContainer = document.getElementById("resultContainer");
    const rotResult = document.getElementById("rotResult");
    const messageResult = document.getElementById("messageResult");

    // Evento para encriptar texto
    formEncrypt.addEventListener("submit", (event) => {
        event.preventDefault();

        const text = textInput.value.trim();
        const rot = parseInt(rotSelect.value);

        if (!text) {
            showError("Por favor, ingresa un texto para encriptar.");
            return;
        }

        if (isNaN(rot)) {
            showError("Por favor, selecciona un ROT válido.");
            return;
        }

        const encryptedText = cesarEncrypt(text, rot);
        downloadAsXML(encryptedText);
        
        // Mostrar mensaje de éxito
        showSuccess(`Texto encriptado con ROT${rot} y descargado como archivo XML.`);
    });

    // Evento para desencriptar archivo XML
    formDecrypt.addEventListener("submit", (event) => {
        event.preventDefault();

        const file = fileInput.files[0];
        if (!file) {
            showError("Por favor, selecciona un archivo XML.");
            return;
        }

        if (!file.name.endsWith(".xml")) {
            showError("El archivo seleccionado no es un XML válido.");
            return;
        }

        const reader = new FileReader();
        reader.onload = () => {
            try {
                const content = reader.result;
                const parser = new DOMParser();
                const xmlDoc = parser.parseFromString(content, "application/xml");

                if (xmlDoc.getElementsByTagName("parsererror").length > 0) {
                    showError("El archivo XML no es válido o tiene errores de formato.");
                    return;
                }

                const encryptedMessage = xmlDoc.getElementsByTagName("encrypted")[0]?.textContent;
                if (!encryptedMessage) {
                    showError("El archivo XML no contiene un mensaje encriptado válido.");
                    return;
                }

                const result = identifyROT(encryptedMessage);
                if (result) {
                    // Mostrar el resultado en la interfaz
                    displayResult(result.rot, result.decryptedMessage);
                } else {
                    showError("No se pudo identificar un ROT válido para este mensaje.");
                }
            } catch (error) {
                showError("Ocurrió un error al procesar el archivo XML.");
                console.error(error);
            }
        };

        reader.readAsText(file);
    });

    // Función para mostrar errores con estilo
    function showError(message) {
        // Puedes mejorar esto implementando un sistema de notificaciones
        alert(message);
    }
    
    // Función para mostrar éxito con estilo
    function showSuccess(message) {
        // Puedes mejorar esto implementando un sistema de notificaciones
        alert(message);
    }
    
    // Función para mostrar el resultado del descifrado
    function displayResult(rot, message) {
        // Actualizar el contenido
        rotResult.textContent = rot;
        messageResult.textContent = message;
        
        // Mostrar el contenedor de resultados
        resultContainer.style.display = "block";
        
        // Desplazarse hasta el resultado
        resultContainer.scrollIntoView({ behavior: "smooth" });
    }
});

function cesarEncrypt(text, shift) {
    return text.replace(/[a-z]/gi, (char) => {
        const base = char === char.toUpperCase() ? 65 : 97;
        return String.fromCharCode(((char.charCodeAt(0) - base + shift) % 26) + base);
    });
}

function cesarDecrypt(text, shift) {
    return text.replace(/[a-z]/gi, (char) => {
        const base = char === char.toUpperCase() ? 65 : 97;
        return String.fromCharCode(((char.charCodeAt(0) - base - shift + 26) % 26) + base);
    });
}

function identifyROT(encryptedText) {
    // Intentar cada posible ROT (0-25)
    for (let rot = 0; rot < 26; rot++) {
        const decryptedMessage = cesarDecrypt(encryptedText, rot);
        if (isReadable(decryptedMessage)) {
            return { rot, decryptedMessage };
        }
    }
    return null;
}

function isReadable(text) {
    // Lista ampliada de palabras comunes en español para mejorar la detección
    const commonWords = ["el", "la", "los", "las", "de", "del", "y", "en", "a", "que", "es", "un", "una", "por", "con", "para", "hola", "yo", "tú", "él", "ella", "nos", "ellos", "esto", "eso", "sí", "no", "hay", "lo", "al", "mi", "me", "te", "se", "su", "bien", "mal", "más", "menos", "muy", "como", "pero", "ya", "sí", "gracias", "también", "aquí", "allí", "cuando", "donde", "quién", "qué", "cómo", "porque", "nada", "todo", "hoy", "mañana", "ayer", "amigo", "casa", "comer", "beber", "hacer", "ir", "venir", "estar", "tener", "ver", "decir", "dar"];
    
    // Convertir el texto a minúsculas y dividirlo en palabras
    const words = text.toLowerCase().match(/\b\w+\b/g) || [];
    
    // Verificar si al menos una palabra común está presente
    return commonWords.some(word => words.includes(word)) || 
           // Verificar si hay secuencias de letras que parecen palabras en español
           (text.match(/[aeiouáéíóú]/gi)?.length || 0) > text.length * 0.2; // Al menos 20% de vocales
}

function downloadAsXML(encryptedText) {
    const escapedText = encryptedText.replace(/[<>&'"]/g, (char) => {
        const escapeMap = { "<": "&lt;", ">": "&gt;", "&": "&amp;", "'": "&apos;", '"': "&quot;" };
        return escapeMap[char];
    });

    const xmlContent = `<?xml version="1.0" encoding="UTF-8"?>
<message>
    <encrypted>${escapedText}</encrypted>
</message>`;

    const blob = new Blob([xmlContent], { type: "application/xml" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = "mensaje_encriptado.xml";
    link.click();
    URL.revokeObjectURL(link.href);
}