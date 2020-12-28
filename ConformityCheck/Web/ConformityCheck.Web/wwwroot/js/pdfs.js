$(function () {
    $('a.openPdf').click(function () {
        var url = $(this).attr('href');
        var windowWidth = 600;
        var windowHeight = 800;
        var x = (screen.width - windowWidth) / 2;
        var y = (screen.height - windowHeight) / 2;
        window.open(url, 'popupWindow',
            'width = ' + windowWidth
            + ', height= ' + windowHeight
            + ', scrollbars = yes, left = ' + x + ', top = ' + y);
    });
    return false;
});

$(function () {
    $('a.addPdf').click(function () {
        var divForInputFile = $('#inputFile');
        divForInputFile.removeAttr('hidden');
    });
});