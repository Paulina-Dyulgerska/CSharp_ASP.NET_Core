$(document).ready(function () {
    console.log($('#Id').val());
    $.ajax({
        url: '/Articles/GetSuppliersById/' + $('#Id').val(),
        dataType: 'json',
        success: function (json) {
            let html = '<option class="form-control" value="">Select Supplier</option>';
            for (i = 0; i < json.length; i++) {
                var supplier = json[i];
                html += '<option class="form-control" value="' + supplier.id + '">'
                    + supplier.name + ' - ' + supplier.number;
                if (supplier.isMainSupplier) {
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
            let html = '<option class="form-control" value="">Select conformity type</option>';
            for (i = 0; i < json.length; i++) {
                var conformityType = json[i];
                html += '<option class="form-control" value="' + conformityType.id + '">'
                    + conformityType.description;
                if (conformityType.supplierConfirmed) {
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
