function openYoutube(element) {

    img = document.getElementById(element);

    img.innerHTML = htmlDecode(img.getAttribute("data-embeded"));
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}