// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.onload = function Load()
{
    CheckFragment();
}

function IntegerOnly(obj)
{
    obj.value = obj.value.replace(/[^0-9]/g,'');
}

function CheckFragment ()
{
    var urlHash = window.location.hash;
    if (urlHash != "")
    {
        urlHash = urlHash.replace("#", "?");
        window.location.href = "/Home/OAuthResult/" + urlHash;
    }
}