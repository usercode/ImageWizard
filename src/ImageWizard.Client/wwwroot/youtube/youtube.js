function openYoutube(element, videoId) {

    console.log(element);
    console.log(videoId);

    img = document.getElementById(element);

    console.log(img);

    img.innerHTML = htmlDecode(img.getAttribute("data-embeded"));


    //iframe = document.createElement("iframe");
    //iframe.setAttribute("src", "https://www.youtube-nocookie.com/embed/" + videoId);
    //iframe.setAttribute("frameborder", "0");
    //iframe.setAttribute("allow", "autoplay; encrypted-media");
    //iframe.setAttribute("allowfullscreen", "allowfullscreen");

    //ímg.parentNode.appendChild(iframe);
}

function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}