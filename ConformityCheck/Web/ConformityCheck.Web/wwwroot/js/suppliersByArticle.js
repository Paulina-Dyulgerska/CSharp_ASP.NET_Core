// article suppliers by article id:
$(document).ready(function () {
    console.log($('#Id').val());
    let selectList = $('#Conformity_SupplierId');
    $.ajax({
        method: 'GET',
        //url: '/Articles/GetSuppliersById/' + $('#Id').val(),
        url: '/api/GetArticleSuppliers/' + $('#Id').val(),
        dataType: 'json',
        success: function (json) {
            selectList.empty();

            let option = document.createElement('option');
            option.text = 'Select supplier';
            selectList.append(option);

            for (i = 0; i < json.length; i++) {
                const supplier = json[i];
                option = document.createElement('option');
                option.value = supplier.id;
                let text = escapeHtml(supplier.name) + ' - ' + escapeHtml(supplier.number);
                if (supplier.isMainSupplier) {
                    text += ' - Main Supplier';
                }
                option.text = text;
                selectList.append(option);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error by loading the article suppliers.');
            //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
});

//conformity types by article id and choosen supplier
$('#Conformity_SupplierId').change(function () {
    //var data = JSON.stringify({
    //    articleId: $('#Id').val(),
    //    supplierId: $('#Conformity_SupplierId').val(),
    //});
    let selectList = $('#Conformity_ConformityTypeId');
    $.ajax({
        method: 'GET',
        //url: '/Articles/GetConformityTypesByIdAndSupplier',
        url: '/api/GetArticleConformityTypesBySupplier',
        dataType: 'json',
        data: { 'articleId': $('#Id').val(), 'supplierId': $('#Conformity_SupplierId').val() },
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            selectList.empty();

            let option = document.createElement('option');
            option.text = 'Select conformity type';
            selectList.append(option);

            for (i = 0; i < json.length; i++) {
                const conformityType = json[i];
                option = document.createElement('option');
                option.value = conformityType.id;
                let text = escapeHtml(conformityType.description);
                if (conformityType.supplierConfirmed) {
                    text += ' - Confirmed by this supplier';
                } else {
                    text += ' - Not confirmed by this supplier';
                }
                option.text = text;
                selectList.append(option);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error by loading the article conformity types');
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
})
