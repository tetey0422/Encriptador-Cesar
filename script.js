document.addEventListener("DOMContentLoaded", () => {
    const formEncrypt = document.querySelector("form[action='/encrypt']");
    const formDecrypt = document.querySelector("form[action='/upload']");
    const textInput = document.getElementById("textInput");
    const rotSelect = document.getElementById("rotSelect");
    const fileInput = document.getElementById("fileInput");

    // Evento para encriptar texto
    formEncrypt.addEventListener("submit", (event) => {
        event.preventDefault(); // Evitar que el formulario se envíe

        const text = textInput.value.trim();
        const rot = parseInt(rotSelect.value);

        if (!text) {
            alert("Por favor, ingresa un texto para encriptar.");
            return;
        }

        if (isNaN(rot)) {
            alert("Por favor, selecciona un ROT válido.");
            return;
        }

        const encryptedText = cesarEncrypt(text, rot);
        downloadAsXML(encryptedText);
        alert("Texto encriptado y descargado como archivo XML.");
    });

    // Evento para desencriptar archivo XML
    formDecrypt.addEventListener("submit", (event) => {
        event.preventDefault(); // Evitar que el formulario se envíe

        const file = fileInput.files[0];
        if (!file) {
            alert("Por favor, selecciona un archivo XML.");
            return;
        }

        const reader = new FileReader();
        reader.onload = () => {
            const content = reader.result;
            const parser = new DOMParser();
            const xmlDoc = parser.parseFromString(content, "application/xml");

            // Verificar si el archivo tiene errores de análisis
            if (xmlDoc.getElementsByTagName("parsererror").length > 0) {
                alert("El archivo XML no es válido o tiene errores de formato.");
                return;
            }

            // Verificar si contiene la etiqueta <encrypted>
            const encryptedMessage = xmlDoc.getElementsByTagName("encrypted")[0]?.textContent;
            if (!encryptedMessage) {
                alert("El archivo XML no contiene un mensaje encriptado válido.");
                return;
            }

            // Identificar el ROT y descifrar el mensaje
            const result = identifyROT(encryptedMessage);
            if (result) {
                alert(`ROT identificado: ${result.rot}\nMensaje descifrado: ${result.decryptedMessage}`);
            } else {
                alert("No se pudo identificar un ROT válido.");
            }
        };

        reader.readAsText(file);
    });
});

function cesarEncrypt(text, shift) {
    return text.split("").map((char) => {
        if (char.match(/[a-z]/i)) {
            const base = char === char.toUpperCase() ? 65 : 97;
            return String.fromCharCode(((char.charCodeAt(0) - base + shift) % 26) + base);
        }
        return char; // Dejar caracteres no alfabéticos sin cambios
    }).join("");
}

function cesarDecrypt(text, shift) {
    return text.split("").map((char) => {
        if (char.match(/[a-z]/i)) {
            const base = char === char.toUpperCase() ? 65 : 97;
            return String.fromCharCode(((char.charCodeAt(0) - base - shift + 26) % 26) + base);
        }
        return char; // Dejar caracteres no alfabéticos sin cambios
    }).join("");
}

function identifyROT(encryptedText) {
    for (let rot = 0; rot < 26; rot++) {
        const decryptedMessage = cesarDecrypt(encryptedText, rot);
        if (isReadable(decryptedMessage)) {
            return { rot, decryptedMessage };
        }
    }
    return null;
}

function isReadable(text) {
    // Verificar si el texto descifrado contiene palabras legibles (puedes personalizar esta lógica)
    const commonWords = ["el", "la", "de", "y", "que", "en", "un", "es", "por", "con"];
    return commonWords.some((word) => text.toLowerCase().includes(word));
}

function downloadAsXML(encryptedText) {
    const xmlContent = `<?xml version="1.0" encoding="UTF-8"?>
<message>
    <encrypted>${encryptedText}</encrypted>
</message>`;

    const blob = new Blob([xmlContent], { type: "application/xml" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = "mensaje_encriptado.xml";
    link.click();
    URL.revokeObjectURL(link.href);
}