$(document).ready(function () {
    // Check for FileReader API (HTML5) support.
    if (!window.FileReader) {
        alert('This browser is not HTML5 compliant! Press OK to see more info.');
        document.location = _spPageContextInfo.webServerRelativeUrl + "/Pages/NotSupported.aspx";
    }
    
});