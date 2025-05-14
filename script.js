document.addEventListener("DOMContentLoaded", () => {
    const formEncrypt = document.querySelector("form[action='/encrypt']");
    const formDecrypt = document.querySelector("form[action='/upload']");
    const textInput = document.getElementById("textInput");
    const rotSelect = document.getElementById("rotSelect");
    const fileInput = document.getElementById("fileInput");

    // Evento para encriptar texto
    formEncrypt.addEventListener("submit", (event) => {
        event.preventDefault();

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
        event.preventDefault();

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

            if (xmlDoc.getElementsByTagName("parsererror").length > 0) {
                alert("El archivo XML no es válido o tiene errores de formato.");
                return;
            }

            const encryptedMessage = xmlDoc.getElementsByTagName("encrypted")[0]?.textContent;
            if (!encryptedMessage) {
                alert("El archivo XML no contiene un mensaje encriptado válido.");
                return;
            }

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
        return char;
    }).join("");
}

function cesarDecrypt(text, shift) {
    return text.split("").map((char) => {
        if (char.match(/[a-z]/i)) {
            const base = char === char.toUpperCase() ? 65 : 97;
            return String.fromCharCode(((char.charCodeAt(0) - base - shift + 26) % 26) + base);
        }
        return char;
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
    const commonWords = ["el", "la", "los", "las", "de", "del", "y", "en", "a", "que", "es", "un", "una", "por", "con", "para", "hola", "yo", "tú", "él", "ella", "nos", "ellos", "esto", "eso", "sí", "no", "hay", "lo", "al", "mi", "me", "te", "se", "su", "bien", "mal", "más", "menos", "muy", "como", "pero", "ya", "sí", "gracias", "también", "aquí", "allí", "cuando", "donde", "quién", "qué", "cómo", "porque", "nada", "todo", "hoy", "mañana", "ayer", "amigo", "casa", "comer", "beber", "hacer", "ir", "venir", "estar", "tener", "ver", "decir", "dar"];
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