// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var urlHash;

function WindowOpen(url)
{
    window.open(url, "_new", "height:auto, width:auto");
}

window.onload = function Load()
{
    CheckFragment();
}

function CheckFragment ()
{
    urlHash = window.location.hash;
    if (urlHash != "")
    {
        urlHash = urlHash.replace("#", "?");
        if(window.opener != null)
        {
            window.opener.location.href = "/Home/OAuthResult/" + urlHash;
            window.close();
        }
        else
        {
            window.location.href = "/Home/OAuthResult/" + urlHash;
        }
    }
}