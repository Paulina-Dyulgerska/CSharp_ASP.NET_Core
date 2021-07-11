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
//document.querySelector('#searchInput').addEventListener('keyup', filterTableByTwoColumns, false);
$('#searchInput').on('keyup', filterTableByTwoColumns);
function filterTableByTwoColumns(event) {
    //console.log(event);
    var filter = event.target.value.toUpperCase();
    var rows = document.querySelector("#dataTable tbody").rows;
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

//// color rows:
//$(document).ready(function () {
//    var rows = $('#dataTable > tbody > tr');
//    for (var i = 0; i < rows.length; i++) {
//        if (rows[i].getElementsByClassName('isConfirmed')[0].textContent === 'True') {
//            rows[i].classList.add('table-success');
//        } else {
//            rows[i].classList.add('table-danger');
//        }   
//    }
//});

//// color rows:
//document.querySelectorAll('#dataTable > tbody > tr').forEach(r => {
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
//    var table = document.querySelector("#dataTable");
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

//// row click:
$("#dataTable").click(function addRowHandlers({ target }) {
    if (target.parentElement.localName === "tr") {
        const link = target.parentElement.querySelector("a.btn-details");
        if (link) {
            link.click();
        }
    }
});

// search article:
//$('#suggestions').hide();
$('#searchAticleInput').on('keyup', function () {
    console.log($('#searchAticleInput').val());
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            url: '/Articles/GetByNumberOrDescription',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    $('#suggestions').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var article = json[i];
                        html += '<option class="form-control" value="' + article.id + '">'
                            + article.number + ' - ' + article.description +
                            '</option>';
                    }
                    $('#suggestions').show();
                    $('#selectSearchArticleInput').html(html);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading articles');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions').hide();
    }
});


// search supplier:
//$('#suggestions').hide();
$('#searchSupplierInput').on('keyup', function () {
    console.log($('#searchSupplierInput').val());
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            url: '/Suppliers/GetByNumberOrName',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    $('#suggestions').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var supplier = json[i];
                        html += '<option class="form-control" value="' + supplier.id + '">'
                            + supplier.number + ' - ' + supplier.name +
                            '</option>';
                    }
                    $('#suggestions').show();
                    $('#selectSearchSupplierInput').html(html);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading suppliers');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions').hide();
    }
});

// search conformity types:
//$('#suggestions').hide();
$('#searchConformityTypeInput').on('keyup', function () {
    console.log($('#searchConformityTypeInput').val());
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            url: '/ConformityTypes/GetByIdOrDescription',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    $('#suggestions').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var conformityType = json[i];
                        html += '<option class="form-control" value="' + conformityType.id + '">'
                            + conformityType.description + '</option>';
                    }
                    $('#suggestions').show();
                    $('#selectSearchConformityTypeInput').html(html);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading suppliers');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions').hide();
    }
});