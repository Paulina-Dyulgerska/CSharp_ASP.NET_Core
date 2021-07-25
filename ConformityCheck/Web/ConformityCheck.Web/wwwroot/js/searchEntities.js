var entityMap = {
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

// search articles:
//$('#suggestions.articles').hide();
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
                    $('#suggestions.articles').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var article = json[i];
                        html += '<option class="form-control" value="' + escapeHtml(article.id) + '">'
                            + escapeHtml(article.number) + ' - '
                            + escapeHtml(article.description) + '</option>';
                    }
                    $('#suggestions.articles').show();
                    $('#selectSearchArticleInput').html(html);
                    //document.createTextNode
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading articles');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions.articles').hide();
    }
});


// search suppliers:
//$('#suggestions.suppliers').hide();
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
                    $('#suggestions.suppliers').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var supplier = json[i];
                        html += '<option class="form-control" value="' + escapeHtml(supplier.id) + '">'
                            + escapeHtml(supplier.number) + ' - ' + escapeHtml(supplier.name) +
                            '</option>';
                    }
                    $('#suggestions.suppliers').show();
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
        //$('#suggestions.suppliers').hide();
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
                    $('#suggestions.conformity-type').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var conformityType = json[i];
                        html += '<option class="form-control" value="' + escapeHtml(conformityType.id) + '">'
                            + escapeHtml(conformityType.description) + '</option>';
                    }
                    $('#suggestions.conformity-type').show();
                    $('#selectSearchConformityTypeInput').html(html);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformity types');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions.conformity-type').hide();
    }
});

// search conformities:
//$('#suggestions').hide();
$('#searchConformityInput').on('keyup', function () {
    console.log($('#searchConformityInput').val());
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            url: '/Conformities/GetByArticleOrSupplierOrConformityType',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    $('#suggestions.conformity').hide();
                }
                else {
                    let html = '';
                    for (i = 0; i < json.length; i++) {
                        var conformity = json[i];
                        html += '<option class="form-control" value="' + escapeHtml(conformity.id) + '">'
                            + escapeHtml(conformity.article.number) + ' - '
                            + escapeHtml(conformity.supplier.name) + '</option>';
                    }
                    $('#suggestions.conformity').show();
                    $('#selectSearchConformityInput').html(html);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformities');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //$('#suggestions.conformity').hide();
    }
});