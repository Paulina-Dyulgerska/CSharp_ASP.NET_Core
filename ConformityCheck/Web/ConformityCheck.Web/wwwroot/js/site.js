// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function isNullOrWhitespace(input) {
    return (typeof input === 'undefined' || input == null) || input.replace(/\s/g, '').length < 1;
}

const entityMap = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '&quot;',
    "'": '&#39;',
    '/': '&#x2F;',
    '`': '&#x60;',
    '=': '&#x3D;'
};

function escapeHtml(string) {
    return String(string).replace(/[&<>"'`=\/]/g, function (s) {
        return entityMap[s];
    });
}

//$(".submenu-edit .nav-list .list-item").click(function showSubMenu({ target }) {
//    console.log(target.parentElement);
//    const link = target.parentElement.querySelector(".quick-nav-submenu");
//    console.log(link);
//    console.log(link.style.display, " 1");
//    if (link.style.display == "none") {
//        link.style.display = "";
//        $(link).show();
//    } else {
//        link.style.display = "none";
//        $(link).hide();
//    }
//});

// show and hide the sub menu quick navigation items:
$(document).ready(function () {
    $(".submenu .nav-list .submenu-list-item").hover(function () {
        $(this).children(".quick-nav-submenu").slideDown('fast');
    }, function () {
        $(this).children(".quick-nav-submenu").slideUp('slow');
    });
});
