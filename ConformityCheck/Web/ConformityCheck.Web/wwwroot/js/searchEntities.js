let dropDownContent = $('.dropdown-content');
dropDownContent.hide();

// search articles: safer than the function directly writing in the select's html:
$('#searchAticleInput').on('input', function () {
    let dropDownList = $('#selectSearchArticleInput');
    dropDownList.hide();
    //let recaptchaValue = $('#RecaptchaValue').val();
    if (!isNullOrWhitespace($(this).val()) && $(this).val().length > 2) {
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
                dropDownList.empty();
                dropDownList.show();
                for (i = 0; i < json.length; i++) {
                    const article = json[i];
                    const articleDetailsLink = "/Articles/Details/" + article.id;
                    let articleLinkElement = document.createElement('a');
                    articleLinkElement.href = articleDetailsLink;
                    articleLinkElement.text = article.number + ' - ' + article.description;
                    //articleLinkElement.text = escapeHtml(article.number) + ' - ' + article.description;
                    articleLinkElement.classList.add('form-control');
                    dropDownList.append(articleLinkElement);
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
        dropDownList.hide();
    }
});

// search suppliers: safer than the function directly writing in the select's html:
$('#searchSupplierInput').on('input', function () {
    let dropDownList = $('#selectSearchSupplierInput');
    dropDownList.hide();
    if (!isNullOrWhitespace($(this).val()) && $(this).val().length > 2) {
        $.ajax({
            method: 'GET',
            //url: '/Suppliers/GetByNumberOrName',
            url: '/api/GetSuppliersByNumberOrName',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                dropDownList.empty();
                dropDownList.show();
                for (i = 0; i < json.length; i++) {
                    const supplier = json[i];
                    const supplierDetailsLink = "/Suppliers/Details/" + supplier.id;
                    let supplierLinkElement = document.createElement('a');
                    supplierLinkElement.href = supplierDetailsLink;
                    supplierLinkElement.text = supplier.number + ' - ' + supplier.name;
                    supplierLinkElement.classList.add('form-control');
                    dropDownList.append(supplierLinkElement);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading suppliers');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        dropDownList.hide();
    }
});

// search conformity types: safer than the function directly writing in the select's html:
$('#searchConformityTypeInput').on('input', function () {
    let dropDownList = $('#selectSearchConformityTypeInput');
    dropDownList.hide();
    if (!isNullOrWhitespace($(this).val())) {
        $.ajax({
            method: 'GET',
            //url: '/ConformityTypes/GetByIdOrDescription',
            url: '/api/GetConformityTypeByIdOrDescription',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                dropDownList.empty();
                dropDownList.show();
                for (i = 0; i < json.length; i++) {
                    const conformityType = json[i];
                    const conformityTypeDetailsLink = "/ConformityTypes/Details/" + conformityType.id;
                    let conformityTypeLinkElement = document.createElement('a');
                    conformityTypeLinkElement.href = conformityTypeDetailsLink;
                    conformityTypeLinkElement.text = conformityType.description;
                    conformityTypeLinkElement.classList.add('form-control');
                    dropDownList.append(conformityTypeLinkElement);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformity types');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        dropDownList.hide();
    }
});

// search conformities: safer than the function directly writing in the select's html:
$('#searchConformityInput').on('input', function () {
    let dropDownList = $('#selectSearchConformityInput');
    dropDownList.hide();
    if (!isNullOrWhitespace($(this).val()) && $(this).val().length > 1) {
        $.ajax({
            method: 'GET',
            //url: '/Conformities/GetByArticleOrSupplierOrConformityType',
            url: '/api/GetConformitiesByArticleOrSupplierOrConformityType',
            dataType: 'json',
            data: { 'input': $(this).val() },
            success: function (json) {
                dropDownList.empty();
                dropDownList.show();
                    for (i = 0; i < json.length; i++) {
                        const conformity = json[i];
                        const conformityDetailsLink = "/Conformities/Details/" + conformity.id;
                        let conformityLinkElement = document.createElement('a');
                        conformityLinkElement.href = conformityDetailsLink;
                        conformityLinkElement.text = conformity.article.number + ' - ' + conformity.supplier.name;
                        conformityLinkElement.classList.add('form-control');
                        dropDownList.append(conformityLinkElement);
                    }
                    suggestions.show();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error by loading conformities');
                //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
    else {
        dropDownList.hide();
    }
});

// search users: safer than the function directly writing in the select's html:
$('#searchUserInput').on('input', function () {
    let dropDownList = $('#selectSearchUserInput');
    dropDownList.hide();
    //let recaptchaValue = $('#RecaptchaValue').val();
    if (!isNullOrWhitespace($(this).val()) && $(this).val().length > 2) {
        $.ajax({
            //method: 'POST',
            method: 'GET',
            url: '/api/GetUserByUsernameOrEmailOrRole',
            contentType: 'application/json',
            dataType: 'json',
            //data: JSON.stringify({ 'input': $(this).val(), 'recaptchaValue': recaptchaValue }),
            data: { 'input': $(this).val() },
            success: function (json) {
                dropDownList.empty();
                dropDownList.show();
                for (i = 0; i < json.length; i++) {
                    const user = json[i];
                    const userDetailsLink = "/Administration/Users/Details/" + user.id;
                    let userLinkElement = document.createElement('a');
                    userLinkElement.href = userDetailsLink;
                    userLinkElement.text = user.email;
                    //userLinkElement.text = escapeHtml(user.email);
                    userLinkElement.classList.add('form-control');
                    dropDownList.append(userLinkElement);
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
        dropDownList.hide();
    }
});