$(document).ready(function () {
    //$('#ValidForSingleArticle').removeAttr('checked')
    $('#ValidForSingleArticle').prop('checked', false);
    $('#ValidForAllArticles').prop('checked', false);
    $('.articles-container').hide();
});

$('#SupplierId').change(function () {
    $('.articles-container').hide();
    $('#ArticleId').html(null);
    console.log($("#SupplierId").val());
    $.ajax({
        //url: '/Suppliers/GetArticlesById/' + $("#SupplierId").val(),
        url: '/Suppliers/GetArticlesById/' + $(this).val(),
        dataType: 'json',
        success: function (json) {
            let html = '<option class="form-control" value=""></option>';
            if (json.length >= 1) {
                for (i = 0; i < json.length; i++) {
                    var article = json[i];
                    html += '<option class="form-control" value="' + article.id + '">'
                        + article.number + ' - ' + article.description +
                        '</option>';
                }
                $('#ArticleId').html(html);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
            console.log(ajaxOptions);
            console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            alert('Error by loading the supplier articles');
            //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
});

$(document).ready(function () {
    $('#ValidForAllArticles').change(function () {
        if ($('#ValidForAllArticles').prop('checked')) {
            $('#ValidForSingleArticle').prop('disabled', true);
        } else {
            $('#ValidForSingleArticle').prop('disabled', false);
        }
    });
});

$(document).ready(function () {
    $('#ValidForSingleArticle').change(function () {
        if ($('#ValidForSingleArticle').prop('checked')) {
            $('.articles-container').show();
            $('#ValidForAllArticles').prop('disabled', true);
        } else {
            $('.articles-container').hide();
            $('#ValidForAllArticles').prop('disabled', false);
        }
    });
});

//$('body > div > main > div > div.col-10 > div > form > div:nth-child(12) > input').submit();
//$(document).ready(function () {
//    //$('#ValidForSingleArticle').removeAttr('checked')
//    $('body > div > main > div > div.col-10 > div > form > div:nth-child(12) > input').submit(function (e) {
//        console.log('Hi 1')
//        if (!($('#ValidForSingleArticle').prop('checked') && $('#ArticleId').select())) {
//            console.log('Hi 2')
//            e.preventDefault();
//            alert('Please select an article!');
//            return;
//        } else {
//            console.log('Hi 3')
//        }
//    });
//});

//$(document).ready(function ($) {
//    console.log('Hi 3');
//    var allowSubmit = false;
//    $('#btn').click(function (event) {
//        console.log('Hi 4');
//        var form = $(".col-md-6 mx-auto");
//        if (form.valid() && !allowSubmit) {
//            event.preventDefault();
//            //run our functions
//            _ourfunction(test, {
//                success: function (data) {
//                    if (!($('#ValidForSingleArticle').prop('checked') && $('#ArticleId').select())) {
//                        console.log('Hi 2');
//                        alert('Please select an article!');
//                        return;
//                    } else {
//                        console.log('Hi 3');
//                    }
//                },
//                error: function (data) {
//                    console.log("error");
//                }
//            });
//            allowSubmit = true;
//        }
//    });
//});