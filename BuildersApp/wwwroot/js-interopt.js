function message(e) { alert(e); }
function set(key, value) { localStorage.setItem(key, value); }
function get(key) { return localStorage.getItem(key); }
function remove(key) { return localStorage.removeItem(key); }
function logError(errorMessage) { console.log(errorMessage); }
async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}