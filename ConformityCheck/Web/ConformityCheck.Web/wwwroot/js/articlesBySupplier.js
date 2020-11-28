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

$('#ValidForAllArticles').change();
$(document).ready(function () {
    $('#ValidForAllArticles').change(function () {
        if ($('#ValidForAllArticles').prop('checked')) {
            $('.ArticleId').hide();
            //$('#ArticleId').html("");
            $('#ValidForSingleArticle').prop('disabled', true);
        } else {
            $('.ArticleId').show();
            $('#ValidForSingleArticle').prop('disabled', false);
        }
    });
});

$('#ValidForSingleArticle').change();
$(document).ready(function () {
    $('#ValidForSingleArticle').prop('checked', false)
    $('#ValidForSingleArticle').change(function () {
        if ($('#ValidForSingleArticle').prop('checked')) {
            $('.ArticleId').show();
            $('#ValidForAllArticles').prop('disabled', true);
        } else {
            $('.ArticleId').hide();
            $('#ValidForAllArticles').prop('disabled', false);
            //$('#ArticleId').html("");
        }
    });
});
