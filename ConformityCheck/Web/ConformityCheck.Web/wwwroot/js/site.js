// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function isNullOrWhitespace(input) {
    return (typeof input === 'undefined' || input == null) || input.replace(/\s/g, '').length < 1;
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
