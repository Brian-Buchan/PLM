$(function ($) {
    //get model data from view, which was designated in the script tag with data-* attributes. 
    //Taken from arserbin3 at http://stackoverflow.com/questions/7626662/access-a-model-property-in-a-javascript-file
    var $vars = $('#jslink').data();

    var imgEdit = document.getElementById($vars.imageId);
    var imgIdInput = document.getElementById("imageId");
    var answerIdInput = document.getElementById("answerId");
    var newImgDataInput = document.getElementById("newImgData");
    var originalImgDataInput = document.getElementById("originalImgData");

    var myPixie = Pixie.setOptions({
        replaceOriginal: true,
        appendTo: 'body',
        onSave: function (data, img) {
            $.ajax({
                type: 'POST',
                url: '/Pictures/ImageEditor',
                data: { imageData: data, imgId: img.id, answerId: img.title, origURL: img.dataset.origSource },
                success: function (xhr, status, text) {
                    imgIdInput.value = imgEdit.id;
                    answerIdInput.value = imgEdit.title;
                    newImgDataInput.value = imgEdit.src;
                    originalImgDataInput.value = imgEdit.dataset.origSource;

                    myPixie.close();
                },
                error: function (xhr, status, text) {
                    myPixie.close();
                }
            });
        }
    });

    $('#' + $vars.imageId).on('click', function (e) {
        if (e.target.src !== "/Content/Images/Error.bmp") {
            myPixie.open({
                url: e.target.src,
                image: e.target
            });
        }
        else {
            alert("We can't seem to find your image. Try deleting this one, then re-upload the image.");
        }
    });
});