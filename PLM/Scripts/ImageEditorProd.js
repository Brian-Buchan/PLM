$(function ($) {
    //get model data from view, which was designated in the script tag with data-* attributes. 
    //Taken from arserbin3 at http://stackoverflow.com/questions/7626662/access-a-model-property-in-a-javascript-file
    var $vars = $('#jslink').data();

    var imgEdit = document.getElementById($vars.imageId);
    var imgIdInput = document.getElementById("imgId");
    var answerIdInput = document.getElementById("answerId");
    var imgDataInput = document.getElementById("imgData");
    var origUrlInput = document.getElementById("origUrl");
    var tempUrlInput = document.getElementById("tempUrl");

    var myPixie = Pixie.setOptions({
        replaceOriginal: true,
        appendTo: 'body',
        onSave: function (data, img) {
            $.ajax({
                type: 'POST',
                url: '/PerceptualLearning/Pictures/ImageEditor',
                data: { imgData: data, imgId: img.id, answerId: img.title, origUrl: img.dataset.origSource },
                success: function (xhr, status, text) {
                    imgIdInput.value = imgEdit.id;
                    answerIdInput.value = imgEdit.title;
                    imgDataInput.value = imgEdit.src;
                    origUrlInput.value = imgEdit.dataset.origSource;
                    tempUrlInput.value = text.statusText;

                    alert('Image changes have been saved successfully. \nRemember to ' +
                    'click the save button to finalize your changes.');
                    myPixie.close();
                },
                error: function (xhr, status, text) {
                    alert(text);
                    myPixie.close();
                }
            });
        }
    });

    $('#' + $vars.imageId).on('click', function (e) {
        if (e.target.src !== "/PerceprtualLearning/Content/Images/Error.bmp") {
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