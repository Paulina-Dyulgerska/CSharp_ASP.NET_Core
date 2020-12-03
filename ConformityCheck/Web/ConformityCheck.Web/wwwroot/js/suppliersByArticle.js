$(document).ready(function () {
    console.log($('#Id').val());
    $.ajax({
        url: '/Articles/GetSuppliersById/' + $('#Id').val(),
        dataType: 'json',
        success: function (json) {
            html = '<option class="form-control" value="">Select Supplier</option>';
            for (i = 0; i < json.length; i++) {
                html += '<option class="form-control" value="' + json[i].id + '">'
                    + json[i].name + ' - ' + json[i].number;
                if (json[i].isMainSupplier) {
                    html += ' - Main Supplier';
                }
                html += '</option>';
            }
            $('#Conformity_SupplierId').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error by loading the supplier articles');
            //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });

    
});

$('#Conformity_SupplierId').change();
$('#Conformity_SupplierId').change(function () {
    //var data = JSON.stringify({
    //    articleId: $('#Id').val(),
    //    supplierId: $('#Conformity_SupplierId').val(),
    //});

    $.ajax({
        method: 'GET',
        url: '/Articles/GetConformityTypesByIdAndSupplier',
        dataType: 'json',
        data: { 'articleId': $('#Id').val(), 'supplierId': $('#Conformity_SupplierId').val() },
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            html = '<option class="form-control" value="">Select conformity type</option>';
            for (i = 0; i < json.length; i++) {
                html += '<option class="form-control" value="' + json[i].id + '">'
                    + json[i].description;
                if (json[i].SupplierConfirmed) {
                    html += ' - Confirmed by this supplier';
                } else {
                    html += ' - Not confirmed by this supplier';
                }

                html += '</option>';
            }
            $('#Conformity_ConformityTypeId').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error by loading the supplier articles');
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
})
