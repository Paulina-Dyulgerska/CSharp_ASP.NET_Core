$('#SupplierId').change();
$('#SupplierId').change(function () {
    console.log($("#SupplierId").val());
    $.ajax({
        //url: '/Suppliers/GetArticlesById/' + $("#SupplierId").val(),
        url: '/Suppliers/GetArticlesById/' + $(this).val(),
        dataType: 'json',
        success: function (json) {
            html = '<option class="form-control" value=""></option>';
            for (i = 0; i < json.length; i++) {
                html += '<option class="form-control" value="' + json[i].id + '">'
                    + json[i].number + ' - ' + json[i].description +
                    '</option>';
            }
            $('#ArticleId').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error by loading the supplier articles');
            //alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
});

$(document).ready(function () {
    //$('#ValidForSingleArticle').removeAttr('checked')
    $('#ValidForSingleArticle').prop('checked', false);
    $('#ValidForAllArticles').prop('checked', false);
    $('.ArticleId').hide();
});

$('#ValidForAllArticles').change();
$(document).ready(function () {
    $('#ValidForAllArticles').change(function () {
        if ($('#ValidForAllArticles').prop('checked')) {
            $('#ValidForSingleArticle').prop('disabled', true);
        } else {
            $('#ValidForSingleArticle').prop('disabled', false);
        }
    });
});

$('#ValidForSingleArticle').change();
$(document).ready(function () {
    $('#ValidForSingleArticle').change(function () {
        if ($('#ValidForSingleArticle').prop('checked')) {
            $('.ArticleId').show();
            $('#ValidForAllArticles').prop('disabled', true);
        } else {
            $('.ArticleId').hide();
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