function downloadPasswordFile(pass,secreteKey) {
    var obj = { password: pass, key: secreteKey};
    var filename = "secrete.txt";
    var blob = new Blob([JSON.stringify(obj)], { type: 'text/plain' });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob, filename);
    } else {
        var e = document.createEvent('MouseEvents'),
            a = document.createElement('a');
        a.download = filename;
        a.href = window.URL.createObjectURL(blob);
        a.dataset.downloadurl = ['text/plain', a.download, a.href].join(':');
        e.initEvent('click', true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        a.dispatchEvent(e);
    }
}