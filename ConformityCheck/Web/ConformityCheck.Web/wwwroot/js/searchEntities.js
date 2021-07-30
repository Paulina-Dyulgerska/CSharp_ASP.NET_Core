// search articles: safer than the function directly writing in the select's html:
$('#searchAticleInput').on('keyup', function () {
    console.log($('#searchAticleInput').val());
    let selectList = $('#selectSearchArticleInput');
    let suggestions = $('#suggestions.articles');
    //let recaptchaValue = $('#RecaptchaValue').val();
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            //method: 'POST',
            method: 'GET',
            //url: '/Articles/GetByNumberOrDescription',
            url: '/api/GetArticlesByNumberOrDescription',
            contentType: 'application/json',
            dataType: 'json',
            //data: JSON.stringify({ 'input': $(this).val(), 'recaptchaValue': recaptchaValue }),
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    suggestions.hide();
                }
                else {
                    selectList.empty();
                    for (i = 0; i < json.length; i++) {
                        const article = json[i];
                        let option = document.createElement('option');
                        option.value = article.id;
                        option.text = escapeHtml(article.number) + ' - ' + escapeHtml(article.description)
                        selectList.append(option);
                    }
                    suggestions.show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading articles');
                console.log(xhr.statusText);
                console.log(xhr.responseText);
                console.log(xhr);
                console.log(thrownError);
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //suggestions.hide();
    }
});

// search suppliers: safer than the function directly writing in the select's html:
$('#searchSupplierInput').on('keyup', function () {
    console.log($('#searchSupplierInput').val());
    let selectList = $('#selectSearchSupplierInput');
    let suggestions = $('#suggestions.suppliers');
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            //url: '/Suppliers/GetByNumberOrName',
            url: '/api/GetSuppliersByNumberOrName',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    suggestions.hide();
                }
                else {
                    selectList.empty();
                    for (i = 0; i < json.length; i++) {
                        const supplier = json[i];
                        let option = document.createElement('option');
                        option.value = supplier.id;
                        option.text = escapeHtml(supplier.number) + ' - ' + escapeHtml(supplier.name);
                        selectList.append(option);
                    }
                    suggestions.show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading suppliers');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //suggestions.hide();
    }
});

// search conformity types: safer than the function directly writing in the select's html:
$('#searchConformityTypeInput').on('keyup', function () {
    console.log($('#searchConformityTypeInput').val());
    let selectList = $('#selectSearchConformityTypeInput');
    let suggestions = $('#suggestions.conformity-type');
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            //url: '/ConformityTypes/GetByIdOrDescription',
            url: '/api/GetConformityTypeByIdOrDescription',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    suggestions.hide();
                }
                else {
                    selectList.empty();
                    for (i = 0; i < json.length; i++) {
                        const conformityType = json[i];
                        let option = document.createElement('option');
                        option.value = conformityType.id;
                        option.text = escape(conformityType.description);
                        selectList.append(option);
                    }
                    suggestions.show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformity types');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //suggestions.hide();
    }
});

// search conformities: safer than the function directly writing in the select's html:
$('#searchConformityInput').on('keyup', function () {
    console.log($('#searchConformityInput').val());
    let selectList = $('#selectSearchConformityInput');
    let suggestions = $('#suggestions.conformity');
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            //url: '/Conformities/GetByArticleOrSupplierOrConformityType',
            url: '/api/GetConformitiesByArticleOrSupplierOrConformityType',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                if (json.length < 1) {
                    suggestions.hide();
                }
                else {
                    selectList.empty();
                    for (i = 0; i < json.length; i++) {
                        const conformity = json[i];
                        let option = document.createElement('option');
                        option.value = conformity.id;
                        option.text = escapeHtml(conformity.article.number) + ' - ' + escapeHtml(conformity.supplier.name)
                        selectList.append(option);
                    }
                    suggestions.show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformities');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        //suggestions.hide();
    }
});

//// search articles:
////$('#suggestions.articles').hide();
//$('#searchAticleInput').on('keyup', function () {
//    console.log($('#searchAticleInput').val());
//    if (!isNullOrWhitespace($(this).val())) {
//        $.ajax({
//            method: 'GET',
//            url: '/Articles/GetByNumberOrDescription',
//            dataType: 'json',
//            data: { 'input': $(this).val() },
//            success: function (json) {
//                if (json.length < 1) {
//                    $('#suggestions.articles').hide();
//                }
//                else {
//                    let html = '';
//                    for (i = 0; i < json.length; i++) {
//                        var article = json[i];
//                        html += '<option class="form-control" value="' + escapeHtml(article.id) + '">'
//                            + escapeHtml(article.number) + ' - '
//                            + escapeHtml(article.description) + '</option>';
//                    }
//                    $('#suggestions.articles').show();
//                    $('#selectSearchArticleInput').html(html);
//                    //document.createTextNode
//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                alert('Error by loading articles');
//                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
//            }
//        });
//    }
//    else {
//        //$('#suggestions.articles').hide();
//    }
//});

//// search suppliers:
////$('#suggestions.suppliers').hide();
//$('#searchSupplierInput').on('keyup', function () {
//    console.log($('#searchSupplierInput').val());
//    if (!isNullOrWhitespace($(this).val())) {
//        $.ajax({
//            method: 'GET',
//            url: '/Suppliers/GetByNumberOrName',
//            dataType: 'json',
//            data: { 'input': $(this).val() },
//            success: function (json) {
//                if (json.length < 1) {
//                    $('#suggestions.suppliers').hide();
//                }
//                else {
//                    let html = '';
//                    for (i = 0; i < json.length; i++) {
//                        var supplier = json[i];
//                        html += '<option class="form-control" value="' + escapeHtml(supplier.id) + '">'
//                            + escapeHtml(supplier.number) + ' - ' + escapeHtml(supplier.name) +
//                            '</option>';
//                    }
//                    $('#suggestions.suppliers').show();
//                    $('#selectSearchSupplierInput').html(html);
//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                alert('Error by loading suppliers');
//                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
//            }
//        });
//    }
//    else {
//        //$('#suggestions.suppliers').hide();
//    }
//});

//// search conformity types:
//$('#searchConformityTypeInput').on('keyup', function () {
//    console.log($('#searchConformityTypeInput').val());
//    if (!isNullOrWhitespace($(this).val())) {
//        $.ajax({
//            method: 'GET',
//            url: '/ConformityTypes/GetByIdOrDescription',
//            dataType: 'json',
//            data: { 'input': $(this).val() },
//            success: function (json) {
//                if (json.length < 1) {
//                    $('#suggestions.conformity-type').hide();
//                }
//                else {
//                    let html = '';
//                    for (i = 0; i < json.length; i++) {
//                        var conformityType = json[i];
//                        html += '<option class="form-control" value="' + escapeHtml(conformityType.id) + '">'
//                            + escapeHtml(conformityType.description) + '</option>';
//                        console.log(html);
//                    }
//                    $('#suggestions.conformity-type').show();
//                    $('#selectSearchConformityTypeInput').html(html);
//                    //$('#selectSearchConformityTypeInput').html(html);
//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                alert('Error by loading conformity types');
//                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
//            }
//        });
//    }
//    else {
//        //$('#suggestions.conformity-type').hide();
//    }
//});


//// search conformities:
//$('#searchConformityInput').on('keyup', function () {
//    console.log($('#searchConformityInput').val());
//    if (!isNullOrWhitespace($(this).val())) {
//        $.ajax({
//            method: 'GET',
//            url: '/Conformities/GetByArticleOrSupplierOrConformityType',
//            dataType: 'json',
//            data: { 'input': $(this).val() },
//            success: function (json) {
//                if (json.length < 1) {
//                    $('#suggestions.conformity').hide();
//                }
//                else {
//                    let html = '';
//                    for (i = 0; i < json.length; i++) {
//                        var conformity = json[i];
//                        html += '<option class="form-control" value="' + escapeHtml(conformity.id) + '">'
//                            + escapeHtml(conformity.article.number) + ' - '
//                            + escapeHtml(conformity.supplier.name) + '</option>';
//                    }
//                    $('#suggestions.conformity').show();
//                    $('#selectSearchConformityInput').html(html);
//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                alert('Error by loading conformities');
//                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
//            }
//        });
//    }
//    else {
//        //$('#suggestions.conformity').hide();
//    }
//});