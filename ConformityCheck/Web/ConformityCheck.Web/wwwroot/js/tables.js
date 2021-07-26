// sorting column:
function sortList() {
    var table, i, switching, b, shouldSwitch;
    table = document.getElementById('dataTable');
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

// filtering tables:
// document.querySelector('#searchInput').addEventListener('keyup', filterTableByTwoColumns, false);
$('#searchInput').on('keyup', filterTableByTwoColumns);
function filterTableByTwoColumns(event) {
    //console.log(event);
    var filter = event.target.value.toUpperCase();
    var rows = document.querySelector(".dataTable tbody").rows;
    //console.log(rows);
    for (var i = 0; i < rows.length; i++) {
        var firstCol = rows[i].cells[0].textContent.toUpperCase();
        var secondCol = rows[i].cells[1].textContent.toUpperCase();
        //console.log(firstCol.indexOf(filter));
        if (firstCol.indexOf(filter) > -1 || secondCol.indexOf(filter) > -1) {
            rows[i].style.display = "";
            rows[i].classList.add("d-flex");
        } else {
            rows[i].style.display = "none";
            rows[i].classList.remove("d-flex");
        }
    }
}

// row click:
$(".dataTable").click(function addRowHandlers({ target }) {
    console.log("hi")
    if (target.parentElement.localName === "tr") {
        const link = target.parentElement.querySelector("a.btn-details");
        console.log(link);
        if (link) {
            link.click();
        }
    }
});

// sorting icon reverse:
$(".icon").click(function () {
    $(this).toggleClass("rotate");
});

//// color rows:
//$(document).ready(function () {
//    var rows = $('.dataTable > tbody > tr');
//    for (var i = 0; i < rows.length; i++) {
//        if (rows[i].getElementsByClassName('isConfirmed')[0].textContent === 'True') {
//            rows[i].classList.add('table-success');
//        } else {
//            rows[i].classList.add('table-danger');
//        }   
//    }
//});

//// color rows:
//document.querySelectorAll('.dataTable > tbody > tr').forEach(r => {
//    r.addEventListener('load', colorTheRow(r), false);
//})
//function colorTheRow(r) {
//    if (r.getElementsByClassName('isConfirmed')[0].textContent === 'Yes') {
//        //r.classList.add('table-success');
//        r.setAttribute('style', 'background-color: #dcfddc;');
//    } else {
//        //r.classList.add('table-danger');
//        //r.setAttribute('style', 'background-color: #f7eaea;');
//    }
//}

//// row click:
//$(document).ready(function addRowHandlers() {
//    var table = document.querySelector(".dataTable");
//    table.onclick = ({ target }) => {
//        console.log(this);
//        if (target.parentElement.localName === "tr") {
//            const link = target.parentElement.querySelector("a.btn-info");
//            if (link) {
//                link.click();
//            }
//        }
//    }
//});

//// row click:
//$(document).ready(function addRowHandlers() {
//    var table = document.querySelector("#articlesDataTable");
//    var rows = table.getElementsByTagName("tr");
//    for (i = 1; i < rows.length; i++) {
//        var currentRow = table.rows[i];
//        var createClickHandler = function (row) {
//            return function () {
//                //var cell = row.getElementsByTagName("td")[0];
//                var id = row.getAttribute('data-id');
//                //console.log(row);
//                //alert("id:" + id);
//                window.location.href = '/Articles/Details/' + id;
//            };
//        };
//        currentRow.onclick = createClickHandler(currentRow);
//    }
//});
