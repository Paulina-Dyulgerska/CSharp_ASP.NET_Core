// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function sortList() {
    var table, i, switching, b, shouldSwitch;
    table = document.getElementById("table");
    switching = true;
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // start by saying: no switching is done:
        switching = false;
        b = table.getElementsByTagName("tr");
        // Loop through all list-items:
        for (i = 0; i < (b.length - 1); i++) {
            // start by saying there should be no switching:
            shouldSwitch = false;
            /* check if the next item should
            switch place with the current item: */
            if (b[i].firstElementChild.textContent.toLowerCase() > b[i + 1].firstElementChild.textContent.toLowerCase()) {
                /* if next item is alphabetically
                lower than current item, mark as a switch
                and break the loop: */
                shouldSwitch = true;
                break;
            }
        }
        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark the switch as done: */
            b[i].parentNode.insertBefore(b[i + 1], b[i]);
            switching = true;
        }
    }
}



function filterTableByTwoColumns(event) {
    console.log(event);
    var filter = event.target.value.toUpperCase();
    var rows = document.querySelector("#table tbody").rows;
    console.log(rows);
    for (var i = 0; i < rows.length; i++) {
        var firstCol = rows[i].cells[0].textContent.toUpperCase();
        var secondCol = rows[i].cells[1].textContent.toUpperCase();
        console.log(firstCol.indexOf(filter));
        if (firstCol.indexOf(filter) > -1 || secondCol.indexOf(filter) > -1) {
            rows[i].style.display = "";
            rows[i].classList.add("d-flex");
        } else {
            rows[i].style.display = "none";
            rows[i].classList.remove("d-flex");
        }
    }
}

document.querySelector('#searchInput').addEventListener('keyup', filterTableByTwoColumns, false);